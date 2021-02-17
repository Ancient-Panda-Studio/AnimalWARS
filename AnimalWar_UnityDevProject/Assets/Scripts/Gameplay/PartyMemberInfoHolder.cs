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
      UIManager.PartyMembers.Remove(id);
      Destroy(this);
      if (UIManager.PartyMembers.Count == 1)
      {
         UIManager.DisbandParty();
      }
   }
}
