using UnityEngine;

namespace PP.Entities.Player {

  public class Walk : Locomotion.MoveBehaviour {

    readonly int speedHash = Animator.StringToHash("Move Speed");
    readonly int jumpHash = Animator.StringToHash("Jump");
    Transform transform;

    public Walk(Locomotion player) : base(player) {
      transform = player.transform;
    }

    public override void MoveTick(ref Locomotion.InputFrame input, float deltaTime) {
      Vector3 move = new Vector3(input.Move.x, 0, input.Move.y);

      // Convert the input to world space.
      move = transform.TransformVector(move);
      player.Velocity += move * deltaTime;

      float targetSpeed = player.MoveSpeed;
      if (input.Sneak)
        targetSpeed = player.SneakSpeed;

      player.Velocity = AdjustVelocity(move, targetSpeed, deltaTime);

      // Jump only when the player is on the ground and has attempted to jump recently.
      if (player.OnGround && player.jumpTime > Time.timeSinceLevelLoad) {
        player.OnGround = false;
        player.jumpTime = float.NegativeInfinity;   
        player.Velocity.y = player.jumpImpulse;

        player.Animator.SetTrigger(jumpHash);
      }
    }

    private Vector3 AdjustVelocity(Vector3 input, float targetSpeed, float deltaTime) {
      Vector3 velocity = player.Velocity;
      velocity.y = 0;
      float magnitude = velocity.magnitude;

      // Set the move speed value of the animator.
      player.Animator.SetFloat(speedHash, magnitude);

      // If there is input accelerate.
      if (input.magnitude > 0) {
        if (magnitude > targetSpeed * player.SpeedScale)
          velocity = input * targetSpeed * player.SpeedScale;
        else
          velocity += input * deltaTime * player.Acceleration * player.AccelerationScale;
      }

      // If there is no input, decelerate.
      else {
        if (velocity.sqrMagnitude > 1.4f)
          velocity -= velocity.normalized * player.Acceleration * deltaTime * player.AccelerationScale;
        else
          velocity -= velocity * player.Acceleration * deltaTime * player.AccelerationScale;
      }

      velocity.y = player.Velocity.y;
      return velocity;
    }
  }

}