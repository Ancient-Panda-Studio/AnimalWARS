using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * THIS SCRIPT WILL HANDLE PLAYER INPUTS AND SERVER ANSWERS TO THOSE INPUTS
 */
//[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
 public CharacterController playerController;
 public float maxYVelocity = 10f, maxSpeed = 5f;
 public GameObject shield;
 [Range(0f, 1f)] public float jumpControl = .3f;
 public float jumpHeight = 1f;
 private float _currentSpeed;
 private float _currentYVelocity;
 private bool setShieldAviable = true;

 public float rotationSmoothDuration = .1f;
 public float speedSmoothDuration = .1f;

 public float gravitationalForce = -12f;
 private float _rotationSmoothVelocity, _speedSmoothVelocity;

 public Animator characterAnimator;
 private Vector2 _inputDir;
 private Transform _camaraTransform;
 public bool isDead = false;
 public bool isAttacking;
 public bool ultimateAviable = true;

 public float ShieldTimer = 6f;
 public float NextShieldAviable = 6f;
 public float currentShieldTime = 0;
 public Transform attackBegin;
 float currentHitDistance;
 public float SphereCastRadius;
 public float MaxSphereCastDistance;
 public GameObject bambuStrike;
 public GameObject targetUltimate;
 public Animator MyAnimator;

 public float AttackSpeed = 1f;
 public float lastYBeforeUltimate;
 private float NextAttack;
 public LayerMask SphereLayer;
 public LayerMask ultimateLayer;

 public enum PlayerClass
 {
  Panda = 0,
  Tiger = 1,
  Wolf
 }

 public PlayerClass myClass;
 private void Awake()
 {
//  MyAnimator.enabled = false;
 }

 private void Start()
 {
  if (!(Camera.main is null)) _camaraTransform = Camera.main.transform;
  //MyAnimator = GetComponent<Animator>();
 }



 #region LOCALINPUTS

