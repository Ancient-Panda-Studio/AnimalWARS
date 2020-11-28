using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace InGame
{
    public class GameLoop : MonoBehaviour
    {
        private float _startTimeDeltaTime;
        private bool _pause;
        private bool _god;
        public static GameLoop Instance;
        private void Start()
        {
            _startTimeDeltaTime = Time.timeScale;
            Instance = this;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                Movement.Instance.SwitchCamera();
                _god = !_god;
            }
            if(_pause) return;
            if (Input.GetKeyDown(KeyCode.F11))
            {
                StartCoroutine(Unpause());
                Time.timeScale = 0f;
                
                _pause = true;
            }
        }

        public bool IsGod()
        {
            return _god;
        }
        public bool IsPaused()
        {
            return _pause;
        }

        private IEnumerator Unpause()
        {
            yield return new WaitForSecondsRealtime(3f);
            Time.timeScale = _startTimeDeltaTime;
            _pause = false;
        }
    }
}
