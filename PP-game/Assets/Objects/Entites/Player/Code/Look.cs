using UnityEngine;
using UnityEngine.InputSystem;

namespace PP.Entities.Player {

  public class Look : MonoBehaviour {
    [SerializeField] private Transform head;
    [SerializeField] public Animator Animator;

    const float HEAD_LIMIT = 89.9f;

    float headAngle = 0;
    Vector2 lookInput;

    readonly int headLookHash = Animator.StringToHash("Head Look Vertical");

    public void OnLook(InputAction.CallbackContext context) {
      lookInput = context.action.ReadValue<Vector2>();
      // TODO: Tie look input to deltaTime UNLESS we are using a mouse.
    }

    private void Update() {
      // Rotate the root gameobject of the player.
      transform.Rotate(Vector3.up, lookInput.x, Space.Self);

      // Rotate the x axis of the player's head with some sensible limits.
      headAngle = Mathf.Clamp(headAngle - lookInput.y, -HEAD_LIMIT, HEAD_LIMIT);
      head.localRotation = Quaternion.Euler(headAngle, 0, 0);

      // Tell the animator how to blend our look up and down animations.
      Animator.SetFloat(headLookHash, Mathf.InverseLerp(-HEAD_LIMIT, HEAD_LIMIT, headAngle));
    }
  }
}