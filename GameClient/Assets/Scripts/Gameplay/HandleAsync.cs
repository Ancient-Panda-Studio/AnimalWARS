using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class HandleAsync : MonoBehaviour
{
    public static HandleAsync Instance;
    private bool _imageRetrieved;
    private bool _isDone;
    private new string _name;
    private bool _ownsNothing;
    private string _price;
    private List<int> _tempFriendIDList = new List<int>();
    private List<string> _tempFriendList = new List<string>();

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
    }

    public void Routine(IEnumerator _routine)
    {
        StartCoroutine(_routine);
    }
    public IEnumerator GetUserBasicData()
    {
        var form = new WWWForm();
        form.AddField("user", Constants.Username);
        var www = new WWW(Constants.WebServer + "basicdata.php", form);
        yield return www;
        if (www.text[0] == '0')
        {
            //GetData
            PlayerVariables.FreeCoins = int.Parse(www.text.Split('\t')[1]);
            PlayerVariables.PaidCoins = int.Parse(www.text.Split('\t')[2]);
            PlayerVariables.AccountLevel = int.Parse(www.text.Split('\t')[3]);
            PlayerVariables.CurrentXp = int.Parse(www.text.Split('\t')[4]);
            StartCoroutine(GetUserOwnedSkins());
        }
        else
        {
            Debug.Log(www.text);
        }
    }

    public IEnumerator CreateSprites(string _skinID)
    {
        var form = new WWWForm();
        form.AddField("id", _skinID);
        using (var www = UnityWebRequest.Post(Constants.WebServer + "retrieveskinimage.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var bytes = www.downloadHandler.data;

                var texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);

                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                Skins.PopulateSkinImage(sprite);
                _imageRetrieved = true;
            }
        }
    }

    public IEnumerator GetUserOwnedSkins()
    {
        var form = new WWWForm();
        form.AddField("id", PlayerVariables.UserID);
        var www = new WWW(Constants.WebServer + "populateskins.php", form);
        yield return www;
        if (www.text[0] == '0')
        {
            //GetData
            Debug.Log(www.text);
            _ownsNothing = true;
            StartCoroutine(GenerateSkinsToShow());
        }
        else
        {
            var temp = www.text;
            var noSpaces = temp.Replace(" ", "");
            var clean = Regex.Replace(noSpaces, "\\D+", "");
            PlayerVariables.OwnedSkinsJson = clean;
            StartCoroutine(GenerateSkinsToShow());
        }
    }

    private IEnumerator GenerateSkinsToShow()
    {
        var AllSkinsIntList = new List<int>();
        if (!_ownsNothing)
        {
            var OwnedSkinsIntList = PlayerVariables.OwnedSkinsJson.Select(t => int.Parse(t.ToString())).ToList();
            
            AllSkinsIntList.AddRange(PlayerVariables.AllSkinsJson.Select(t => int.Parse(t.ToString())));
            var SkinsToGetInfo = AllSkinsIntList.Except(OwnedSkinsIntList).ToList();
            foreach (var t in SkinsToGetInfo)
            {
                _isDone = false;
                StartCoroutine(PopulateSkins(t.ToString()));
                StartCoroutine(CreateSprites(t.ToString()));
                yield return new WaitUntil(() => _isDone = true);
                yield return new WaitUntil(() => _imageRetrieved = true);
                Skins.PopulateSkinScroller(_name, _price);
            }
        }
        else
        {
            AllSkinsIntList.AddRange(PlayerVariables.AllSkinsJson.Select(t => int.Parse(t.ToString())));
            var SkinsToGetInfo = AllSkinsIntList;
            foreach (var t in SkinsToGetInfo)
            {
                _isDone = false;
                _imageRetrieved = false;
                StartCoroutine(PopulateSkins(t.ToString()));
                StartCoroutine(CreateSprites(t.ToString()));
                yield return new WaitUntil(() => _isDone = true);
                yield return new WaitUntil(() => _imageRetrieved = true);
                Skins.PopulateSkinScroller(_name, _price);
            }
        }

        yield return null;
    }

    public IEnumerator PopulateSkins(string _skinID)
    {
        var form = new WWWForm();
        form.AddField("id", _skinID);
        var www = new WWW(Constants.WebServer + "getitemdata.php", form);
        yield return www;
        if (www.text[0] == '0')
        {
            //GetData
            _name = www.text.Split('\t')[1];
            _price = www.text.Split('\t')[2];
            _isDone = true;
        }
        else
        {
            Debug.Log(www.text);
        }
    }

    public IEnumerator GetAllSkins()
    {
        var www = new WWW(Constants.WebServer + "getallskins.php");
        yield return www;
        if (www.text[0] == '0')
        {
            //GetData
            Debug.Log(www.text);
        }
        else
        {
            var temp = www.text;
            var noSpaces = temp.Replace(" ", "");
            var clean = Regex.Replace(noSpaces, "\\D+", "");
            PlayerVariables.AllSkinsJson = clean;
        }
    }

    public IEnumerator FindUsers(string txt)
    {
        
        //FIRST DELETE ALL GAME OBJECTS IF THEY ARE UP
        var form = new WWWForm();
        form.AddField("user", txt);
        using (var www = UnityWebRequest.Post(Constants.WebServer + "getusers.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Call callbackfunction to pass result
                if (www.downloadHandler.text == "0")
                {
                    FindPlayersSql.Instance.PopulateQuery(null);
                    yield break;
                }
                var jsonArrayString = www.downloadHandler.text;
                StartCoroutine(FindPlayersSql.Instance.PopulateQuery(jsonArrayString));
            }
        }
    }
    public IEnumerator GetFriends()
    {
        var form = new WWWForm();
        form.AddField("user", Constants.Username);
        using (var www = UnityWebRequest.Post(Constants.WebServer + "getfriends.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var jsonArrayStr = www.downloadHandler.text;
                var jsonArray = JSON.Parse(jsonArrayStr) as JSONArray;
                if (www.downloadHandler.text == "0")
                {
                    //No Friends
                    //TODO Show ADD FRIEND BUTTON
                    Debug.Log("No Friends");
                }
                else
                {
                    
                    //TODO WE NEED SERVER ID AND DB ID 
                    
                    //GET SERVER ID WITH USER NAME
                    
                    if (jsonArray.IsNull)
                    {
                        yield return null;
                    }
                    for (var i = 0; i < jsonArray.Count; i++)
                    {
                        var isDone = false;
                        string friendUser = null;
                        var friendID = 0;
                        if (jsonArray[i].AsObject["userone"] != Constants.Username)
                        {
                            friendUser = jsonArray[i].AsObject["userone"];
                            friendID = jsonArray[i].AsObject["idone"];
                        }

                        if (jsonArray[i].AsObject["usertwo"] != Constants.Username)
                        {
                            friendUser = jsonArray[i].AsObject["usertwo"];
                            friendID = jsonArray[i].AsObject["idtwo"];
                        }
                        GameManager.Instance.FriendsList.Add(friendID,friendUser);
                        _tempFriendList.Add(friendUser);
                        _tempFriendIDList.Add(friendID);
                    }

                    UIManager.Instance.CreateFriends(_tempFriendList, _tempFriendIDList);
                    _tempFriendList = null;
                    _tempFriendIDList = null;
                }

               
            }
        }
    }
}