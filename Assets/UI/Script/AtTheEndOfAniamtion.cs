using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtTheEndOfAniamtion : MonoBehaviour
{

    [SerializeField]
    private string _OnStartGameFlowEvent;
    public void StartGame(string sceneName)
    {
        BoltFlowSystem.Instance.SetFSMvariable("SCENE_TO_LOAD", sceneName);
        BoltFlowSystem.Instance.TriggerFSMevent(_OnStartGameFlowEvent);
    }

}