// ReSharper disable Unity.PerformanceAnalysis
 private void DeleteUltimate()
 {
  Camera.main.GetComponent<CameraController>().LockCursor();

  playerController.enabled = true;
  //transform.position = new Vector3(transform.position.x, lastYBeforeUltimate, transform.position.z);
  //MyAnimator.enabled = false;
  if(myClass != PlayerClass.Panda) return;
  if (!bambuStrike.activeSelf) return;
  targetUltimate.SetActive(false);
  bambuStrike.SetActive(false);
 }

 private void UltimateAviableAgain()
 {
  ultimateAviable = true;
 }

 private void Update()
 {
  if (isDead)
  {
   //Attack();
  }
  else
  {
   if (myClass == PlayerClass.Panda)
   {
    if (targetUltimate.activeSelf && myClass == PlayerClass.Panda)
    {
     ShowUltimateSelector();
    }

    if (Input.GetKeyDown(KeyCode.F) && myClass == PlayerClass.Panda)
    {
     if (!bambuStrike.activeSelf)
     {
      if (ultimateAviable)
      {
       if (TutorialManager.Instance != null)
       {
        TutorialManager.Instance.UpdateUltiText();
       }

       StartUlimate();
       Invoke("DeleteUltimate", 5f);
       Invoke("UltimateAviableAgain", 25f);
      }
      else
      {
       if (TutorialManager.Instance != null)
       {
        TutorialManager.Instance.ShowAlertText("You may only use your ultimate every 25 seconds");
       }
      }
     }
    }

    if (Input.GetMouseButtonDown(1) && myClass == PlayerClass.Panda)
    {
     if (!shield.activeSelf)
     {
      if (setShieldAviable)
      {
       StartShield();
       Invoke("RemoveShield", 6f);
       Invoke("SetShieldAviable", 12f);
      }
      else
      {
       if (TutorialManager.Instance != null)
       {
        TutorialManager.Instance.ShowAlertText("You may only use shield every 6 seconds");
       }
      }
     }
     else
     {
      CancelInvoke("RemoveShield");
      RemoveShield();
     }
    }

    if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && myClass == PlayerClass.Panda)
    {
     if (Time.time >= NextAttack)
     {
      Debug.Log("AttackCalled");
      NextAttack = Time.time + AttackSpeed;
      Attack();
     }
    }

    if (shield.activeSelf && _inputDir != Vector2.zero && myClass == PlayerClass.Panda)
    {
     if (TutorialManager.Instance != null)
     {
      TutorialManager.Instance.ShowAlertText(
       "You move at a diminished speed while shielding press RMB again to cancel the shield");
     }

     _currentSpeed = 3;
    }
    else
    {
     _currentSpeed = 6;
    }

    _inputDir = GetInputs().normalized;
    Move(_inputDir);

    if (Input.GetKeyDown(KeyCode.E) && _inputDir.magnitude == 0)
    {
     characterAnimator.Play("EMOTE");
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
     Jump();
     SetAnimationValue("Jump", true);
    }

    SetAnimationValue("SpeedController", _inputDir.magnitude == 0 ? 0f : 1f);
   }
   else
   {
    _currentSpeed = 6;
    _inputDir = GetInputs().normalized;
    Move(_inputDir);

    if (Input.GetKeyDown(KeyCode.E) && _inputDir.magnitude == 0)
    {
     characterAnimator.Play("EMOTE");
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
     Jump();
     SetAnimationValue("Jump", true);
    }

    SetAnimationValue("SpeedController", _inputDir.magnitude == 0 ? 0f : 1f);
   }
  }
 }

 private void StartUlimate()
 {
  playerController.enabled = false;
  lastYBeforeUltimate = transform.position.y;

  //MyAnimator.enabled = true;
  ultimateAviable = false;
   bambuStrike.SetActive(true);
   //MyAnimator.Play("PandaUltimateAnimation");
   ShowUltimateSelector();
 }

 private void ShowUltimateSelector()
 {
  if (!targetUltimate.activeSelf)
  {
   transform.position = new Vector3(transform.position.x, 2, transform.position.z);
   targetUltimate.SetActive(true);
   Camera.main.GetComponent<CameraController>().UnLockCursor();
   //MyAnimator.enabled = true;
  }
  Debug.Log("Raycast is called");
  Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
  RaycastHit hit;
  if (!Physics.Raycast(castPoint, out hit, Mathf.Infinity, ultimateLayer)) return;
  Debug.Log("Raycast is hit");

  if (Input.GetMouseButtonDown(0))
  {
   Debug.Log("Transform has been called");
   transform.position = hit.point;
   CancelInvoke("DeleteUltimate");
   DeleteUltimate();
  }
  targetUltimate.transform.position = hit.point;
 }

 private void StartShield()
  {
   shield.SetActive(true);
   setShieldAviable = false;
  }

  void SetShieldAviable()
  {
   setShieldAviable = true;
  }
  void RemoveShield()
  {
   if (shield.activeSelf)
   {
    shield.SetActive(false);
    NextShieldAviable = Time.time + ShieldTimer;
   }
  }
  private void OnDrawGizmos()
  {
   /*Gizmos.color = Color.red;
   Gizmos.DrawWireSphere(attackBegin.position, SphereCastRadius);*/
  }

  private void Attack()
  {

   var x = Physics.OverlapSphere(attackBegin.position, SphereCastRadius, SphereLayer);
   foreach (var collider in x)
   {
    if (collider.gameObject.GetComponent<DummyLoop>())
    {
     var dml = collider.gameObject.GetComponent<DummyLoop>();
     if (dml.dummyType == DummyLoop.TypeOfDummy.Defense)
     {
      if (dml.Shield.activeSelf)
      {
       TutorialManager.Instance.ShowAlertText("Do not attack the enemy while its protective shield is active!");
      }
      else
      {
       TutorialManager.Instance.NewStage();
      }
     }
    }
    Debug.Log(collider.gameObject.tag);
   }
  }

  private void Move(Vector2 inputDir)
  {
   if (inputDir != Vector2.zero)
   {
    SetCharacterRotation();
   }

   _currentSpeed = CalculateSpeed() * inputDir.magnitude;
   ApplyGravity();
   ApplyCharacterMovement();
   //SetAnimationValue("VelocityY", currentYVelocity);
   if (playerController.isGrounded)
   {
    if (characterAnimator.GetBool("Jump"))
    {
     SetAnimationValue("Jump", false);
    }

    _currentYVelocity = 0f;
   }

   SendMovement();
  }

  private void Jump()
  {
   if (!playerController.isGrounded) return;
   Debug.Log("Jumping");
   var jumpVel = Mathf.Sqrt(-2 * gravitationalForce * jumpHeight);
   _currentYVelocity = jumpVel;
  }

  private float CalculateSpeed()
  {
   return Mathf.SmoothDamp(_currentSpeed, maxSpeed, ref _speedSmoothVelocity,
    GetModifiedSmoothTime(speedSmoothDuration));
  }

  private Vector2 GetInputs()
  {
   return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
  }

  private void SetCharacterRotation()
  {
   float targetRotation = (Mathf.Atan2(_inputDir.x, _inputDir.y) * Mathf.Rad2Deg + _camaraTransform.eulerAngles.y);
   transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
    ref _rotationSmoothVelocity, GetModifiedSmoothTime(rotationSmoothDuration));
  }

  private void ApplyGravity()
  {

   if (_currentYVelocity <= maxYVelocity)
   {
    _currentYVelocity += Time.deltaTime * gravitationalForce;
   }
  }

  private void ApplyCharacterMovement()
  {


   var velocity = transform.forward * _currentSpeed + Vector3.up * _currentYVelocity;
   playerController.Move(velocity * Time.deltaTime);
  }

  private void SetAnimationValue(string valueName, bool value)
  {
   characterAnimator.SetBool(valueName, value);

  }

  private void SetAnimationValue(string valueName, int value)
  {
   characterAnimator.SetInteger(valueName, value);

  }

  private void SetAnimationValue(string valueName, float value)
  {
   characterAnimator.SetFloat(valueName, value, speedSmoothDuration, Time.deltaTime);
  }

  private float GetModifiedSmoothTime(float smoothTime)
  {
   if (playerController.isGrounded)
   {
    return smoothTime;
   }

   if (jumpControl == 0f)
   {
    return float.MaxValue;
   }

   return smoothTime / jumpControl;
  }

  #endregion

  #region UPDATESERVERINPUTS

  private void SendMovement()
  {
   var transform1 = transform;
   ClientSend.PlayerMovement(transform1.position, transform1.rotation);
  }

  #endregion

  public void SetDeathState()
  {
   isDead = !isDead;
  }
}