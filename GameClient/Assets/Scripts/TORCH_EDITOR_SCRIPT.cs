using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InGame;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TORCH_EDITOR_SCRIPT : MonoBehaviour
{
    public GameObject[] torches = null;
    public GameObject  lightning;
    public bool SetIntensity;
    [Range(0, 100f)]
    public float intensityValue;
    [Range(0,100f)]
    public float range;
    public List<GameObject> torchesLight;
    public Color newColor;
    private void Start()
    {
        if(Application.isPlaying) return;
        lightning = GameObject.FindGameObjectWithTag("INSTANTATION");
        torches = GameObject.FindGameObjectsWithTag("TORCH");
        if(torches != null) return;
        foreach (var x in torches){
            GameObject toInstance = Instantiate(lightning);
            toInstance.transform.parent = x.transform;
            toInstance.transform.localPosition = new Vector3(0, 0.015f,0);
            toInstance.tag = "Respawn";
        }
        
    }

    private void Update()
    {
        if(Application.isPlaying) return;
        if (!SetIntensity) return;
        var s = GameObject.FindGameObjectsWithTag("Respawn"); 
        if (torchesLight.Count != s.Length)
        {
            foreach (var t in s)
            {
                if (!torchesLight.Contains(t))
                {
                    torchesLight.Add(t);

                }
            }
        }


        foreach (var lightx in torchesLight)
        {
            var l = lightx.GetComponent<Light>();
            l.intensity = intensityValue;
            l.range = range;
            l.color = newColor;
        }
    }
}
