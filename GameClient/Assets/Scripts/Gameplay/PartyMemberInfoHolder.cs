using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberInfoHolder : MonoBehaviour
{
   public TMP_Text nameText;
   public int id;
   
   
   public void Remove()
   {
      UIManager.Instance.PartyMembers.Remove(id);
      Destroy(this);
      if (UIManager.Instance.PartyMembers.Count == 1)
      {
         UIManager.Instance.DisbandParty();
      }
   }
}
