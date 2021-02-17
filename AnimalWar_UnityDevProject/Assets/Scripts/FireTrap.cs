using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public GameObject fireTrapCollider;
    private float _interval = 5f;
    private float _flameLifeTime = 2f;
    private float _baseInterval;
    private bool _fireInProgress;
    
    private void Awake()
    {
        _baseInterval = _interval;
        if (fireTrapCollider.activeSelf)
        {
            fireTrapCollider.SetActive(false);
        }
    }

    private void Update()
    {
        if (_fireInProgress)
        {
            if (_flameLifeTime > 0)
            {
                _flameLifeTime -= Time.deltaTime;
            }
            else
            {
                fireTrapCollider.SetActive(false);
                _fireInProgress = false;
                _flameLifeTime = 2f;
            }
        }
        else
        {
            if (_interval > 0)
            {
                _interval -= Time.deltaTime;
            }
            else
            {
                Fire();
                _interval = _baseInterval;
            }
        }
       
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }

    private void Fire()
    {
        _fireInProgress = true;
        fireTrapCollider.SetActive(true);
    }
}
