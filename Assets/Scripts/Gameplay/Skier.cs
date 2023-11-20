using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Skier : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _MaxSpeed;
    [SerializeField]
    private float _Acceleration;
    [SerializeField]
    private float _JumpForce = 1.0f;
    [SerializeField]
    private float _AcrobaticJumpForce = 2.0f;
    [SerializeField]
    private float _Gravity;

    private float _horizontalDir = .0f;
    private float _verticalDir = .0f;
    private float _currentSpeed = .0f;

    private bool _grounded = true;
    private bool _acrobatic = false;
    private bool _paused = false;

    private Vector3 _startingPosition;

    private void OnEnable()
    {
        _startingPosition = transform.position;

        // subcribing to pause and gameover events
        /*
        LevelSystem.Instance._OnPauseEvent.Subscribe(PauseSkier);
        LevelSystem.Instance._OnResumeEvent.Subscribe(ResumeSkier);
        LevelSystem.Instance._OnGameOverEvent.Subscribe(GameOverSkier);
        */
    }

    private void OnDisable()
    {
        // subcribing to pause and gameover events
        /*
        LevelSystem.Instance._OnPauseEvent.Unsubscribe(PauseSkier);
        LevelSystem.Instance._OnResumeEvent.Unsubscribe(ResumeSkier);
        LevelSystem.Instance._OnGameOverEvent.Unsubscribe(GameOverSkier);
        */
    }

    #region MOVEMENT

    // return current speed
    public float GetSpeed()
    {
        return _currentSpeed;
    }

    public void IncreaseMaxSpeed(float increase)
    {
        _MaxSpeed += increase;
    }

    // reset the speed to stop the skier
    public void ResetSpeed()
    {
        _currentSpeed = .0f;
    }

    // flip skier's sprite basing on sign of the input
    private void Flip(float value)
    {
        // mirror sprite
        if (value < .0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (value > .0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
            
    }

    // move 45° diagonally left or right
    public void HorizontalMove(float input)
    {
        // 45° left or right means that the projection on the left direction is half the direction => 0.5
        _horizontalDir = input * .5f;

        Flip(input);
    }

    // jump -> add vertical component to direction
    public void Jump()
    {
        // if already jumping
        if (transform.position.z > .0f)
            return;

        if (_acrobatic)
            _verticalDir = _AcrobaticJumpForce;
        else
            _verticalDir = _JumpForce;

        _grounded = false;

        // disable box collider
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // acrobatic jump
    public void AcrobaticJump()
    {
        _acrobatic = true;
    }

    public void StandardJump()
    {
        _acrobatic = false;
    }

    public bool IsAcrobatic()
    {
        return _acrobatic;
    }

    public bool IsGrounded()
    {
        return _grounded;
    }

    public float GetJumpTime()
    {
        if (_acrobatic)
            return (_AcrobaticJumpForce / -_Gravity) * 2.0f;

        return (_JumpForce / -_Gravity) * 2.0f;
    }

    // Update position
    void Update()
    {
        _currentSpeed += _Acceleration * Time.deltaTime;

        if (_currentSpeed > _MaxSpeed)
            _currentSpeed = _MaxSpeed;

        // move in current direction
        Vector3 hDir = new Vector3(_horizontalDir, .0f, .0f);
        Vector3 vDir = new Vector3 (.0f, -_verticalDir, _verticalDir);
        transform.position += hDir * _currentSpeed * Time.deltaTime;
        transform.position += vDir * Time.deltaTime;

        // change scale for jump sensation
        transform.localScale = new Vector3(transform.position.z + 1, transform.position.z + 1, 1.0f);

        // gravity
        if (transform.position.z > .0f)
        {
            _verticalDir += _Gravity * Time.deltaTime;
        }
        else if (transform.position.z < .0f)
        {
            transform.position = new Vector3(transform.position.x, _startingPosition.y, _startingPosition.z);
            _verticalDir = .0f;
            _grounded = true;
            _acrobatic = false;

            // reset scale when grounded
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            // enable box collider
            GetComponent<BoxCollider2D>().enabled = true;
        }

        // correct player's position to loop on x axis
        float maxHorizontalDelta = LevelSystem.Instance.GetSlopeWidth() / 2.0f;
        // positive delta
        transform.position = new Vector3(((transform.position.x + maxHorizontalDelta) % (maxHorizontalDelta * 2.0f)) - maxHorizontalDelta, transform.position.y, transform.position.z);
        // negative delta
        transform.position = new Vector3(((transform.position.x - maxHorizontalDelta) % (maxHorizontalDelta * -2.0f)) + maxHorizontalDelta, transform.position.y, transform.position.z);
    }

    #endregion MOVEMENT

    #region COLLISION

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SlopeObstacle>() == null || LevelSystem.Instance.IsSnowballActive())
            return;

        // reset speed
        _currentSpeed = .0f;
    }

    #endregion COLLISION

    #region EVENTS
    /*
    private void PauseSkier(GameEvent evt)
    {
        _paused = true;
        StartCoroutine(WaitForResume());
    }

    private IEnumerator WaitForResume()
    {
        // save speed data
        float max = _MaxSpeed;
        _MaxSpeed = .0f;

        // wait
        yield return new WaitUntil(() => !_paused);

        // reset data
        _MaxSpeed = max;
    }

    private void ResumeSkier(GameEvent evt)
    {
        _paused = false;
    }

    private void GameOverSkier(GameEvent evt)
    {
        _MaxSpeed = .0f;
    }
    */

    #endregion EVENTS

    public void MoveToPoint(Vector3 point)
    {
        GetComponent<SpriteRenderer>().flipX = false;
        
        StartCoroutine(MoveToPointCoroutine(point));
    }
    private IEnumerator MoveToPointCoroutine(Vector3 point)
    {
        float vectorDistancex = point.x - transform.position.x;
        float vectorDistancey = transform.position.y - point.y;
        float t = vectorDistancey/_currentSpeed;
        float increment = vectorDistancex / t;
        float currentIncrement = 0;
        float positiveVectorDistancex = Mathf.Abs(vectorDistancex);
        float positiveIncrement= Mathf.Abs(increment);
        while (currentIncrement < positiveVectorDistancex)
        {
            transform.position += new Vector3(increment * Time.deltaTime,0f,0f);
            currentIncrement += positiveIncrement * Time.deltaTime;
            yield return null;
        }
    }
}
