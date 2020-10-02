using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class HandleAsync : MonoBehaviour
{
    public static HandleAsync instance;
    private bool imageRetrieved;
    private bool isDone;
    private new string name;
    private bool ownsNothing;
    private string price;
    private List<int> tempFriendIDList = new List<int>();
    private List<string> tempFriendList = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
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
        form.AddField("user", PlayerVariables.UserName);
        var www = new WWW(Constants.WebServer + "basicdata.php", form);
        yield return www;
        if (www.text[0] == '0')
        {
            //GetData
            PlayerVariables.Coins = int.Parse(www.text.Split('\t')[1]);
            PlayerVariables.CurrentXP = int.Parse(www.text.Split('\t')[3]);
            PlayerVariables.UserID = int.Parse(www.text.Split('\t')[4]);
            Constants.ID = PlayerVariables.UserID;
            Routine(GetUserOwnedSkins());
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
                Debug.Log("I'm here");
            }
            else
            {
                var bytes = www.downloadHandler.data;

                var texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);

                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                Skins.PopulateSkinImage(sprite);
                Debug.Log("I'm here");
                imageRetrieved = true;
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
            ownsNothing = true;
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
        if (!ownsNothing)
        {
            var OwnedSkinsIntList = PlayerVariables.OwnedSkinsJson.Select(t => int.Parse(t.ToString())).ToList();

            AllSkinsIntList.AddRange(PlayerVariables.AllSkinsJson.Select(t => int.Parse(t.ToString())));
            var SkinsToGetInfo = AllSkinsIntList.Except(OwnedSkinsIntList).ToList();
            foreach (var t in SkinsToGetInfo)
            {
                isDone = false;
                StartCoroutine(PopulateSkins(t.ToString()));
                StartCoroutine(CreateSprites(t.ToString()));
                yield return new WaitUntil(() => isDone = true);
                yield return new WaitUntil(() => imageRetrieved = true);
                Skins.PopulateSkinScroller(name, price);
            }
        }
        else
        {
            AllSkinsIntList.AddRange(PlayerVariables.AllSkinsJson.Select(t => int.Parse(t.ToString())));
            var SkinsToGetInfo = AllSkinsIntList;
            foreach (var t in SkinsToGetInfo)
            {
                isDone = false;
                imageRetrieved = false;
                StartCoroutine(PopulateSkins(t.ToString()));
                StartCoroutine(CreateSprites(t.ToString()));
                yield return new WaitUntil(() => isDone = true);
                yield return new WaitUntil(() => imageRetrieved = true);
                Skins.PopulateSkinScroller(name, price);
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
            name = www.text.Split('\t')[1];
            price = www.text.Split('\t')[2];
            isDone = true;
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
                Debug.Log(www.downloadHandler.text);
                var jsonArrayStr = www.downloadHandler.text;
                var jsonArray = JSON.Parse(jsonArrayStr) as JSONArray;
                if (jsonArray.Count <= 0)
                {
                    //No Friends
                    //TODO Show ADD FRIEND BUTTON
                    Debug.Log("No Friends");
                }
                else
                {
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

                        tempFriendList.Add(friendUser);
                        tempFriendIDList.Add(friendID);
                        Debug.Log(i + " user: " + friendUser + " friendid: " + friendID);
                    }

                    Debug.Log("I finished the loop");
                    UIManager.instance.CreateFriends(tempFriendList, tempFriendIDList);
                    tempFriendList = null;
                    tempFriendIDList = null;
                }

               
            }
        }
    }
}