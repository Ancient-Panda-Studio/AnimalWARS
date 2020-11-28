using System;
using System.Collections;
using System.Collections.Generic;
using InGame;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{

    public GameObject gameMenu;
    public Animator gameMenuAnimations;
    public Animator mainAnimator;
    public GameObject configMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (gameMenu.activeSelf)
        {
            Movement.Instance.DisableCamaraMovement(true);
            StartCoroutine(CloseMenu());
        } else
        {
            Movement.Instance.DisableCamaraMovement(false);
            gameMenu.SetActive(true);
        }
    }
    
    public void ExitGame()
    {
        StartCoroutine(ExitApp(1));
    }
    public void ReturnToMenu(){
                StartCoroutine(ExitApp(2));
    }
    public void ToggleConfig()
    {
        if (!configMenu.activeSelf)
        {
            configMenu.SetActive(true);
        }
        else
        {
            ExitConfig();
        }
    }

    public void ExitConfig()
    {
        StartCoroutine(CloseConfig());
    }

    IEnumerator CloseConfig()
    {
        yield return new WaitForSeconds(.5f);
        configMenu.SetActive(false);
    }

    private IEnumerator ExitApp(int x)
    {
        mainAnimator.Play("Exit APP");
        yield return new WaitForSeconds(.5f);
        if(x == 1){
        Application.Quit();
        } else {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        
    }

    private IEnumerator CloseMenu()
    {
        gameMenuAnimations.Play("CloseMenu");
        yield return new WaitForSeconds(.3f);
        gameMenu.SetActive(false);

    }
    
}
