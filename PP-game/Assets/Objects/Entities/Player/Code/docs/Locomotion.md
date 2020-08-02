# Locomotion.cs

Locomotion moves the player with various kinds of MoveBehaviours.

By extending the [MoveBehaviour](#MoveBehaviour) class we can create extra movement types. For example, Walking, Gliding, No Clip and Swimming. These can all be in their own class with their own logic. When it comes time to swim, we just set the [Behaviour](#Behaviour) variable of the Locomotion script to the instance of the swim class.

This also provides the ability to create mods to add extra Movement types. Flying in space anyone?

Currently we have one [MoveBehaviour](#MoveBehaviour) called `Walk` in `Walk.cs`.

## Inspector Only Variables


### Animator
The animator component of the player's mesh.

### JumpFudge
How long in seconds to remember that the player has attempted to jump. *Makes jumping feel responsive.*

### JumpImpulse
The upward velocity of the jump in meters per second. *How high*

### MoveSpeed
The speed the player moves at while walking in meters per second.

### SneakSpeed
The speed the player moves while holding sneak in meters per second.

### Acceleration
The acceleration used to gain speed and stop moving.



## Public Variables


### SpeedScale
The factor to scale the speed of the player by. *Think slime or soul sand from minecraft.*

### AccelerationScale
The factor to scale the acceleration of the player by. *Think ice.*

### Velocity
The velocity of the player.

### OnGround
True if the character controller is on the ground. This is set every frame on `Update`.

### Behaviour
The currently in use [MoveBehaviour](#MoveBehaviour). By default this is `Walk` from `Walk.cs`.


## Methods

```cs
// All three of these are called by the PlayerInput component when one of these inputs are changed.
public void OnMove(InputAction.CallbackContext context)
public void OnCrouch(InputAction.CallbackContext context)
public void OnJump(InputAction.CallbackContext context)
```


# Classes

## InputFrame
The Input Frame stores all the inputs that have happened since the last frame for locomotion.

```cs
public class InputFrame {
  // A normalized vector representing the state of WASD or left stick.
  public Vector2 Move;

  // Is the player trying to jump?
  public bool TryJump;

  // Is the player trying to sneak?
  public bool Sneak;
}
```

## MoveBehaviour

The MoveBehavour class is where

```cs
public abstract class MoveBehaviour {

  // A reference to the locomotion script that owns this instance (set in constructor)
  internal Locomotion player;
  protected MoveBehaviour(Locomotion player);

  // Called every frame when this MoveBehaviour is active. 
  public abstract void MoveTick(ref InputFrame input, float deltaTime);

  // When the this MoveBehaviour has been enabled or disabled.
  public virtual void Enable() { }
  public virtual void Disable() { }
}
```