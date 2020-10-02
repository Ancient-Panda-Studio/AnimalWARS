using Network;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class FriendController : MonoBehaviour
    {
        [HideInInspector] public bool connectionState;
        [HideInInspector] public string name;
        [HideInInspector] public int playerID;

        public TMP_Text connectionText;
        public TMP_Text nameText;

        private void Start()
        {
            nameText.text = name;
            switch (connectionState)
            {
                case true:
                    connectionText.text = "On";
                    break;
                case false:
                    connectionText.text = "Off";
                    break;
            }
        }

        public void Invite()
        {
            //Debug.Log(nameText.text);
            ClientSend.SendInvitation(playerID, Constants.Username, nameText.text);
        }
    }
}