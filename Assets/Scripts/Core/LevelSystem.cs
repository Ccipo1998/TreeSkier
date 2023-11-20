using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class LevelSystem : Singleton<LevelSystem>
{



    [Header("Scene data")]
    [SerializeField]
    private string _SceneName;

    [Header("Level Data")]
    [SerializeField]
    private Skier _Skier;

    [SerializeField]
    private uint _MaxLifes;
    [SerializeField]
    private float _SlopeWidth;
    [SerializeField]
    private float _SlopeHeight;
    [SerializeField]
    private GameObject _OutOfScopePoint;
    [SerializeField]
    private GameObject _InScopePoint;

    [Header("Snowball Data")]
    [SerializeField]
    [Tooltip("Seconds")]
    private uint _SnowballChargeTime;
    [SerializeField]
    private float _SnowflakeGain;

    [Header("Spawner")]
    [SerializeField]
    private Spawner _Spawner;

    private uint _score;
    private uint _lives;
    
    [Header("Event Data")]
    [SerializeField]
    private SlopeObjectGameEvent _BombHitGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _RampHitGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _RampExitHitGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _X2HitGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _HeartHitGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _CoinHitGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _TreeHitGameEvent;
    [SerializeField]
    private SlopeObjectGameEvent _OffFogGameEvent;
    [SerializeField]
    private SpawnContainerGameEvent _OutOfScopeEvent;
    [SerializeField]
    private SpawnContainerGameEvent _InScopeEvent;
    [SerializeField]
    private GameEvent _OnPauseEvent;
    [SerializeField]
    private GameEvent _OnResumeEvent;
    [SerializeField]
    private GameEvent _OnGameOverEvent;
    [SerializeField]
    private GameEvent _OnRestartEvent;

    [Header("Flow system events")]
    [SerializeField]
    private string _OnRampFlowEvent;
    [SerializeField]
    private string _OffRampFlowEvent;
    [SerializeField]
    private string _OnAcrobaticStartFlowEvent;
    [SerializeField]
    private string _OnAcrobaticEndFlowEvent;
    [SerializeField]
    private string _OnResumeFlowEvent;
    [SerializeField]
    private string _OnRestartFlowEvent;

    [Header("Difficulty")]
    [SerializeField]
    private float _TimeStep = 10.0f;
    [SerializeField]
    private float _SpeedIncrease = .2f;
    [SerializeField]
    private float _SpawnDensityIncrease = .1f;
    [SerializeField]
    private float _SecondsFog = 45f;

    [Header("UI")]
    [SerializeField]
    private idContainer _PlaygroundViewControllerID;

    [Header("Audio")]
    [SerializeField]
    private idContainer _JumpAudioId;
    [SerializeField]
    private idContainer _BigJumpAudioId;
    [SerializeField]
    private idContainer _BombAudioId;
    [SerializeField]
    private idContainer _TreeHitAudioId;
    [SerializeField]
    private idContainer _X2AudioId;
    [SerializeField]
    private idContainer _SnowflakeAudioId;
    [SerializeField]
    private idContainer _CoinAudioId;
    [SerializeField]
    private idContainer _HeathAudioId;
    [SerializeField]
    private idContainer _FallAudioId;
    [SerializeField]
    private idContainer _JumpSuccessAudioId;
    [SerializeField]
    private idContainer _NormalMusicAudioId;
    [SerializeField]
    private idContainer _FastMusicAudioId;

    private float _difficulty = 1.0f;

    private uint _scoreMultiplier = 1;

    // up
    private Vector3 _movementDir = new Vector3(.0f, 1.0f, .0f);

    private Ramp _hittedRamp = null;
    private List<Snowflake> _snowflakes = new List<Snowflake>();


    [Header("DEBUG Data")]
    [SerializeField]
    private float _snowballCharge = .0f;
    private bool _isSnowball = false;

    private PlaygroundViewController _playgroundController;

    private Coroutine _x2Coroutine = null;

    public float GetDifficulty()
    {
        return _difficulty;
    }

    public Vector3 GetInScopePoint()
    {
        return _InScopePoint.transform.position;
    }

    // get out of scope point
    public Vector3 GetOutOfScopePoint()
    {
        return _OutOfScopePoint.transform.position;
    }

    // return skier's current velocity
    public Vector3 GetVelocity()
    {
        return _movementDir * _Skier.GetSpeed();
    }

    // get slope width
    public float GetSlopeWidth()
    {
        return _SlopeWidth;
    }

    public float GetSlopeHeight()
    {
        return _SlopeHeight;
    }

    public bool IsSnowballActive()
    {
        return _snowballCharge == 1.0f;
    }

    public bool IsGameOver()
    {
        return _lives == 0;
    }

    private void OnEnable()
    {
        //initial music
        AudioSystem.Instance.PlayMusic(_NormalMusicAudioId.Id);
        // init lifes
        _lives = _MaxLifes;

        // subscribing to all the possible events
        _BombHitGameEvent.Subscribe(BombHitEvent);
        _RampHitGameEvent.Subscribe(RampHitEvent);
        _RampExitHitGameEvent.Subscribe(RampExitHitEvent);
        _X2HitGameEvent.Subscribe(X2HitEvent);
        _HeartHitGameEvent.Subscribe(HeartHitEvent);
        _CoinHitGameEvent.Subscribe(CoinHitEvent);
        _TreeHitGameEvent.Subscribe(TreeHitEvent);
        _OutOfScopeEvent.Subscribe(OutOfScopeEvent);
        _InScopeEvent.Subscribe(InScopeEvent);
        _OnPauseEvent.Subscribe(PauseEvent);
        _OnResumeEvent.Subscribe(ResumeEvent);
        _OnGameOverEvent.Subscribe(GameOverEvent);
        _OnRestartEvent.Subscribe(RestartEvent);
        _OffFogGameEvent.Subscribe(OffFogEvent);

        // start progressive difficulty
        StartCoroutine(ProgressiveDifficulty());
        StartCoroutine(ProgressiveSpawnPoints());

        // start charging snowball
        StartCoroutine(ChargeSnowball());

        StartSpawner();

        StartCoroutine(GetPlaygroundViewController());

        StartCoroutine(SpawnFogTimer());
    }

    private void OnDisable()
    {
        // unsubscribe
        // subscribing to all the possible events
        _BombHitGameEvent.Unsubscribe(BombHitEvent);
        _RampHitGameEvent.Unsubscribe(RampHitEvent);
        _RampExitHitGameEvent.Unsubscribe(RampExitHitEvent);
        _X2HitGameEvent.Unsubscribe(X2HitEvent);
        _HeartHitGameEvent.Unsubscribe(HeartHitEvent);
        _CoinHitGameEvent.Unsubscribe(CoinHitEvent);
        _TreeHitGameEvent.Unsubscribe(TreeHitEvent);
        _OutOfScopeEvent.Unsubscribe(OutOfScopeEvent);
        _InScopeEvent.Unsubscribe(InScopeEvent);
        _OnPauseEvent.Unsubscribe(PauseEvent);
        _OnResumeEvent.Unsubscribe(ResumeEvent);
        _OnGameOverEvent.Unsubscribe(GameOverEvent);
        _OnRestartEvent.Unsubscribe(RestartEvent);
        _OffFogGameEvent.Unsubscribe(OffFogEvent);
    }

    public void StartSpawner()
    {
        // first call to spawner
        _Spawner.SetUp();
        _Spawner.Spawn();
        
    }

    public void SpawnSnowflakes()
    {
        _snowflakes = _Spawner.SpawnSnowFlakes(_hittedRamp.GetSnoflakesNumber());
    }

    public void CheckSnowflakesHit(Vector2 touchPosition)
    {
        // touch position already in world coordinates

        // check if the point is inside one of the snowflakes
        foreach (Snowflake sf in _snowflakes)
        {
            if (sf.IsHit())
                continue;

            // check position
            if (sf.gameObject.GetComponent<BoxCollider2D>().OverlapPoint(touchPosition))
            {
                sf.Hit();
                AudioSystem.Instance.PlaySound(_SnowflakeAudioId.Id);
            }
                
        }            
    }

    public void UpdateSnowflakesStatus()
    {
        // check if the snowflakes are all taken or not
        int counter = _hittedRamp.GetSnoflakesNumber();
        foreach (Snowflake sf in _snowflakes)
        {
            if (sf.IsHit())
                --counter;
        }

        // all taken -> nothing to do
        if (counter == 0)
            return;

        // not all taken -> reset status
        foreach (Snowflake sf in _snowflakes)
            sf.Setup();
    }

    public void DespawnSnokflakes()
    {
        // back to pool
        foreach (Snowflake sf in _snowflakes)
        {
            idContainer idManager = sf.GetPoolManagerId();
            PoolManager manager = PoolingSystem.Instance.GetPoolManager(idManager);

            if (manager != null)
                // back to pool
                manager.ReturnPooledObject(sf);
        }

        _snowflakes.Clear();
    }

    public int GetScore()
    {
        return (int)_score;
    }

    #region INTERNAL METHODS

    // Lose a life
    private void Damage()
    {
        if (_snowballCharge == 1.0f)
        {
            // invulnerability
            _snowballCharge = .0f;

            StopSnowballMusicAndAnimations();

            // update UI
            _playgroundController.UpdateSnowMask(_snowballCharge);

            return;
        }

        if (_lives > 0)
        {
            --_lives;
            _playgroundController.HideHeart();
        }
    }

    // Gain a life
    private void Heal()
    {
        if (_lives < _MaxLifes)
        {
            ++_lives;
            _playgroundController.ShowHeart();
            
        }
    }

    // Gain points
    private void AddPoints(uint points)
    {
        _score += points * _scoreMultiplier;

        _playgroundController.SetPoints((int)_score);
    }

    // set the points' multiplier * 2
    private void DoublePoints(float duration)
    {
        if (_x2Coroutine != null)
            StopCoroutine(_x2Coroutine);
        
        _x2Coroutine = StartCoroutine(X2Timer(duration));
    }

    // duration of x2 collectible
    private IEnumerator X2Timer(float duration)
    {
        // double multiplier
        _scoreMultiplier *= 2;

        // show ui
        _playgroundController.ShowX2Timer();

        float passed = .0f;

        // normalized time remaining
        float time = (duration - passed) / duration;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            time = (duration - passed) / duration;

            // update ui
            _playgroundController.UpdateX2Mask(time);

            yield return null;
        }

        // hide ui
        _playgroundController.HideX2Timer();

        // reset multiplier
        _scoreMultiplier = 1;

        _x2Coroutine = null;
    }

    private void BombHitEvent(GameEvent evt)
    {
        // effect
        Damage();
        AudioSystem.Instance.PlaySound(_BombAudioId.Id);
        // disable
        SlopeObject slopeObj = ((SlopeObjectGameEvent)evt).SlopeObject;
        slopeObj.gameObject.SetActive(false);
    }

    private void RampHitEvent(GameEvent evt)
    {
        // trigger jump charge animation
        if (!_isSnowball)
            _Skier.GetComponent<Animator>().SetTrigger("ChargeJump");

        // change flow system state
        BoltFlowSystem.Instance.TriggerFSMevent(_OnRampFlowEvent);

        // take type of ramp
        SlopeObject slopeObj = ((SlopeObjectGameEvent)evt).SlopeObject;
        Ramp ramp = slopeObj as Ramp;

        if (ramp != null)
        {
            _hittedRamp = ramp;
            _Skier.MoveToPoint(ramp.GetPointJump().position);
        }   
    }

    private void RampExitHitEvent(GameEvent evt)
    {
        // jump
        _Skier.Jump();

        if (_Skier.IsAcrobatic())
        {
            // trigger acrobatic jump animation
            if (!_isSnowball)
                _Skier.GetComponent<Animator>().SetTrigger("AcrobaticJump");

            // change flow system state
            BoltFlowSystem.Instance.TriggerFSMevent(_OnAcrobaticStartFlowEvent);
            AudioSystem.Instance.PlaySound(_BigJumpAudioId.Id);
            // start waiting for acrobatic jump to end
            StartCoroutine(WaitAcrobaticJumpEnding());

            // update ui
            _playgroundController.ShowJumpTimer();
            _playgroundController.StartJumpTimerMask(_Skier.GetJumpTime());
        }
        else
        {
            // trigger standard jump animation
            if (!_isSnowball)
                _Skier.GetComponent<Animator>().SetTrigger("StandardJump");

            // change flow system state
            BoltFlowSystem.Instance.TriggerFSMevent(_OffRampFlowEvent);

            // start waiting for jump to end
            StartCoroutine(WaitJumpEnding());
            AudioSystem.Instance.PlaySound(_JumpAudioId.Id);
        }
    }

    private IEnumerator WaitAcrobaticJumpEnding()
    {
        yield return new WaitUntil(() => _Skier.IsGrounded());

        // trigger landing animation
        if (!_isSnowball)
            _Skier.GetComponent<Animator>().SetTrigger("Landing");

        // change flow system state
        BoltFlowSystem.Instance.TriggerFSMevent(_OnAcrobaticEndFlowEvent);

        // update ui
        _playgroundController.HideJumpTimer();
    }

    private IEnumerator WaitJumpEnding()
    {
        yield return new WaitUntil(() => _Skier.IsGrounded());

        // trigger landing animation
        if (!_isSnowball)
            _Skier.GetComponent<Animator>().SetTrigger("Landing");
    }

    public void ApplyAcrobaticJumpResult()
    {
        // check if the snowflakes are all taken or not
        int counter = _hittedRamp.GetSnoflakesNumber();
        float snowflakesGain = counter * _SnowflakeGain;
        foreach (Snowflake sf in _snowflakes)
        {
            if (sf.IsHit())
                --counter;
        }

        // all taken -> points gain and snowball gain
        if (counter == 0)
        {
            AudioSystem.Instance.PlaySound(_JumpSuccessAudioId.Id);
            // points
            AddPoints(_hittedRamp.GetPointsGain());

            // snowball
            _snowballCharge = Mathf.Min(1.0f, _snowballCharge + _SnowflakeGain);
            if (!_isSnowball && _snowballCharge == 1.0f)
                PlaySnowballMusicAndAnimations();

            // update UI
            _playgroundController.UpdateSnowMask(_snowballCharge);
        }
        // not all taken -> stop and damage
        else
        {
            AudioSystem.Instance.PlaySound(_FallAudioId.Id);
            _Skier.ResetSpeed();
            Damage();
        }
    }

    private void X2HitEvent(GameEvent evt)
    {
        X2 slopeObj = (X2)((SlopeObjectGameEvent)evt).SlopeObject;

        AudioSystem.Instance.PlaySound(_X2AudioId.Id);

        // effect
        DoublePoints(slopeObj.GetDuration());

        // disable
        slopeObj.gameObject.SetActive(false);
    }

    private void HeartHitEvent(GameEvent evt)
    {
        AudioSystem.Instance.PlaySound(_HeathAudioId.Id);

        // effect
        Heal();

        // disable
        SlopeObject slopeObj = ((SlopeObjectGameEvent)evt).SlopeObject;
        slopeObj.gameObject.SetActive(false);
    }

    private void CoinHitEvent(GameEvent evt)
    {
        Coin slopeObj = (Coin)((SlopeObjectGameEvent)evt).SlopeObject;

        AudioSystem.Instance.PlaySound(_CoinAudioId.Id);
        // effect
        AddPoints(slopeObj.GetPointsGain());

        // disable
        slopeObj.gameObject.SetActive(false);
    }
    private void OffFogEvent(GameEvent evt)
    {
        Fog slopeObj = (Fog)((SlopeObjectGameEvent)evt).SlopeObject;
        idContainer idManager = slopeObj.GetPoolManagerId();
        PoolManager manager = PoolingSystem.Instance.GetPoolManager(idManager);

        // disable
        slopeObj.gameObject.SetActive(false);

        if (manager != null)
            // back to pool
            manager.ReturnPooledObject(slopeObj);
    }
    private void TreeHitEvent(GameEvent evt)
    {
        if (!_isSnowball)
            _Skier.GetComponent<Animator>().SetTrigger("HitTree");

        // effect
        Damage();

        AudioSystem.Instance.PlaySound(_TreeHitAudioId.Id);

        // disable
        SlopeObject slopeObj = ((SlopeObjectGameEvent)evt).SlopeObject;
        Vector3 pos = slopeObj.transform.position;
        slopeObj.gameObject.SetActive(false);

        // place a broken tree from pool calling the spawn system
        // TODO
    }

    private void OutOfScopeEvent(GameEvent evt)
    {
        SpawnContainer container = ((SpawnContainerGameEvent)evt).Container;

        // clear
        container.Clear();
        // back to pool
        BackToPool(container);
    }

    private void InScopeEvent(GameEvent evt)
    {
        SpawnContainer container = ((SpawnContainerGameEvent)evt).Container;
        int index = -1;

        // check for ramps in spawned objects
        for (int i = 0; i < container.GetSpawnedObjects().Count; ++i)
        {
            if (container.GetSpawnedObjects()[i] as Ramp != null)
            {
                index = i;
                break;
            }
        }

        // call to spawner
        _Spawner.Spawn(index);
    }

    private void PauseEvent(GameEvent evt)
    {
        Time.timeScale = .0f;
    }

    private void ResumeEvent(GameEvent evt)
    {
        BoltFlowSystem.Instance.TriggerFSMevent(_OnResumeFlowEvent);

        Time.timeScale = 1.0f;
    }

    private void GameOverEvent(GameEvent evt)
    {
        Time.timeScale = .0f;
    }

    private void RestartEvent(GameEvent evt)
    {
        UISystem.Instance.HideAllViews();

        // disable all coroutines to avoid exceptions in playground controller
        StopAllCoroutines();

        AudioSystem.Instance.StopMusic();

        Time.timeScale = 1.0f;
        
        BoltFlowSystem.Instance.SetFSMvariable("SCENE_TO_LOAD", _SceneName);
        BoltFlowSystem.Instance.TriggerFSMevent(_OnRestartFlowEvent);
    }

    private void BackToPool(SpawnContainer container)
    {
        idContainer idManager = container.GetPoolManagerId();
        PoolManager manager = PoolingSystem.Instance.GetPoolManager(idManager);

        if (manager != null)
            // back to pool
            manager.ReturnPooledObject(container);
    }

    private IEnumerator ProgressiveDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(_TimeStep);
            
            _Skier.IncreaseMaxSpeed(_SpeedIncrease);
            _Spawner.DecreaseHeightOfWindow(_SpawnDensityIncrease);

            _difficulty += .1f;

        }
    }

    private IEnumerator ProgressiveSpawnPoints()
    {
        float height = _Spawner.GetWindowHeight();
        float minHeight = _Spawner.GetMinWindowHeight();

        int numSpawnPoints = _Spawner.GetNumSpawn();
        int maxNumSpawnPoints = _Spawner.GetMaxNumSpawn();

        float diff = height - minHeight;
        int diffPoints = maxNumSpawnPoints - numSpawnPoints;
        float diffStep = diff / (float)diffPoints;

        float timeStep = (diffStep / _SpawnDensityIncrease) * _TimeStep;

        while (numSpawnPoints < maxNumSpawnPoints)
        {
            yield return new WaitForSeconds(timeStep);

            ++numSpawnPoints;
            _Spawner.SetSpawn(numSpawnPoints);

            _difficulty += 1.0f;
        }
    }

    private IEnumerator ChargeSnowball()
    {
        // value to add to the charge each second
        float valueToAdd = 1.0f / (float)_SnowballChargeTime;

        while (true)
        {
            // wait 1 sec
            yield return new WaitForSeconds(1.0f);

            // add value
            _snowballCharge = Mathf.Min(1.0f, _snowballCharge + valueToAdd);
            if (!_isSnowball && _snowballCharge == 1.0f)
                PlaySnowballMusicAndAnimations();

            // update ui
            _playgroundController.UpdateSnowMask(_snowballCharge);

            // check bound
            yield return new WaitUntil(() => _snowballCharge < 1.0f);
        }
    }

    private IEnumerator GetPlaygroundViewController()
    {
        // wait for playground ui instantiation
        yield return new WaitUntil(() => UISystem.Instance.GetViewController(_PlaygroundViewControllerID) != null);

        // get
        _playgroundController = (PlaygroundViewController)UISystem.Instance.GetViewController(_PlaygroundViewControllerID);
    }

    private void PlaySnowballMusicAndAnimations()
    {
        _isSnowball = true;
        
        AudioSystem.Instance.PlayMusic(_FastMusicAudioId.Id);
        _Skier.GetComponent<Animator>().SetTrigger("SnowballCreate");
    }

    private void StopSnowballMusicAndAnimations()
    {
        _isSnowball = false;
        
        AudioSystem.Instance.PlayMusic(_NormalMusicAudioId.Id);
        _Skier.GetComponent<Animator>().SetTrigger("SnowballBreak");
    }

    private void SpawnFog()
    {
        _Spawner.SpawnFog();
    }
    private IEnumerator SpawnFogTimer()
    {
        yield return new WaitForSeconds(_SecondsFog);
        SpawnFog();
    }
    #endregion INTERNAL METHODS
}
