using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Player
{
    [Serializable]
    public class Bindings
    {
        public static Bindings PlayerBinds = new Bindings();
        [FormerlySerializedAs("CurrentKeys")] public List<KeyCode> currentKeys;
        public KeyCode forward, backwards, left, right, jump, interact, ability;

        public void AddToDictionary(KeyCode keyCode)
        {
            currentKeys.Add(forward);
            currentKeys.Add(backwards);
            currentKeys.Add(left);
            currentKeys.Add(right);
            currentKeys.Add(interact);
            currentKeys.Add(ability);
            currentKeys.Add(jump);
        }
    }
}