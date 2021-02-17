using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public AudioSource[] sources;

    public void PlayUISound(int sfx) {
        sources[sfx].Play();
    }
}
