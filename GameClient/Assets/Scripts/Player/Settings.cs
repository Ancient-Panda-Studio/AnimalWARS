using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//TODO  REMAKE WHOLE SCRIPT...
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

        private bool _isFullscreen = true;
        private int _currentRefreshRate;
        private int _currentWidth;
        private int _currentHeight;
        private float _currentSfxVolume;
        private float _currentAmbienceVolume;
        private float _currentMusicVolume;
        private float _currentMainVolume;
        private int _currentGraphics;

        #endregion

        #region defaultStates

        private bool _defaultScreenMode = true;
        private int _defaultRefreshRate;
        private int _defaultWidth;
        private int _defaultHeight;
        private const float DefaultVolume = 1;

        #endregion

        #region ReadWrite

        private void ReadData()
        {
            //Sets Resolution to last set to player and if none was selected to default

            #region ResolutionHandle

            if (PlayerPrefs.HasKey("resolutionWidth"))
            {
                _currentWidth = PlayerPrefs.GetInt("resolutionWidth");
                _defaultWidth = PlayerPrefs.GetInt("Default Resolution WIDTH (DO NOT CHANGE THIS)");
            }
            else
            {
                _currentWidth = Display.displays[0].systemWidth;
                _defaultWidth = _currentWidth;
                WriteData(0, "resolutionWidth", _currentWidth, 0f, null);
                WriteData(0, "Default ResolutionWIDTH (DO NOT CHANGE THIS)", _currentWidth, 0f, null);
            }

            if (PlayerPrefs.HasKey("resolutionHeight"))
            {
                _currentHeight = PlayerPrefs.GetInt("resolutionHeight");
                _defaultWidth = PlayerPrefs.GetInt("Default Resolution Height (DO NOT CHANGE THIS)");
            }
            else
            {
                _currentHeight = Display.displays[0].systemHeight;
                _defaultHeight = _currentHeight;
                WriteData(0, "resolutionHeight", _currentHeight, 0f, null);
                WriteData(0, "Default Resolution Height (DO NOT CHANGE THIS)", _currentHeight, 0f, null);
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
                _currentRefreshRate = PlayerPrefs.GetInt("refreshRate");
                _defaultRefreshRate = PlayerPrefs.GetInt("DefaultRefreshRate");
            }
            else
            {
                _currentRefreshRate = Screen.currentResolution.refreshRate;
                WriteData(0, "refreshRate", Screen.currentResolution.refreshRate, 0f, null);
                WriteData(0, "DefaultRefreshRate", Screen.currentResolution.refreshRate, 0f, null);
            }

            #endregion

            //Sets ScreenMode to defaultScreenMode or the one set by player

            #region ScreenMode

            if (PlayerPrefs.HasKey("screenMode"))
            {
                _isFullscreen = IntToBool(PlayerPrefs.GetInt("screenMode"));
                _defaultScreenMode = IntToBool(PlayerPrefs.GetInt("DefaultScreenMode"));
            }
            else
            {
                _isFullscreen = true;
                _defaultScreenMode = true;
                WriteData(0, "screenMode", BoolToInt(_isFullscreen), 0f, null);
                WriteData(0, "DefaultScreenMode", BoolToInt(_defaultScreenMode), 0f, null);
            }

            SetFullScreen(BoolToInt(_isFullscreen));
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

            SetDropDowns(_currentWidth + "x" + _currentHeight, _currentRefreshRate + "hz",
                _isFullscreen ? "FullScreen" : "Windowed");
        }

        private void SetScreenData()
        {
            switch (_currentRefreshRate)
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

            switch (_currentHeight)
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

        public void FallBackToDefault()
        {
            currentBinds.forward = KeyCode.W;
            currentBinds.ability = KeyCode.Q;
            currentBinds.interact = KeyCode.E;
            currentBinds.jump = KeyCode.Space;
            
            SetRefreshRate(4,_defaultRefreshRate);
            mainMixer.SetFloat("MasterVolume", 1);
            mainVolumeSlider.value = 1;
            ambienceMixer.audioMixer.SetFloat("AmbienceVolume", 1);
            ambienceVolumeSlider.value = 1;
            sfxMixer.audioMixer.SetFloat("SFXVolume", 1);
            sfxVolumeSlider.value = 1;
            musicMixer.audioMixer.SetFloat("MusicVolume", 1);
            musicVolumeSlider.value = 1;
            WriteData(2, "masterVolume", 0, 1, null);
            WriteData(2, "sfxVolume", 0, 1, null);
            WriteData(2, "ambienceVolume", 0, 1, null);
            WriteData(2, "musicVolume", 0, 1, null);
        }
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
                    Screen.SetResolution(640, 480, _isFullscreen, _currentRefreshRate);
                    _currentWidth = 640;
                    _currentHeight = 480;
                    WriteData(0, "resolutionWidth", _currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", _currentHeight, 0f, null);
                    break;
                case 1:
                    Screen.SetResolution(800, 600, _isFullscreen, _currentRefreshRate);
                    _currentWidth = 800;
                    _currentHeight = 600;
                    WriteData(0, "resolutionWidth", _currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", _currentHeight, 0f, null);
                    break;
                case 2:
                    Screen.SetResolution(1024, 768, _isFullscreen, _currentRefreshRate);
                    _currentWidth = 1024;
                    _currentHeight = 768;
                    WriteData(0, "resolutionWidth", _currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", _currentHeight, 0f, null);
                    break;
                case 3:
                    Screen.SetResolution(1280, 720, _isFullscreen, _currentRefreshRate);
                    _currentWidth = 1280;
                    _currentHeight = 720;
                    WriteData(0, "resolutionWidth", _currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", _currentHeight, 0f, null);
                    break;
                case 4:
                    Screen.SetResolution(1920, 1080, _isFullscreen, _currentRefreshRate);
                    _currentWidth = 1920;
                    _currentHeight = 1080;
                    WriteData(0, "resolutionWidth", _currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", _currentHeight, 0f, null);
                    break;
                case 5:
                    Screen.SetResolution(2560, 1440, true, _currentRefreshRate);
                    _currentWidth = 2560;
                    _currentHeight = 1440;
                    WriteData(0, "resolutionWidth", _currentWidth, 0f, null);
                    WriteData(0, "resolutionHeight", _currentHeight, 0f, null);
                    break;
                case 6: //Custom
                    Screen.SetResolution(width, height, _isFullscreen, _currentRefreshRate);
                    break;
            }
        } //Allows the player to change resolution

        private void SetRefreshRate(int i, int custom)
        {
            switch (i)
            {
                case 0:
                    Screen.SetResolution(_currentWidth, _currentHeight, _isFullscreen, 48);
                    _currentRefreshRate = 48;
                    break;
                case 1:
                    Screen.SetResolution(_currentWidth, _currentHeight, _isFullscreen, 60);
                    _currentRefreshRate = 60;
                    break;
                case 2:
                    Screen.SetResolution(_currentWidth, _currentHeight, _isFullscreen, 100);
                    _currentRefreshRate = 100;
                    break;
                case 3: 
                    Screen.SetResolution(_currentWidth, _currentHeight, _isFullscreen, 144);
                    _currentRefreshRate = 144;
                    break;
                case 4: //Custom
                    Screen.SetResolution(_currentWidth, _currentHeight, _isFullscreen, custom);
                    _currentRefreshRate = custom;
                    break;
            }
        } //Allows the player to set a screen refresh rate

        private void SetFullScreen(int i)
        {
            switch (i)
            {
                case 0:
                    Screen.fullScreen = true;
                    _isFullscreen = true;
                    break;
                case 1:
                    Screen.fullScreen = false;
                    _isFullscreen = false;
                    break;
            }
        } //Switches between FullScreen and Windowed mode

        private void FallBackSounds(int i, int desiredVolume)
        {
            
        } 

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

            switch (x)
            {
                case true:
                    sfxMixer.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("sfxVolume"));
                    sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
                    break;
                case false:
                    sfxMixer.audioMixer.SetFloat("SFXVolume", sfxVolumeSlider.value);
                    WriteData(2, "sfxVolume", 0, sfxVolumeSlider.value, null);
                    break;
            }
        }

        public void SetAmbienceVolume(bool x)
        {
            switch (x)
            {
                case true:
                    ambienceMixer.audioMixer.SetFloat("AmbienceVolume", PlayerPrefs.GetFloat("ambienceVolume"));
                    ambienceVolumeSlider.value = PlayerPrefs.GetFloat("ambienceVolume");
                    break;
                case  false:
                    ambienceMixer.audioMixer.SetFloat("AmbienceVolume", ambienceVolumeSlider.value);
                    WriteData(2, "ambienceVolume", 0, ambienceVolumeSlider.value, null);
                    break;
                    ;
            }
        }

        public void SetMusicVolume(bool x)
        {
            switch (x)
            {
                    case true:
                        musicMixer.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("musicVolume"));
                        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
                        break;
                    case false:
                        musicMixer.audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
                        WriteData(2, "musicVolume", 0, musicVolumeSlider.value, null);
                        break;
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