using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class LogoManager : MonoBehaviour
{
    public VideoPlayer player;
    private int CurrentFrame = 0;

    private void Update() {
        CurrentFrame++;
        if(CurrentFrame >= 10){
        if(!player.isPlaying){
            SceneManager.LoadScene(1);
        }
        }
    }
}
