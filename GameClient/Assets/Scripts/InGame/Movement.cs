using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace InGame
{
	[RequireComponent(typeof(CharacterController))]
	public class Movement : MonoBehaviour
	{
		public float walkSpeed = 2;
		public float runSpeed = 6;
		public float gravity = -12;
		public float jumpHeight = 1;
		[Range(0,1)]
		public float airControlPercent;

		public float turnSmoothTime = 0.2f;
		private float _turnSmoothVelocity;

		public float speedSmoothTime = 0.1f;
		private float _speedSmoothVelocity;
		private float _currentSpeed;
		private float _velocityY;

		private Transform _cameraT;
		public Transform godCamera;
		private CharacterController _controller;
		private float _flySpeed = 0.5f;
		private float _accelerationAmount  = 3f;
		private readonly float accelerationRatio= 1f;
		private float slowDownRatio = 0.5f;
		private Vector2 currentRotation;
		private Vector3 lastCamPos;
		private Quaternion lastCamRot;
		public static Movement Instance;
		float mainSpeed = 100.0f; //regular speed
		float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
		float maxShift = 1000.0f; //Maximum speed when holdin gshift
		float camSens = 0.25f; //How sensitive it with mouse
		private float totalRun= 1.0f;
		private bool _enableCam = true;
		private Vector3 spawn;
		private Quaternion spawn_rot;
		private void Start ()
		{
			Instance = this;
			spawn = this.transform.position;
			spawn_rot =this.transform.rotation;
			if (!(Camera.main is null)) _cameraT = Camera.main.transform;
			_controller = GetComponent<CharacterController> ();
		}
		public void ResetPos(){
		this.transform.position = spawn;
		this.transform.rotation = spawn_rot;
		}
		private void Update () {
			// input
			if (GameLoop.Instance.IsGod())
			{
				GodMovement();
			}
			else
			{
				if (godCamera.gameObject.activeSelf)
				{
					SwitchCamera();
				}
				if (GameLoop.Instance.IsPaused()) return;
				var input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
				var inputDir = input.normalized;
				var running = Input.GetKey (KeyCode.LeftShift);

				if (!_enableCam) return;
				Move(inputDir, running);

				if (Input.GetKeyDown(KeyCode.Space))
				{
					Jump();
				}
			}
		
		}
		private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
			Vector3 p_Velocity = new Vector3();
			if (Input.GetKey (KeyCode.W)){
				p_Velocity += new Vector3(0, 0 , 1);
			}
			if (Input.GetKey (KeyCode.S)){
				p_Velocity += new Vector3(0, 0, -1);
			}
			if (Input.GetKey (KeyCode.A)){
				p_Velocity += new Vector3(-1, 0, 0);
			}
			if (Input.GetKey (KeyCode.D)){
				p_Velocity += new Vector3(1, 0, 0);
			}
			return p_Velocity;
		}
		private void GodMovement()
		{
			var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			var movSpeed = 15f;
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				var transform1 = godCamera.transform;
				transform1.position += (-transform1.right * (movSpeed * Time.deltaTime));
			}

			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				var transform1 = godCamera.transform;
				transform1.position += (transform1.right * (movSpeed * Time.deltaTime));
			}

			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				var transform1 = godCamera.transform;
				transform1.position += (transform1.forward * (movSpeed * Time.deltaTime));
			}

			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				var transform1 = godCamera.transform;
				transform1.position += (-transform1.forward * (movSpeed * Time.deltaTime));
			}
			
			if (Input.GetKey(KeyCode.Space))
			{
				var transform1 = godCamera.transform;
				transform1.position += (Vector3.up * (movSpeed * Time.deltaTime));
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				var transform1 = godCamera.transform;
				transform1.position += (-Vector3.up * (movSpeed * Time.deltaTime));
			}
			var localEulerAngles = godCamera.transform.localEulerAngles;
			var newRotationX = localEulerAngles.y + Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
			var newRotationY = localEulerAngles.x - Input.GetAxis("Mouse Y") * 1000f * Time.deltaTime;
			localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
			godCamera.transform.localEulerAngles = localEulerAngles;
			if (Input.GetKeyDown(KeyCode.F12))	
				SwitchCamera();
			if (Input.GetKeyDown(KeyCode.M))
				transform.position = godCamera.position; //Moves the player to the flycam's position. Make sure not to just move the player's camera.
			if (Input.GetKeyDown(KeyCode.N))
			{
				godCamera.position = lastCamPos;
				godCamera.rotation = lastCamRot;
			}
		}
		private void Move(Vector2 inputDir, bool running) {
			if (inputDir != Vector2.zero) {
				var targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + _cameraT.eulerAngles.y;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
			}
			var targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
			_currentSpeed = Mathf.SmoothDamp (_currentSpeed, targetSpeed, ref _speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

			_velocityY += Time.deltaTime * gravity;
			var velocity = transform.forward * _currentSpeed + Vector3.up * _velocityY;

			_controller.Move (velocity * Time.deltaTime);
			var controllerVelocity = _controller.velocity;
			_currentSpeed = new Vector2 (controllerVelocity.x, controllerVelocity.z).magnitude;

			if (_controller.isGrounded) {
				_velocityY = 0;
			}

		}

		public void SwitchCamera()
		{
			lastCamPos = _cameraT.transform.position;
			lastCamRot = _cameraT.transform.rotation;
			godCamera.gameObject.SetActive(!godCamera.gameObject.activeSelf);
			_cameraT.gameObject.SetActive(!_cameraT.gameObject.activeSelf);
		}
		private void Jump()
		{
			if (!_controller.isGrounded) return;
			var jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			_velocityY = jumpVelocity;
		}

		private float GetModifiedSmoothTime(float smoothTime) {
			if (_controller.isGrounded) {
				return smoothTime;
			}

			if (airControlPercent == 0) {
				return float.MaxValue;
			}
			return smoothTime / airControlPercent;
		}

		public void DisableCamaraMovement(bool b)
		{
			_cameraT.GetComponent<CinemachineBrain>().enabled = b;
			_enableCam = b;
			Cursor.lockState = b ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !b;
		}
	}
}
