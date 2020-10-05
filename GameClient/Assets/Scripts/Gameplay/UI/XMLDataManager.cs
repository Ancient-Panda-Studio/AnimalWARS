using UnityEngine;
using  System.Collections;
using  System.Collections.Generic;
using  System.Xml;
using  System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
        if (!File.Exists(@"C:\Users\Kedalen\Documents\AnimalWars\Settings\Settings.xml"))
        { 
            Entry.GenerateNewFile(); 
            return;
        }
        Entry._foundDocument = XDocument.Load(@"C:\Users\Kedalen\Documents\AnimalWars\Settings\Settings.xml");
    }

    public DataEntry Entry;
}

[System.Serializable]
    public class DataEntry
    {
        public Player.ConfigurationManager manager;
        [HideInInspector]public float masterVolume;
        [HideInInspector]public float musicVolume;
        [HideInInspector]public float sfxVolume;
        [HideInInspector]public float ambientVolume;
        [HideInInspector]public int worldDetail;
        [HideInInspector]public int textureDetail;
        [HideInInspector]public int shadowDetail;
        [HideInInspector]public int particleDetail;
        [HideInInspector]public int screenMode;
        [HideInInspector]public string forwardKey;
        [HideInInspector]public string backwardKey;
        [HideInInspector]public string leftKey;
        [HideInInspector]public string rightKey;
        [HideInInspector]public string abilityKey;
        [HideInInspector]public string interactKey;
        public XDocument _foundDocument;

        
        public void UpdateXML(int modified,int intValue, float floatValue, string stringValue)
        {
            switch (modified)
            {
                case 0:
                    var masterElement = _foundDocument.Element("MasterVolume");
                    masterElement.Value = ""+floatValue;
                    break;
                case 1:
                    var musicElement = _foundDocument.Element("MusicVolume");
                    musicElement.Value = ""+floatValue;
                    break;
                case 2:
                    var sfxElement = _foundDocument.Element("SfxVolume");
                    sfxElement.Value = ""+floatValue;
                    break;
                case 3:
                    var ambientElement = _foundDocument.Element("AmbientVolume");
                    ambientElement.Value = ""+floatValue;
                    break;
                case 4:
                    var overallElement = _foundDocument.Element("Overall");
                    overallElement.Value = ""+intValue;
                    break;
                case 5:
                    var textureElement = _foundDocument.Element("TextureDetail");
                    textureElement.Value = ""+intValue;
                    break;
                case 6:
                    var shadowElement = _foundDocument.Element("ShadowDetail");
                    shadowElement.Value = ""+intValue;
                    break;
                case 7:
                    var aaElement = _foundDocument.Element("AntiAliasing");
                    aaElement.Value = ""+intValue;
                    break;
                case 8:
                    var screenModeElement = _foundDocument.Element("ScreenMode");
                    screenModeElement.Value = ""+intValue;
                    break;
                case 9:
                    var resolutionElement = _foundDocument.Element("ScreenResolution");
                    resolutionElement.Value = stringValue;
                    break;
                case 10:
                    var forwardElement = _foundDocument.Element("Forwards");
                    forwardElement.Value = "" + intValue;
                    break;
                case 11:
                    var backElement = _foundDocument.Element("Backwards");
                    backElement.Value = "" + intValue;
                    break;
                case 12:
                    var leftElement = _foundDocument.Element("Left");
                    leftElement.Value = "" + intValue;
                    break;
                case 13:
                    var rightElement = _foundDocument.Element("Right");
                    rightElement.Value = "" + intValue;
                    break;
                case 14:
                    var interactElement = _foundDocument.Element("Interact");
                    interactElement.Value = "" + intValue;
                    break;
                case 15:
                    var abilityElement = _foundDocument.Element("Ability");
                    abilityElement.Value = "" + intValue;
                    break;
            }
        }
        public void DocumentHandler(bool reading)
        {
            if (!reading)
            {
                Debug.Log("kekw");
            }
            else
            {
                //ReadData
                if (!File.Exists(@"C:\Users\Kedalen\Documents\AnimalWars\Settings\Settings.xml"))
                {
                    GenerateNewFile();
                    return;
                }

                //ReadsXML And Applies 
                var document = XDocument.Load(@"C:\Users\Kedalen\Documents\AnimalWars\Settings\Settings.xml");
                var readMasterVolume = (float) document.Descendants("MasterVolume").FirstOrDefault();
                var readMusicVolume = (float) document.Descendants("MusicVolume").FirstOrDefault();
                var readSfxVolume = (float) document.Descendants("SfxVolume").FirstOrDefault();
                var readAmbientVolume = (float) document.Descendants("AmbientVolume").FirstOrDefault();
                var readOverall = (int) document.Descendants("Overall").FirstOrDefault();
                var readTextureDetail = (int) document.Descendants("TextureDetail").FirstOrDefault();
                var readShadowDetail = (int) document.Descendants("ShadowDetail").FirstOrDefault();
                var readAntiAliasing = (int) document.Descendants("AntiAliasing").FirstOrDefault();
                var readScreenMode = (int) document.Descendants("ScreenMode").FirstOrDefault();
                var readForwardKey = (int) document.Descendants("Forwards").FirstOrDefault();
                var readBackwardKey = (int) document.Descendants("Backwards").FirstOrDefault();
                var readScreenResolution = (string) document.Descendants("ScreenResolution").FirstOrDefault();
                var readLeftKey = (int) document.Descendants("Left").FirstOrDefault();
                var readRightKey = (int) document.Descendants("Right").FirstOrDefault();
                var readInteractKey = (int) document.Descendants("Interact").FirstOrDefault();
                var readAbilityKey = (int) document.Descendants("Ability").FirstOrDefault();
                
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
        }

        public void GenerateNewFile()
        {
            //First start or document deleted create new document with default values
            if (File.Exists(@"C:\Users\Kedalen\Documents\AnimalWars\Settings\Settings.xml"))
            {
                File.Delete(@"C:\Users\Kedalen\Documents\AnimalWars\Settings\Settings.xml");
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
                    new XElement("Interact", (int) KeyCode.E)
                )
            );
            document.Save(@"C:\Users\Kedalen\Documents\AnimalWars\Settings\Settings.xml");
            _foundDocument = document;
        }
    }
    
    
    [System.Serializable]
    public class SettingsDataBase
    {
        public List<DataEntry> list = new List<DataEntry>();
    }
