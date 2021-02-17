using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCollider : MonoBehaviour
{
    public PlayerMovement myPlayer;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Colliding with {other.tag}");
        if(other.CompareTag("LPlayer")) return;
        if (myPlayer.isAttacking)
        {
            if (other.gameObject.CompareTag("EPlayer"))
            {
                Debug.Log("Done Damage");
            }
        }
    }
}
