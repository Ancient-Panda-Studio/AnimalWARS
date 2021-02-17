using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FindPlayersSql : MonoBehaviour
{
    public static FindPlayersSql Instance;

    public GameObject sqlDataShower;
    public Transform sqlDataHolder;
    public InputField usernameText;
    public string CurrentJson;
    private List<string> currentUserCards = new List<string>();
    private Dictionary<string, GameObject> current = new Dictionary<string, GameObject>();
    private bool _destroyAllCards;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);

        }
    }
    public void FindFriends()
    {
        if (usernameText.text == "")
        {
            Debug.Log("Empty :v");
            sqlDataShower.SetActive(false);
        }
        else
        {
            sqlDataShower.SetActive(true);
            StartCoroutine(HandleAsync.Instance.FindUsers(usernameText.text));
        }
    }

    public void DestroyAll(bool t)
    {
        _destroyAllCards = t;
    }

    public IEnumerator PopulateQuery(string jsonArrayStr)
    {

        if (jsonArrayStr == null)
        {
            if (currentUserCards.Count != 0)
            {
                foreach (var strName in current.Keys.ToList())
                {
                    GameObject tempGm = current[strName];
                    tempGm.SetActive(false);
                    current.Remove(strName);
                    Destroy(tempGm);
                    currentUserCards = new List<string>();
                }
            }
            yield break;
        }
        else
        {
            var jsonArray = JSON.Parse(jsonArrayStr) as JSONArray;
            if (currentUserCards.Count != 0)
            {
                foreach (var strName in current.Keys.ToList())
                {
                    GameObject tempGm = current[strName];
                    tempGm.SetActive(false);
                    current.Remove(strName);
                    Destroy(tempGm);
                    currentUserCards = new List<string>();
                }
            } //Destroy ALL CARDS
            List<string> newCards = new List<string>();
            if (currentUserCards.Count != 0)
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    string userNameToAdd = jsonArray[i].AsObject["username"];
                    newCards.Add(userNameToAdd);
                }
                List<string> finalNewCards = currentUserCards.Union(newCards).ToList();
                for (int i = 0; i < finalNewCards.ToList().Count; i++)
                {
                    var user = Instantiate(Resources.Load("Prefabs/UserFound") as GameObject, sqlDataHolder, false);
                    var y = user.GetComponent<AddFriend>();
                    y.txt.text = finalNewCards[i];
                    Debug.Log(i + " is the number for" + finalNewCards[i]);
                    current.Add(finalNewCards[i], user);
                }
                currentUserCards = finalNewCards;
            }
            else
            {
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    string userNameToAdd = jsonArray[i].AsObject["username"];
                    currentUserCards.Add(userNameToAdd);
                    var user = Instantiate(Resources.Load("Prefabs/UserFound") as GameObject, sqlDataHolder, false);
                    var y = user.GetComponent<AddFriend>();
                    y.txt.text = currentUserCards[i];
                    current.Add(currentUserCards[i], user);
                }
            }
        }
        yield return null;
    }

}