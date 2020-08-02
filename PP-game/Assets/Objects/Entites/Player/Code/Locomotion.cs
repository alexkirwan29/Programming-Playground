using UnityEngine;
using UnityEngine.InputSystem;

namespace PP.Entities.Player {
  [RequireComponent(typeof(CharacterController))]
  public class Locomotion : MonoBehaviour {

    private readonly int fallingHash = Animator.StringToHash("IsFalling");
    [SerializeField] public Animator Animator;

    public float SpeedScale = 1f;
    public float AccelerationScale = 1f;

    [SerializeField] internal float jumpFudge = 0.125f;
    [SerializeField] internal float jumpImpulse = 200;

    [SerializeField] internal float MoveSpeed = 5;
    [SerializeField] internal float SneakSpeed = 1;
    [SerializeField] internal float Acceleration = 1.8f;

    [HideInInspector] public Vector3 Velocity;
    [HideInInspector] public bool OnGround;

    InputFrame input = new InputFrame();
    internal float jumpTime = float.NegativeInfinity;

    private MoveBehaviour behaviour;
    public MoveBehaviour Behaviour {
      get => behaviour;
      set {
        if (behaviour != null)
          behaviour.Disable();

        behaviour = value;

        if (behaviour != null)
          behaviour.Enable();
      }
    }

    private CharacterController character;

    private void Awake() {
      character = GetComponent<CharacterController>();
      Behaviour = new Walk(this);
    }

    private void Update() {
      // Set our ground state.
      OnGround = character.isGrounded;
      Animator.SetBool(fallingHash, !OnGround);

      // If we have a move behaviour, move.
      if (behaviour != null)
        behaviour.MoveTick(ref input, Time.deltaTime);

      // Do some gravity when we are on the ground.
      if (!OnGround)
        Velocity.y += Physics.gravity.y * Time.deltaTime;

      // Move the character every frame based on the velocity.
      character.Move(Velocity * Time.deltaTime);
    }



    // -----------------------------------------------------------------------
    // Events that are called from the PlayerInput component
    // -----------------------------------------------------------------------

    public void OnMove(InputAction.CallbackContext context) {
      input.Move = context.action.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context) {
      input.Sneak = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context) {
      input.TryJump = context.ReadValueAsButton();
      if (context.started)
        jumpTime = Time.timeSinceLevelLoad + jumpFudge;
    }

    // -----------------------------------------------------------------------
    // Classes used within this script
    // -----------------------------------------------------------------------

    public abstract class MoveBehaviour {
      internal Locomotion player;
      protected MoveBehaviour(Locomotion player) {
        this.player = player;
      }
      public abstract void MoveTick(ref InputFrame input, float deltaTime);
      public virtual void Enable() { }
      public virtual void Disable() { }
    }

    public class InputFrame {
      public Vector2 Move;
      public bool TryJump;
      public bool Sneak;
    }
  }
}