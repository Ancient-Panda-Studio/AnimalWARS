using UnityEngine;
using  System.Collections;
using  System.Collections.Generic;
using  System.Xml;
using  System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class XMLDataManager : MonoBehaviour
{
    public static XMLDataManager Instance;

    private void Awake()
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
        if (!File.Exists(@"C:\Users\oriol\Documents\AnimalWars\Settings\Settings.xml"))
        { 
            Entry.GenerateNewFile(); 
            return;
        }
        Entry._foundDocument = XDocument.Load(@"C:\Users\oriol\Documents\AnimalWars\Settings\Settings.xml");
        Entry.DocumentHandler();
    }

    public DataEntry Entry;
}

[System.Serializable]
    public class DataEntry
    {
        public Player.ConfigurationManager manager;
        public XDocument _foundDocument;

        
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
                
                case 4:
                    Debug.Log("Updating Overall");
                    var overallElement = _foundDocument.Descendants("Settings").Elements("Overall").FirstOrDefault();
                    overallElement?.SetValue(intValue);
                    break;
                
                case 5:
                    var textureElement = _foundDocument.Descendants("Settings").Elements("TextureDetail").FirstOrDefault();
                    textureElement?.SetValue(intValue);
                    break;
                case 6:
                    var shadowElement = _foundDocument.Descendants("Settings").Elements("ShadowDetail").FirstOrDefault();
                    shadowElement?.SetValue(intValue);
                    break;
                case 7:
                    var aaElement = _foundDocument.Descendants("Settings").Elements("AntiAliasing").FirstOrDefault();
                    aaElement?.SetValue(intValue);
                    break;
                case 8:
                    var screenModeElement = _foundDocument.Descendants("Settings").Elements("ScreenMode").FirstOrDefault();
                    screenModeElement?.SetValue(intValue);
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
            }
            _foundDocument.Save(@"C:\Users\oriol\Documents\AnimalWars\Settings\Settings.xml");

        }
        public void DocumentHandler()
        { 
                if (!File.Exists(@"C:\Users\oriol\Documents\AnimalWars\Settings\Settings.xml"))
                {
                    GenerateNewFile();
                    return;
                }

                //ReadsXML And Applies 
                var readMasterVolume = (float) _foundDocument.Descendants("MasterVolume").FirstOrDefault();
                var readMusicVolume = (float) _foundDocument.Descendants("MusicVolume").FirstOrDefault();
                var readSfxVolume = (float) _foundDocument.Descendants("SfxVolume").FirstOrDefault();
                var readAmbientVolume = (float) _foundDocument.Descendants("AmbientVolume").FirstOrDefault();
                var readOverall = (int) _foundDocument.Descendants("Overall").FirstOrDefault();
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


                manager.forwardKeyBindText.text = ((KeyCode) readForwardKey).ToString();
                manager.backwardsKeyBindText.text = ((KeyCode) readBackwardKey).ToString();
                manager.leftKeyBindText.text = ((KeyCode) readLeftKey).ToString();
                manager.rightKeyBindText.text = ((KeyCode) readRightKey).ToString();
                manager.abilityKeyBindText.text = ((KeyCode) readAbilityKey).ToString();
                manager.interactKeyBindText.text = ((KeyCode) readInteractKey).ToString();
                manager.jumpKeyBindText.text = ((KeyCode) readJumpKey).ToString();

                manager.masterVolumeSlider.value = readMasterVolume;
                manager.ambientVolumeSlider.value = readAmbientVolume;
                manager.musicVolumeSlider.value = readMusicVolume;
                manager.sfxVolumeSlider.value = readSfxVolume;



                switch (readScreenMode)
                {
                    case 0:
                        manager.scModeHigh.isDefault = true;
                        break;
                    case 1:
                        manager.scModeMid.isDefault = true;

                        break;
                    case 2:
                        manager.scModeLow.isDefault = true;

                        break;
                }
                
                switch (readShadowDetail)
                {
                    case 0:
                        manager.shadowHigh.isDefault = true;
                        break;
                    case 1:
                        manager.shadowMid.isDefault = true;
                        break;
                    case 2:
                        manager.shadowLow.isDefault = true;
                        break;
                }

                
                switch (readAntiAliasing)
                {
                    case 0:
                        manager.aaHigh.isDefault = true;
                        break;
                    case 1:
                        manager.aaMid.isDefault = true;
                        break;
                    case 2:
                        manager.aaLow.isDefault = true;
                        break;
                }

                
                switch (readTextureDetail)
                {
                    case 0:
                        manager.textureHigh.isDefault = true;

                        break;
                    case 1:
                        manager.textureMid.isDefault = true;

                        break;
                    case 2:
                        manager.textureMid.isDefault = true;
                        break;
                }

                if (readOverall != 3)
                {
                     manager.SetOverall(readOverall);
                     manager.SetResolution(readScreenResolution);
                     manager.SetScreenMode(readScreenMode);
                     
                     
                     
                }
                else
                {
                    manager.SetShadowDetail(readShadowDetail);
                    manager.SetAntiAliasing(readAntiAliasing);
                    manager.SetTextureDetail(readTextureDetail);
                    manager.SetResolution(readScreenResolution);
                    manager.SetScreenMode(readScreenMode);
                }
        }

        public void GenerateNewFile()
        {
            //First start or document deleted create new document with default values
            if (File.Exists(@"C:\Users\oriol\Documents\AnimalWars\Settings\Settings.xml"))
            {
                File.Delete(@"C:\Users\oriol\Documents\AnimalWars\Settings\Settings.xml");
            }
            var document = new XDocument(
                new XElement("Settings",
                    new XComment("Volumes MAX = 1 MIN = -80"),
                    new XElement("MasterVolume", 0),
                    new XElement("MusicVolume", 0),
                    new XElement("SfxVolume", 0),
                    new XElement("AmbientVolume", 0),
                    new XComment("Graphics"),
                    new XElement("Overall", 0),
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
                    new XElement("Jump", (int) KeyCode.Space)
                )
            );
            document.Save(@"C:\Users\oriol\Documents\AnimalWars\Settings\Settings.xml");
            _foundDocument = document;
        }
    }
    
    
    [System.Serializable]
    public class SettingsDataBase
    {
        public List<DataEntry> list = new List<DataEntry>();
    }
