// This Document has quite an un-convectional formatting style. Just playing around with things.

using UnityEngine;
using UnityEngine.InputSystem;

namespace PP.Entities.Player {

  [RequireComponent(typeof(CharacterController))]
  public class Locomotion : MonoBehaviour {


    private void Update() {
      MoveTick();
      LookTick();
    }


    // -------------------------------------------------------
    //                Movement and Jump Code
    // -------------------------------------------------------

    private Vector3 velocity;

    private float jumpInputTime = float.NegativeInfinity;

    private bool doSneak = false;
    private bool doWalk = false;

    private Vector2 moveInput;

    

    private void MoveTick() {

      // Transform the input to world space.
      Vector3 input = new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime;
      input = transform.TransformVector(input);

      if (character.isGrounded) {
        // Set our speeds.
        if (doSneak)
          input *= sneakSpeed;
        else if (doWalk)
          input *= walkSpeed;
        else
          input *= runSpeed;

        // Apply our input to the velocity.
        velocity.x = input.x;
        velocity.z = input.z;

        // Jump if the jump key was pressed recently.
        if (jumpInputTime + jumpFudgeTime > Time.timeSinceLevelLoad) {
          velocity.y = jumpImpulse;
          animator.SetTrigger(jumpHash);
          jumpInputTime = float.NegativeInfinity;
        }
      } else {
        // Accelerate our velocity while falling.
        velocity += input * fallMoveSpeed;
        // Apply Gravity.
        velocity.y -= gravity * Time.deltaTime;
      }

      // Move the character.
      character.Move(velocity);
      animator.SetFloat(moveSpeedHash, character.velocity.magnitude);
    }

    public void OnMove(InputAction.CallbackContext context) {
      moveInput = context.action.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context) {
      doSneak = context.ReadValueAsButton();
    }

    public void OnWalk(InputAction.CallbackContext context) {
      doWalk = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context) {
      if (context.started)
        jumpInputTime = Time.timeSinceLevelLoad;
    }





    // -------------------------------------------------------
    //              Head Code
    // -------------------------------------------------------

    private Vector2 lookInput;

    [SerializeField]
    private Transform head;
    const float HEAD_LIMIT = 89.9f;

    float headAngle = 0;



    public void OnLook(InputAction.CallbackContext context) {
      lookInput = context.action.ReadValue<Vector2>();
      // TODO: Tie look input to deltaTime UNLESS we are using a mouse.
    }

    private void LookTick() {
      // Rotate the root gameobject of the player.
      transform.Rotate(Vector3.up, lookInput.x, Space.Self);

      // Rotate the x axis of the player's head with some sensible limits.
      headAngle = Mathf.Clamp(headAngle - lookInput.y, -HEAD_LIMIT, HEAD_LIMIT);
      head.localRotation = Quaternion.Euler(headAngle, 0, 0);

      // Tell the animator how to blend our look up and down animations.
      animator.SetFloat(headLookHash, Mathf.InverseLerp(-HEAD_LIMIT, HEAD_LIMIT, headAngle));
    }





    // -------------------------------------------------------
    //            Set-up and Caching of objects
    // -------------------------------------------------------

    [SerializeField]
    private Animator animator;

    // Cached animation controller string hashes.
    private readonly int fallingHash = Animator.StringToHash("IsFalling");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int moveSpeedHash = Animator.StringToHash("Move Speed");
    private readonly int headLookHash = Animator.StringToHash("Head Look Vertical");

    // Get the character controller onEnable.
    private CharacterController character;
    private void OnEnable() {
      character = GetComponent<CharacterController>();
    }





    // -------------------------------------------------------
    //            Paramaters exposed to the editor
    // -------------------------------------------------------

    // Move speeds.
    [SerializeField]
    private float sneakSpeed = 0.5f;
    [SerializeField]
    private float walkSpeed = 1.5f;
    [SerializeField]
    private float runSpeed = 3f;
    [SerializeField]
    private float fallMoveSpeed = 1f;

    [SerializeField]
    private float jumpFudgeTime = 0.2f;
    [SerializeField]
    private float jumpImpulse = 200;

    [SerializeField]
    private float gravity = 0.7f;
  }
}