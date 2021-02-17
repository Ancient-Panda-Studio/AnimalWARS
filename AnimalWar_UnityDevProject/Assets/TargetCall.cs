using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LPlayer")
        {
            TutorialManager.Instance.LastStage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
