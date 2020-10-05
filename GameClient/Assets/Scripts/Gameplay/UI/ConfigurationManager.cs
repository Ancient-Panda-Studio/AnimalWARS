using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Player
{
 public class ConfigurationManager : MonoBehaviour
 {


  [Header("AUDIO")] public Slider masterVolumeSlider;
  public Slider sfxVolumeSlider;
  public Slider musicVolumeSlider;
  public Slider ambientVolumeSlider;
  public AudioMixerGroup sfxMixer;
  public AudioMixerGroup ambientMixer;
  public AudioMixerGroup musicMixer;
  public AudioMixer masterMixer;
  [Header("KEY BINDS")] public Bindings currentBind;
  public GameObject setKeyBindUI;
  public Text keyBindShow;
  public Text whatKeyAmISetting;
  public Dropdown resDropdown;

  public List<string> optionsToAdd;

  public List<int> resolutionHeight;
  public List<int> resolutionWidth;
  private Dictionary<string, int> optionsID;
  public void Start()
  {
   PopulateResolutionDropDown();
  }

  private void PopulateResolutionDropDown()
  {
   resDropdown.ClearOptions();
   Resolution[] resolutions = Screen.resolutions;
   for (int i = 0; i < resolutions.Length; i++)
   {
    optionsToAdd.Add( resolutions[i].width + "x" + resolutions[i].height);
    resolutionHeight[i] = resolutions[i].height;
    resolutionWidth[i] = resolutions[i].width;
    optionsID.Add(optionsToAdd[i],i);

   }
   resDropdown.AddOptions(optionsToAdd);
  }
  
   // Print the resolutions
  public void SetResolution(string modifier)
  {
   if (optionsID.Count != 0)
   {
    Application.Quit();
   }
   else
   {
    Screen.SetResolution(resolutionWidth[optionsID[modifier]], resolutionHeight[optionsID[modifier]], Screen.fullScreenMode);
    XMLDataManager.Instance.Entry.UpdateXML(9,0,0,optionsToAdd[optionsID[modifier]]);

    
   }
  }
  public void SetOverall(int preset)
  {
   switch (preset)
   {
    case 0:
     SetTextureDetail(2);
     SetShadowDetail(0);
     SetAntiAliasing(0);

     break;
    case 1:
     SetTextureDetail(1);
     SetShadowDetail(1);
     SetAntiAliasing(1);

     
     break;
    case 2:
     SetTextureDetail(0);
     SetShadowDetail(2);
     SetAntiAliasing(2);
     
     break;
   }
  }

  public void SetTextureDetail(int qualityValue)
  {
   QualitySettings.masterTextureLimit = qualityValue;
   XMLDataManager.Instance.Entry.UpdateXML(5,qualityValue,0,null);
  }

  public void SetShadowDetail(int qualityValue)
  {
   switch (qualityValue)
   { 
    case 0:
     QualitySettings.shadows = ShadowQuality.Disable;
     XMLDataManager.Instance.Entry.UpdateXML(6,qualityValue,0,null);

     break;
    case 1:
     QualitySettings.shadows = ShadowQuality.HardOnly;
     XMLDataManager.Instance.Entry.UpdateXML(6,qualityValue,0,null);

     break;
    case 2:
     QualitySettings.shadows = ShadowQuality.All;
     XMLDataManager.Instance.Entry.UpdateXML(6,qualityValue,0,null);
     break;
   }
  }

  public void SetAntiAliasing(int qualityValue)
  {
   QualitySettings.antiAliasing = qualityValue;
   XMLDataManager.Instance.Entry.UpdateXML(7,qualityValue,0,null);

  }

  public void SetScreenMode(int mode)
  {
   switch (mode)
   {
    case 0:
     Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
     XMLDataManager.Instance.Entry.UpdateXML(5,0,0,null);

     break;
    case 1:
     Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
     XMLDataManager.Instance.Entry.UpdateXML(5,1,0,null);

     break;
    case 2:
     Screen.fullScreenMode = FullScreenMode.Windowed;
     XMLDataManager.Instance.Entry.UpdateXML(5,2,0,null);

     break;
   }
  }

  #region KeyBindRegion

  public void SetKeyBinds(int whatKey, KeyCode keyCode)
  {
   switch (whatKey)
   {
    case 0: //Forward
     currentBind.forward = keyCode;
     XMLDataManager.Instance.Entry.UpdateXML(10,(int)keyCode,0,null);

     break;
    case 1: //Backward
     currentBind.backwards = keyCode;
     XMLDataManager.Instance.Entry.UpdateXML(11,(int)keyCode,0,null);

     break;
    case 2: //Left
     currentBind.left = keyCode;
     XMLDataManager.Instance.Entry.UpdateXML(12,(int)keyCode,0,null);

     break;
    case 3: //Right
     currentBind.right = keyCode;
     XMLDataManager.Instance.Entry.UpdateXML(13,(int)keyCode,0,null);

     break;
    case 4: //Ability
     currentBind.ability = keyCode;
     XMLDataManager.Instance.Entry.UpdateXML(15,(int)keyCode,0,null);

     break;
    case 5: //Interact
     currentBind.interact = keyCode;
     XMLDataManager.Instance.Entry.UpdateXML(14,(int)keyCode,0,null);
     break;
   }
  }

  public void StartKeyBindCollect(int i)
  {
   StartCoroutine(CollectKeyBind(i));
  }

  private IEnumerator CollectKeyBind(int i)
  {

   switch (i)
   {
    case 0: //Forward
     keyBindShow.text = currentBind.forward.ToString();
     whatKeyAmISetting.text = "Move Forward";
     break;
    case 1: //Backward
     keyBindShow.text = currentBind.backwards.ToString();
     whatKeyAmISetting.text = "Move Backward";
     break;
    case 2: //Left
     keyBindShow.text = currentBind.left.ToString();
     whatKeyAmISetting.text = "Move Left";
     break;
    case 3: //Right
     keyBindShow.text = currentBind.right.ToString();
     whatKeyAmISetting.text = "Move Right";
     break;
    case 4: //Ability
     keyBindShow.text = currentBind.ability.ToString();
     whatKeyAmISetting.text = "Use Ability";
     break;
    case 5: //Interact
     keyBindShow.text = currentBind.interact.ToString();
     whatKeyAmISetting.text = "Interact";
     break;
   }

   setKeyBindUI.SetActive(true);
   while (!Input.anyKeyDown) yield return null;
   if (Input.GetKey(KeyCode.Escape))
    setKeyBindUI.SetActive(false);
   //StopCoroutine(WaitForKeyPress(0));
   foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
    if (Input.GetKey(keycode))
    {
     var keyCode = keycode;
     keyBindShow.text = keyCode.ToString();
     SetKeyBinds(i, keyCode);
     yield return new WaitForSecondsRealtime(0.5f);
     setKeyBindUI.SetActive(false);
    }
  }

  #endregion
  
  #region AudioRegion

  public void SetMasterVolumeNoXml()
  {
   masterMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
   XMLDataManager.Instance.Entry.UpdateXML(0,0,masterVolumeSlider.value,null);

  }

  public void SetMusicVolumeNoXml()
  {
   musicMixer.audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
   XMLDataManager.Instance.Entry.UpdateXML(1,0,musicVolumeSlider.value,null);

  }

  public void SetSfxVolumeNoXml()
  {
   musicMixer.audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
   XMLDataManager.Instance.Entry.UpdateXML(2,0,sfxVolumeSlider.value,null);

  }

  public void SetAmbientVolumeNoXml()
  {
   musicMixer.audioMixer.SetFloat("AmbienceVolume", ambientVolumeSlider.value);
   XMLDataManager.Instance.Entry.UpdateXML(2,0,ambientVolumeSlider.value,null);

  }

  public void SetMasterVolumeXml(float value)
  {
   masterMixer.SetFloat("MasterVolume", value);
  }

  public void SetMusicVolumeXml(float value)
  {
   musicMixer.audioMixer.SetFloat("MusicVolume", value);
  }

  public void SetSfxVolumeXml(float value)
  {
   musicMixer.audioMixer.SetFloat("SFXVolume", value);
  }

  public void SetAmbientVolumeXml(float value)
  {
   musicMixer.audioMixer.SetFloat("AmbienceVolume", value);
  }

  #endregion

 }
}

