using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateLights : MonoBehaviour
{

    public bool GoLights;
    public GameObject pointLight;
    private List<GameObject> antorchas;
    // Start is called before the first frame update
    void Start()
    {
        antorchas = GameObject.FindGameObjectsWithTag("TORCH").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if(GoLights)
            SetLight();
    }

    void SetLight()
    {
        foreach (var antorcha in antorchas)
        {
            var s = UnityEngine.GameObject.Instantiate(pointLight, antorcha.transform);
            //s.transform.localPosition = new Vector3(-1.86207068f, -0.279653251f, 1.96749783f);
        }
        GoLights = false;
    }
}
