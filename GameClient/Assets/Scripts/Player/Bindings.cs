using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player
{
    [Serializable]
    public class Bindings
    {
        public List<KeyCode> CurrentKeys;
        public KeyCode forward, backwards, left, right, jump, interact, ability;

        public void AddToDictionary(KeyCode keyCode)
        {
            CurrentKeys.Add(forward);
            CurrentKeys.Add(backwards);
            CurrentKeys.Add(left);
            CurrentKeys.Add(right);
            CurrentKeys.Add(interact);
            CurrentKeys.Add(ability);
            CurrentKeys.Add(jump);
        }
    }
}