using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
  public void Disable()
  {
    gameObject.SetActive(false);
  }
}
