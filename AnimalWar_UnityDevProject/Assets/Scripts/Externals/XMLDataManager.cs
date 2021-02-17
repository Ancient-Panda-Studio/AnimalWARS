using UnityEngine;
using  System.Collections;
using  System.Collections.Generic;
using  System.Xml;
using  System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Player;
using UnityEngine.UI;

public class XMLDataManager : MonoBehaviour
{
    public static XMLDataManager Instance;
    public GameObject x;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        if (!Directory.Exists(Constants.DirectoryPath))
        {
            Entry.GenerateNewDirectory();
        }
        else
        {
            if (!File.Exists(Constants.FilePath))
            { 
                Entry.GenerateNewFile(); 
                return;
            }
            else
            {
                Entry._foundDocument = XDocument.Load(Constants.FilePath);
                Entry.DocumentHandler();
            }

        }
        Entry.x = x;

    }

    public DataEntry Entry;
}

[System.Serializable]
    public class DataEntry
    {
        public ConfigurationManager manager;
        public XDocument _foundDocument;
        public int DropDownValue;
        public string DesiredResolution;
        public GameObject x;
        public void ResetDropDown()
        {
            DropDownValue = 0;
            manager.resDropdown.value = 0;
        }
        public void UpdateXML(int modified,int intValue, float floatValue, string stringValue)
        {
            switch (modified)
            {
                case 0:

                    var masterElement = _foundDocument.Descendants("Settings").Elements("MasterVolume").FirstOrDefault();
                    masterElement?.SetValue(floatValue);
                    break;
                case 1:
                    var musicElement = _foundDocument.Descendants("Settings").Elements("MusicVolume").FirstOrDefault();
                    musicElement?.SetValue(floatValue);
                    break;
                case 2:
                    var sfxElement = _foundDocument.Descendants("Settings").Elements("SfxVolume").FirstOrDefault();
                    sfxElement?.SetValue(floatValue);
                    break;
                case 3:
                    var ambientElement = _foundDocument.Descendants("Settings").Elements("AmbientVolume").FirstOrDefault();
                    ambientElement?.SetValue(floatValue);
                    break;
                case 5:
                    var textureElement = _foundDocument.Descendants("Settings").Elements("TextureDetail").FirstOrDefault();
                    textureElement?.SetValue(intValue);
                    switch (intValue)
                    {
                        case 0:
                            manager.textureHigh.isDefault = true;
                            manager.textureMid.isDefault = false;
                            manager.textureLow.isDefault = false;
                            break;
                        case 1:
                            manager.textureMid.isDefault = true;
                            manager.textureHigh.isDefault = false;
                            manager.textureLow.isDefault = false;
                            break;
                        case 2:
                            manager.textureLow.isDefault = true;
                            manager.textureHigh.isDefault = false;
                            manager.textureMid.isDefault = false;
                            break;
                    }
                    break;
                case 6:
                    var shadowElement = _foundDocument.Descendants("Settings").Elements("ShadowDetail").FirstOrDefault();
                    shadowElement?.SetValue(intValue);
                    switch (intValue)
                    {
                        case 0:
                            manager.shadowHigh.isDefault = true;
                            manager.shadowLow.isDefault = false;
                            manager.shadowMid.isDefault = false;
                            break;
                        case 1:
                            manager.shadowMid.isDefault = true;
                            manager.shadowHigh.isDefault = false;
                            manager.shadowLow.isDefault = false;
                            break;
                        case 2:
                            manager.shadowLow.isDefault = true;
                            manager.shadowHigh.isDefault = false;
                            manager.shadowMid.isDefault = false;
                            break;
                    }
                    break;
                case 7:
                    var aaElement = _foundDocument.Descendants("Settings").Elements("AntiAliasing").FirstOrDefault();
                    aaElement?.SetValue(intValue);
                    switch (intValue)
                    {
                        case 0:
                            manager.aaHigh.isDefault = true;
                            manager.aaMid.isDefault = false;
                            manager.aaLow.isDefault = false;
                            break;
                        case 1:
                            manager.aaMid.isDefault = true;
                            manager.aaLow.isDefault = false;
                            manager.aaHigh.isDefault = false;
                            break;
                        case 2:
                            manager.aaLow.isDefault = true;
                            manager.aaHigh.isDefault = false;
                            manager.aaMid.isDefault = false;
                            break;
                    }
                    break;
                case 8:
                    var screenModeElement = _foundDocument.Descendants("Settings").Elements("ScreenMode").FirstOrDefault();
                    screenModeElement?.SetValue(intValue);
                    switch (intValue)
                    {
                        case 0: //FullScreen
                            manager.scModeHigh.isDefault = true;
                            manager.scModeLow.isDefault = false;
                            manager.scModeMid.isDefault = false;
                            break;
                        case 1: //Borderless Window
                            manager.scModeMid.isDefault = true;
                            manager.scModeHigh.isDefault = false;
                            manager.scModeLow.isDefault = false;
                            break;
                        case 2: //FullScreen
                            manager.scModeLow.isDefault = true;
                            manager.scModeMid.isDefault = false;
                            manager.scModeHigh.isDefault = false;
                            break;
                    }
                    break;
                case 9:
                    var resolutionElement = _foundDocument.Descendants("Settings").Elements("ScreenResolution").FirstOrDefault();
                    resolutionElement?.SetValue(stringValue);
                    break;
                case 10:
                    var forwardElement = _foundDocument.Descendants("Settings").Elements("Forwards").FirstOrDefault();
                    forwardElement?.SetValue(intValue);
                    break;
                case 11:
                    var backElement = _foundDocument.Descendants("Settings").Elements("Backwards").FirstOrDefault();
                    backElement?.SetValue(intValue);
                    break;
                case 12:
                    var leftElement = _foundDocument.Descendants("Settings").Elements("Left").FirstOrDefault();
                    leftElement?.SetValue(intValue);
                    break;
                case 13:
                    var rightElement = _foundDocument.Descendants("Settings").Elements("Right").FirstOrDefault();
                    rightElement?.SetValue(intValue);
                    break;
                case 14:
                    var interactElement = _foundDocument.Descendants("Settings").Elements("Interact").FirstOrDefault();
                    interactElement?.SetValue(intValue);
                    break;
                case 15:
                    var abilityElement = _foundDocument.Descendants("Settings").Elements("Ability").FirstOrDefault();
                    abilityElement?.SetValue(intValue);
                    break;
                case 16:
                    var jumpElement = _foundDocument.Descendants("Settings").Elements("Jump").FirstOrDefault();
                    jumpElement?.SetValue(intValue);
                    break;
                case 17:
                    var dropElement = _foundDocument.Descendants("Settings").Elements("ResDropDownValue").FirstOrDefault();
                    dropElement?.SetValue(intValue);
                    break;
            }
            _foundDocument.Save(Constants.FilePath);
        }
        public void DocumentHandler()
        {
                if (!File.Exists(Constants.FilePath))
                {
                    GenerateNewFile();
                    return;
                }

                //ReadsXML And Applies 
                var readMasterVolume = (float) _foundDocument.Descendants("MasterVolume").FirstOrDefault();
                var readMusicVolume = (float) _foundDocument.Descendants("MusicVolume").FirstOrDefault();
                var readSfxVolume = (float) _foundDocument.Descendants("SfxVolume").FirstOrDefault();
                var readAmbientVolume = (float) _foundDocument.Descendants("AmbientVolume").FirstOrDefault();
                var readTextureDetail = (int) _foundDocument.Descendants("TextureDetail").FirstOrDefault();
                var readShadowDetail = (int) _foundDocument.Descendants("ShadowDetail").FirstOrDefault();
                var readAntiAliasing = (int) _foundDocument.Descendants("AntiAliasing").FirstOrDefault();
                var readScreenMode = (int) _foundDocument.Descendants("ScreenMode").FirstOrDefault();
                var readForwardKey = (int) _foundDocument.Descendants("Forwards").FirstOrDefault();
                var readBackwardKey = (int) _foundDocument.Descendants("Backwards").FirstOrDefault();
                var readScreenResolution = (string) _foundDocument.Descendants("ScreenResolution").FirstOrDefault();
                var readLeftKey = (int) _foundDocument.Descendants("Left").FirstOrDefault();
                var readRightKey = (int) _foundDocument.Descendants("Right").FirstOrDefault();
                var readInteractKey = (int) _foundDocument.Descendants("Interact").FirstOrDefault();
                var readAbilityKey = (int) _foundDocument.Descendants("Ability").FirstOrDefault();
                var readJumpKey = (int) _foundDocument.Descendants("Jump").FirstOrDefault();
                var readDropValue = (int) _foundDocument.Descendants("ResDropDownValue").FirstOrDefault();
                DropDownValue = readDropValue;
                //Apply Data
                manager.SetMasterVolumeXml(readMasterVolume);
                manager.SetAmbientVolumeXml(readAmbientVolume);
                manager.SetMusicVolumeXml(readMusicVolume);
                manager.SetSfxVolumeXml(readSfxVolume);
                manager.SetKeyBinds(0,(KeyCode)readForwardKey);
                manager.SetKeyBinds(1,(KeyCode)readBackwardKey);
                manager.SetKeyBinds(2,(KeyCode)readLeftKey);
                manager.SetKeyBinds(3,(KeyCode)readRightKey);
                manager.SetKeyBinds(5,(KeyCode)readInteractKey);
                manager.SetKeyBinds(4,(KeyCode)readAbilityKey);
                manager.SetKeyBinds(6,(KeyCode)readJumpKey);
                //manager.masterMixer.SetFloat("MasterVolume", -60f);
                manager.forwardKeyBindText.text = ((KeyCode) readForwardKey).ToString();
                manager.backwardsKeyBindText.text = ((KeyCode) readBackwardKey).ToString();
                manager.leftKeyBindText.text = ((KeyCode) readLeftKey).ToString();
                manager.rightKeyBindText.text = ((KeyCode) readRightKey).ToString();
                manager.abilityKeyBindText.text = ((KeyCode) readAbilityKey).ToString();
                manager.interactKeyBindText.text = ((KeyCode) readInteractKey).ToString();
                manager.jumpKeyBindText.text = ((KeyCode) readJumpKey).ToString();
                /*manager.masterVolumeSlider.value = readMasterVolume;
                manager.ambientVolumeSlider.value = readAmbientVolume;
                manager.musicVolumeSlider.value = readMusicVolume;
                manager.sfxVolumeSlider.value = readSfxVolume;*/
                switch (readScreenMode)
                    {
                        case 0: //FullScreen
                            manager.scModeHigh.isDefault = true;
                            manager.scModeLow.isDefault = false;
                            manager.scModeMid.isDefault = false;
                            break;
                        case 1: //Borderless Window
                            manager.scModeMid.isDefault = true;
                            manager.scModeHigh.isDefault = false;
                            manager.scModeLow.isDefault = false;
                            break;
                        case 2: //FullScreen
                            manager.scModeLow.isDefault = true;
                            manager.scModeMid.isDefault = false;
                            manager.scModeHigh.isDefault = false;
                            break;
                    }
                switch (readShadowDetail)
                    {
                        case 0:
                            manager.shadowHigh.isDefault = true;
                            manager.shadowLow.isDefault = false;
                            manager.shadowMid.isDefault = false;
                            break;
                        case 1:
                            manager.shadowMid.isDefault = true;
                            manager.shadowHigh.isDefault = false;
                            manager.shadowLow.isDefault = false;
                            break;
                        case 2:
                            manager.shadowLow.isDefault = true;
                            manager.shadowHigh.isDefault = false;
                            manager.shadowMid.isDefault = false;
                            break;
                    }
                switch (readAntiAliasing)
                    {
                        case 0:
                            manager.aaHigh.isDefault = true;
                            manager.aaMid.isDefault = false;
                            manager.aaLow.isDefault = false;
                            break;
                        case 1:
                            manager.aaMid.isDefault = true;
                            manager.aaLow.isDefault = false;
                            manager.aaHigh.isDefault = false;
                            break;
                        case 2:
                            manager.aaLow.isDefault = true;
                            manager.aaHigh.isDefault = false;
                            manager.aaMid.isDefault = false;
                            break;
                    }
                switch (readTextureDetail)
                    {
                        case 0:
                            manager.textureHigh.isDefault = true;
                            manager.textureMid.isDefault = false;
                            manager.textureLow.isDefault = false;
                            break;
                        case 1:
                            manager.textureMid.isDefault = true;
                            manager.textureHigh.isDefault = false;
                            manager.textureLow.isDefault = false;
                            break;
                        case 2:
                            manager.textureLow.isDefault = true;
                            manager.textureHigh.isDefault = false;
                            manager.textureMid.isDefault = false;
                            break;
                    }
                manager.SetScreenMode(readScreenMode);
                manager.SetShadowDetail(readShadowDetail);
                manager.SetAntiAliasing(readAntiAliasing);
                manager.SetTextureDetail(readTextureDetail);
                DesiredResolution = readScreenResolution;
                manager.SetScreenMode(readScreenMode);
        }

        public void GenerateNewFile()
        {
            //First start or document deleted create new document with default values
            if (File.Exists(Constants.FilePath))
            {
                File.Delete(Constants.FilePath);
            }
            var document = new XDocument(
                new XElement("Settings",
                    new XComment("Volumes MAX = 1 MIN = -80"),
                    new XElement("MasterVolume", 0),
                    new XElement("MusicVolume", 0),
                    new XElement("SfxVolume", 0),
                    new XElement("AmbientVolume", 0),
                    new XComment("Graphics"),
                    new XElement("TextureDetail", 2),
                    new XElement("ShadowDetail", 0),
                    new XElement("AntiAliasing", 0),
                    new XElement("ScreenMode", 2),
                    new XElement("ScreenResolution", "1920x1080"),
                    new XComment("Binds"),
                    new XElement("Forwards", (int) KeyCode.W),
                    new XElement("Backwards", (int) KeyCode.S),
                    new XElement("Right", (int) KeyCode.D),
                    new XElement("Left", (int) KeyCode.A),
                    new XElement("Ability", (int) KeyCode.Q),
                    new XElement("Interact", (int) KeyCode.E),
                    new XElement("Jump", (int) KeyCode.Space),
                    new XElement("ResDropDownValue", 0)
                )
            );
            document.Save(Constants.FilePath);
            _foundDocument = document;
            DocumentHandler();
        }

        public void GenerateNewDirectory()
        {
            Directory.CreateDirectory(Constants.DirectoryPath);
            GenerateNewFile();
            _foundDocument = XDocument.Load(Constants.FilePath);
            DocumentHandler();
        }
    }
    [System.Serializable]
    public class SettingsDataBase
    {
        public List<DataEntry> list = new List<DataEntry>();
    }
