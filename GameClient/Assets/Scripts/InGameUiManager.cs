using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUiManager : MonoBehaviour
{
    public Animator configAnimator;
    public Animator panelAnimator;
    public GameObject configMenu;
    public void ToggleConfig()
    {
        if (!configMenu.activeSelf)
        {
            configMenu.SetActive(true);
            configAnimator.Play("open");
        }
    }
    public void ExitConfig()
    {
        configAnimator.Play("close");
        StartCoroutine(CloseConfig());
    }
    IEnumerator CloseConfig()
    {
        yield return new WaitForSeconds(.5f);
        configMenu.SetActive(false);
    }
    public void LFGame()
    {
        StartCoroutine(FindMatch());
    }
    private IEnumerator FindMatch()
    {
        panelAnimator.Play("In");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(3);
    }
}
