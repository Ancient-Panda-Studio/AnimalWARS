using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public class Settings : MonoBehaviour
    {
        public Bindings currentBinds;

        private void Start()
        {
            ReadData(); //Sets data to the one set by user
        }

        #region currentStates

        private bool isFullscreen = true;
        private int currentRefreshRate;
        private int currentWidth;
        private int currentHeight;
        private float currentSfxVolume;
        private float currentAmbienceVolume;
        private float currentMusicVolume;
        private float currentMainVolume;
        private int currentGraphics;

        #endregion

        #region defaultStates

        private bool defaultScreenMode = true;
        private int DefaultRefreshRate;
        private int DefaultWidth;
        private int DefaultHeight;
        private const float DefaultVolume = 1;
        private const int DefaultGraphics = 1;

        #endregion

        #region ReadWrite

        private void ReadData()
        {
            //Sets Resolution to last set to player and if none was selected to default

            #region ResolutionHandle

            if (PlayerPrefs.HasKey("resolutionWidth"))
            {
                currentWidth = PlayerPrefs.GetInt("resolutionWidth");
                DefaultWidth = PlayerPrefs.GetInt("Default Resolution WIDTH (DO NOT CHANGE THIS)");
            }
            else
            {
                currentWidth = Display.displays[0].systemWidth;
                DefaultWidth = currentWidth;
                WriteData(0, "resolutionWidth", currentWidth, 0f, null);
                WriteData(0, "Default ResolutionWIDTH (DO NOT CHANGE THIS)", currentWidth, 0f, null);
            }

            if (PlayerPrefs.HasKey("resolutionHeight"))
            {
                currentHeight = PlayerPrefs.GetInt("resolutionHeight");
                DefaultWidth = PlayerPrefs.GetInt("Default Resolution Height (DO NOT CHANGE THIS)");
            }
            else
            {
                currentHeight = Display.displays[0].systemHeight;
                DefaultHeight = currentHeight;
                WriteData(0, "resolutionHeight", currentHeight, 0f, null);
                WriteData(0, "Default Resolution Height (DO NOT CHANGE THIS)", currentHeight, 0f, null);
            }

            #endregion

            //Sets KeyBinds to default or the ones set by the player

            #region KeyBounds

            if (PlayerPrefs.HasKey("forwardKeyBind"))
                currentBinds.forward = (KeyCode) PlayerPrefs.GetInt("forwardKeyBind");
            else
                currentBinds.forward = KeyCode.W;
            if (PlayerPrefs.HasKey("jumpKeyBind"))
                currentBinds.jump = (KeyCode) PlayerPrefs.GetInt("jumpKeyBind");
            else
                currentBinds.jump = KeyCode.Space;
            if (PlayerPrefs.HasKey("useAbilityKeyBind"))
                currentBinds.ability = (KeyCode) PlayerPrefs.GetInt("useAbilityKeyBind");
            else
                currentBinds.ability = KeyCode.Q;
            if (PlayerPrefs.HasKey("interactKeyBind"))
                currentBinds.interact = (KeyCode) PlayerPrefs.GetInt("interactKeyBind");
            else
                currentBinds.interact = KeyCode.E;

            #endregion

            //Sets RefreshRate to screenDefault or the one set by player

            #region RefreshRate

            if (PlayerPrefs.HasKey("refreshRate"))
            {
                currentRefreshRate = PlayerPrefs.GetInt("refreshRate");
                DefaultRefreshRate = PlayerPrefs.GetInt("DefaultRefreshRate");
            }
            else
            {
                currentRefreshRate = Screen.currentResolution.refreshRate;
                WriteData(0, "refreshRate", Screen.currentResolution.refreshRate, 0f, null);
                WriteData(0, "DefaultRefreshRate", Screen.currentResolution.refreshRate, 0f, null);
            }

            #endregion

            //Sets ScreenMode to defaultScreenMode or the one set by player

            #region ScreenMode

            if (PlayerPrefs.HasKey("screenMode"))
            {
                isFullscreen = IntToBool(PlayerPrefs.GetInt("screenMode"));
                defaultScreenMode = IntToBool(PlayerPrefs.GetInt("DefaultScreenMode"));
            }
            else
            {
                isFullscreen = true;
                defaultScreenMode = true;
                WriteData(0, "screenMode", BoolToInt(isFullscreen), 0f, null);
                WriteData(0, "DefaultScreenMode", BoolToInt(defaultScreenMode), 0f, null);
            }

            SetFullScreen(BoolToInt(isFullscreen));
            SetScreenData();

            #endregion

            //Sets Audio Values

            #region Audio

            if (PlayerPrefs.HasKey("masterVolume"))
            {
                SetMasterVolume(true);
            }
            else
            {
                mainMixer.SetFloat("MasterVolume", DefaultVolume);
                mainVolumeSlider.value = 1;
                WriteData(1, "masterVolume", 0, 1, null);
            }

            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                SetSfxVolume(true);
            }
            else
            {
                sfxMixer.audioMixer.SetFloat("SFXVolume", DefaultVolume);
                sfxVolumeSlider.value = 1;
                WriteData(1, "sfxVolume", 0, 1, null);
            }

            if (PlayerPrefs.HasKey("ambienceVolume"))
            {
                SetAmbienceVolume(true);
            }
            else
            {
                ambienceMixer.audioMixer.SetFloat("AmbienceVolume", DefaultVolume);
                ambienceVolumeSlider.value = 1;
                WriteData(1, "ambienceVolume", 0, 1, null);
            }

            if (PlayerPrefs.HasKey("musicVolume"))
            {
                SetMusicVolume(true);
                Debug.Log(PlayerPrefs.GetFloat("musicVolume"));
            }
            else
            {
                musicMixer.audioMixer.SetFloat("MusicVolume", DefaultVolume);
                musicVolumeSlider.value = 1;
                WriteData(1, "musicVolume", 0, 1, null);
            }

            #endregion

            SetDropDowns(currentWidth + "x" + currentHeight, currentRefreshRate + "hz",
                isFullscreen ? "FullScreen" : "Windowed");
        }

        private void SetScreenData()
        {
            switch (currentRefreshRate)
            {
                case 48:
                    SetRefreshRate(0, 0);
                    break;
                case 60:
                    SetRefreshRate(1, 0);
                    break;
                case 100:
                    SetRefreshRate(2, 0);
                    break;
                case 144:
                    SetRefreshRate(3, 0);
                    break;
            }

            switch (currentHeight)
            {
                case 480:
                    SetScreenResolution(0, 0, 0);
                    break;
                case 600:
                    SetScreenResolution(1, 0, 0);
                    break;
                case 768:
                    SetScreenResolution(2, 0, 0);
                    break;
                case 720:
                    SetScreenResolution(3, 0, 0);
                    break;
                case 1080:
                    SetScreenResolution(4, 0, 0);
                    break;
                case 1440:
                    SetScreenResolution(5, 0, 0);
                    break;
            }
        }

        private static bool IntToBool(int val)
        {
            return val == 1;
        }

        private static int BoolToInt(bool val)
        {
            return val ? 1 : 0;
        }

        private static void WriteData(int i, string x, int intValue, float floatValue, string stringValue)
        {
            switch (i)
            {
                case 0: //Int
                    PlayerPrefs.SetInt(x, intValue);
                    PlayerPrefs.Save();
                    break;
                case 1: //String
                    PlayerPrefs.SetString(x, stringValue);
                    PlayerPrefs.Save();
                    break;
                case 2: //Float
                    PlayerPrefs.SetFloat(x, floatValue);
                    PlayerPrefs.Save();
                    break;
            }
        }

        #endregion

        #region SetSettings

        private void SetKeyBinds(int i, KeyCode keyCode) //Allows the player to change the currentBindings
        {
            switch (i)
            {
                case 0:
                    currentBinds.forward = keyCode;
                    WriteData(0, "forwardKeyBind", (int) keyCode, 0f, null);
                    break;
                case 1:
                    currentBinds.jump = keyCode;
                    WriteData(0, "jumpKeyBind", (int) keyCode, 0f, null);
                    break;
                case 2:
                    currentBinds.ability = keyCode;
                    WriteData(0, "useAbilityKeyBind", (int) keyCode, 0f, null);

                    break;
                case 3:
                    currentBinds.interact = keyCode;
                    WriteData(0, "interactKeyBind", (int) keyCode, 0f, null);
                    break;
            }
        }

        private void SetScreenResolution(int i, int width, int height)
        {
            switch (i)
            {
                case 0:
                    Screen.SetResolution(640, 480, isFullscreen, currentRefreshRate);
                    currentWidth = 640;
                    currentHeight = 480;
                    WriteData(0, "resolutionWidth", currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", currentHeight, 0f, null);
                    break;
                case 1:
                    Screen.SetResolution(800, 600, isFullscreen, currentRefreshRate);
                    currentWidth = 800;
                    currentHeight = 600;
                    WriteData(0, "resolutionWidth", currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", currentHeight, 0f, null);
                    break;
                case 2:
                    Screen.SetResolution(1024, 768, isFullscreen, currentRefreshRate);
                    currentWidth = 1024;
                    currentHeight = 768;
                    WriteData(0, "resolutionWidth", currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", currentHeight, 0f, null);
                    break;
                case 3:
                    Screen.SetResolution(1280, 720, isFullscreen, currentRefreshRate);
                    currentWidth = 1280;
                    currentHeight = 720;
                    WriteData(0, "resolutionWidth", currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", currentHeight, 0f, null);
                    break;
                case 4:
                    Screen.SetResolution(1920, 1080, isFullscreen, currentRefreshRate);
                    currentWidth = 1920;
                    currentHeight = 1080;
                    WriteData(0, "resolutionWidth", currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", currentHeight, 0f, null);
                    break;
                case 5:
                    Screen.SetResolution(2560, 1440, true, currentRefreshRate);
                    currentWidth = 2560;
                    currentHeight = 1440;
                    WriteData(0, "resolutionWidth", currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", currentHeight, 0f, null);
                    break;
                case 6: //Custom
                    Screen.SetResolution(width, height, isFullscreen, currentRefreshRate);
                    break;
            }
        } //Allows the player to change resolution

        private void SetRefreshRate(int i, int custom)
        {
            switch (i)
            {
                case 0:
                    Screen.SetResolution(currentWidth, currentHeight, isFullscreen, 48);
                    currentRefreshRate = 48;
                    break;
                case 1:
                    Screen.SetResolution(currentWidth, currentHeight, isFullscreen, 60);
                    currentRefreshRate = 60;
                    break;
                case 2:
                    Screen.SetResolution(currentWidth, currentHeight, isFullscreen, 100);
                    currentRefreshRate = 100;
                    break;
                case 3: //Custom
                    Screen.SetResolution(currentWidth, currentHeight, isFullscreen, 144);
                    currentRefreshRate = 144;
                    break;
            }
        } //Allows the player to set a screen refresh rate

        private void SetFullScreen(int i)
        {
            switch (i)
            {
                case 0:
                    Screen.fullScreen = true;
                    isFullscreen = true;
                    break;
                case 1:
                    Screen.fullScreen = false;
                    isFullscreen = false;
                    break;
            }
        } //Switches between FullScreen and Windowed mode

        private void SetSoundMixersVolumes(int i, int desiredVolume)
        {
            switch (i)
            {
                case 0: //Music
                    break;
                case 1: //SFX
                    break;
                case 2: //Ambient
                    break;
                case 3: //General
                    break;
            }
        } //Allows the player to change volumes

        private void SetGraphicSettings(int i) //Allows the player to switch between pre established graphic modes
        {
            switch (i)
            {
                case 0: //Super-Low Used for machines below minimum requirements for the game
                    break;
                case 1: //Low used for maximum FPS or for machines right on the minimum requirements
                    break;
                case 2: //Medium used for good balance between FPS and Visuals
                    break;
                case 3: //High used for very good graphics 
                    break;
                case 4: //Used to boost ego **Look I run ultra I'm a big boy**
                    break;
            }
        }

        #endregion

        #region HandleUI

        #region SoundOptions

        public Slider sfxVolumeSlider;
        public Slider ambienceVolumeSlider;
        public Slider mainVolumeSlider;
        public Slider musicVolumeSlider;

        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup ambienceMixer;
        public AudioMixerGroup musicMixer;
        public AudioMixer mainMixer;

        #endregion

        #region ScreenOptions

        public Dropdown resolutionSelector;
        public Dropdown refreshRateSelector;
        public Dropdown screenModeSelector;
        public Text resolutionTxt;
        public Text refreshTxt;
        public Text fullscreenTxt;

        #endregion

        #region KeyBindsOptions

        public GameObject keyBindUI;
        public Text keyPress;
        public Button forwardBoundButton;
        public Button jumpBoundButton;
        public Button abilityBoundButton;

        public Button interactBoundButton;
        //public Button menuBoundButton;

        #endregion

        #endregion

        #region UISetter

        #region Sound

        public void SetMasterVolume(bool x)
        {
            if (x)
            {
                var value = PlayerPrefs.GetFloat("masterVolume");
                mainMixer.SetFloat("MasterVolume", value);
                mainVolumeSlider.value = value;
            }
            else
            {
                mainMixer.SetFloat("MasterVolume", mainVolumeSlider.value);
                WriteData(2, "masterVolume", 0, mainVolumeSlider.value, null);
            }
        }

        private void SetDropDowns(string res, string refresh, string screen)
        {
            resolutionTxt.text = res;
            refreshTxt.text = refresh;
            fullscreenTxt.text = screen;
        }

        public void SetSfxVolume(bool x)
        {
            if (x)
            {
                sfxMixer.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("sfxVolume"));
                sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            }
            else
            {
                sfxMixer.audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
                WriteData(2, "sfxVolume", 0, sfxVolumeSlider.value, null);
            }
        }

        public void SetAmbienceVolume(bool x)
        {
            if (x)
            {
                ambienceMixer.audioMixer.SetFloat("AmbienceVolume", PlayerPrefs.GetFloat("ambienceVolume"));
                ambienceVolumeSlider.value = PlayerPrefs.GetFloat("ambienceVolume");
            }
            else
            {
                ambienceMixer.audioMixer.SetFloat("AmbienceVolume", ambienceVolumeSlider.value);
                WriteData(2, "ambienceVolume", 0, ambienceVolumeSlider.value, null);
            }
        }

        public void SetMusicVolume(bool x)
        {
            if (x)
            {
                musicMixer.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("musicVolume"));
                musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
            }
            else
            {
                musicMixer.audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
                WriteData(2, "musicVolume", 0, musicVolumeSlider.value, null);
            }
        }

        #endregion

        #region ScreenOptions

        public void SetScreenRes()
        {
            SetScreenResolution(resolutionSelector.value, 0, 0);
        }

        public void SetHz()
        {
            SetRefreshRate(refreshRateSelector.value, 0);
        }

        public void SetScreenMode()
        {
            SetFullScreen(screenModeSelector.value);
        }

        #endregion

        #region SetKeyBinds

        private IEnumerator WaitForKeyPress(int i)
        {
            switch (i)
            {
                case 0: //Move
                    keyPress.text = currentBinds.forward.ToString();
                    break;
                case 1: //Jump
                    keyPress.text = currentBinds.jump.ToString();

                    break;
                case 2: //Ability
                    keyPress.text = currentBinds.ability.ToString();

                    break;
                case 3: //Interact
                    keyPress.text = currentBinds.interact.ToString();

                    break;
            }

            keyBindUI.SetActive(true);
            while (!Input.anyKeyDown) yield return null;
            if (Input.GetKey(KeyCode.Escape))
                keyBindUI.SetActive(false);
            //StopCoroutine(WaitForKeyPress(0));
            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                if (Input.GetKey(keycode))
                {
                    var x = keycode;
                    keyPress.text = x.ToString();
                    SetKeyBinds(i, x);

                    yield return new WaitForSecondsRealtime(0.5f);
                    keyBindUI.SetActive(false);
                }
        }

        public void SetKeyBound(int i)
        {
            StartCoroutine(WaitForKeyPress(i));
        }

        #endregion

        #endregion
    }
}