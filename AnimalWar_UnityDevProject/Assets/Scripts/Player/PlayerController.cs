using Network;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
       /* public Bindings binds;
        public float rotateSpeed = 0.1f;
        public float rotation = 0.1f;

        private void Update()
        {
            var charRotation = transform.eulerAngles + new Vector3(0, Input.GetAxisRaw("Mouse X") * rotateSpeed, 0);
            transform.eulerAngles = charRotation;
        }

        private void FixedUpdate()
        {
            SendInputToServer();
        }
        
        /// <summary>Sends player input to the server.</summary>
        Private void SendInputToServer()
        {
            bool[] inputs =
            {
                Input.GetKey(binds.forward),
                Input.GetKey(binds.jump)
            };
            ClientSend.PlayerMovement(inputs);
        }*/
    }
}