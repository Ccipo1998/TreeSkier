using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSlope : MonoBehaviour
{
    [SerializeField]
    private GameObject _Background1;
    [SerializeField]
    private GameObject _Background2;

    // Update is called once per frame
    void Update()
    {
        // move the slope at skier' speed
        _Background1.transform.position += LevelSystem.Instance.GetVelocity() * Time.deltaTime;
        _Background2.transform.position += LevelSystem.Instance.GetVelocity() * Time.deltaTime;

        // correct slope's position to loop it in the background
        float slopeHeight = LevelSystem.Instance.GetSlopeHeight();
        _Background1.transform.position = new Vector3(.0f, ((_Background1.transform.position.y + slopeHeight) % (slopeHeight * 2.0f)) - slopeHeight, .0f);
        _Background2.transform.position = new Vector3(.0f, ((_Background2.transform.position.y + slopeHeight) % (slopeHeight * 2.0f)) - slopeHeight, .0f);
    }
}
