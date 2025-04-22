# PlayerCharacterController Documentation

## Overview

The `PlayerCharacterController` script provides basic movement and interaction capabilities for the player character in the BetterWYD project. This document explains the implementation details, library usage, and integration guidelines.

## File Information

- **File Name:** PlayerCharacterController.cs
- **Location:** Assets/Development/Scripts/PlayerCharacterController.cs
- **Namespace:** BetterWYD.Development

## Dependencies

The script depends on the following Unity namespaces:

```csharp
using UnityEngine;          // Core Unity engine functionality
using UnityEngine.InputSystem; // Unity's new Input System package
```

### UnityEngine

The `UnityEngine` namespace provides core functionality including:
- Component-based architecture via MonoBehaviour
- Physics interactions through Rigidbody
- Vector and Quaternion mathematics for movement and rotation
- Time management for frame-rate independent movement

### UnityEngine.InputSystem

The `UnityEngine.InputSystem` namespace (introduced in Unity 2019.1) provides:
- Modern input handling across multiple platforms and devices
- Action-based input via InputValue parameters
- Callback-driven architecture for input events
- Support for different control schemes (keyboard/mouse, gamepad, touch)

## Class Structure

```csharp
public class PlayerCharacterController : MonoBehaviour
{
    // Serialized fields for Unity Inspector
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    
    // Additional properties, methods, and functionality...
}
```

The `PlayerCharacterController` inherits from `MonoBehaviour`, which is the base class for Unity scripting. This allows the script to:
- Be attached to GameObject instances in scenes
- Participate in Unity's lifecycle events (Awake, Start, Update, etc.)
- Access and modify components via GetComponent and related methods

## Inspector Fields

### Movement Settings

```csharp
[Header("Movement Settings")]
[SerializeField] private float moveSpeed = 5f;      // Units per second
[SerializeField] private float rotationSpeed = 10f; // Degrees per second
[SerializeField] private float jumpForce = 5f;      // Force applied when jumping
```

- **Header Attribute:** The `[Header("Movement Settings")]` attribute creates a labeled section in the Inspector, improving organization.
- **SerializeField Attribute:** The `[SerializeField]` attribute exposes private fields in the Unity Inspector while maintaining proper encapsulation.
- **Default Values:** Default values provide reasonable starting points for testing.

### Ground Check Settings

```csharp
[Header("Ground Check")]        
[SerializeField] private Transform groundCheck;      // Position to check for ground
[SerializeField] private float groundDistance = 0.4f; // Distance to check for ground
[SerializeField] private LayerMask groundMask;        // Layer mask for ground detection
```

- **Transform Reference:** The `groundCheck` reference is used for positioning the ground detection sphere.
- **LayerMask:** The `groundMask` allows selective collision detection with specific layers, optimizing performance.

## Private Variables

```csharp
private Vector2 movementInput; // Player movement input 
private bool jumpInput;        // Player jump input
private Rigidbody rb;          // Rigidbody component
private bool isGrounded;       // Is the player grounded (used for jump control)
```

- **Input State:** `movementInput` and `jumpInput` store the current input state from the Input System.
- **Component Caching:** `rb` stores a reference to the Rigidbody component for improved performance.
- **State Tracking:** `isGrounded` keeps track of whether the character is touching the ground.

## Initialization

```csharp
private void Awake()
{
    rb = GetComponent<Rigidbody>();
}
```

- **Awake Method:** Called when the script instance is being loaded, before Start.
- **Component Caching:** GetComponent is called once during initialization to avoid performance overhead in Update methods.

## Input Handling

### Movement Input

```csharp
public void OnMove(InputValue value)
{
    movementInput = value.Get<Vector2>();
}
```

- **InputSystem Integration:** The `OnMove` method is called by Unity's Input System when the move action is triggered.
- **InputValue Parameter:** Contains the data associated with the input action (direction and magnitude).
- **Vector2 Extraction:** The `value.Get<Vector2>()` method extracts a 2D vector representing the movement direction and magnitude.

### Jump Input

```csharp
public void OnJump(InputValue value)
{
    // Only allow jumping when grounded
    if (isGrounded)
    {
        jumpInput = value.isPressed;

        if (jumpInput)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpInput = false; // Reset jump input after applying force
        }
    }
}
```

- **Conditional Logic:** Jump is only processed when the character is grounded.
- **isPressed Property:** Checks if the jump button is being pressed.
- **Physics Force:** `AddForce` applies a physics-based impulse to the Rigidbody.
- **ForceMode.Impulse:** Applies an instant force that ignores mass.

## Physics Updates

```csharp
private void FixedUpdate()
{
    // Check if the player is grounded
    CheckGrounded();
    
    // Move the player based on input
    HandleMovement();
}
```

- **FixedUpdate Method:** Called at a fixed interval independent of frame rate (typically 50 times per second), making it ideal for physics calculations.
- **Separation of Concerns:** Logic is divided into separate methods for grounding and movement.

### Ground Detection

```csharp
private void CheckGrounded()
{
    // Use a sphere cast to check if the player is grounded
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
}
```

- **Physics.CheckSphere:** Creates a sphere cast at the specified position to detect collisions.
- **Layer Mask Filtering:** Only checks for collision with objects on the specified ground layer.

### Movement Implementation

```csharp
private void HandleMovement()
{
    // Convert the 2D input to a 3D vector
    Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);

    // Only move if there is input
    if (moveDirection.magnitude > 0.1f)
    {
        // Calculate the target rotation based on the input direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move in the forward direction based on the current rotation
        rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
    }
}
```

- **Input Conversion:** Transforms 2D input (x,y) to 3D space (x,0,z).
- **Magnitude Check:** Only processes movement if the input magnitude exceeds a small threshold (0.1f) to avoid drift from tiny inputs.
- **Quaternion.LookRotation:** Creates a rotation that points an object's forward vector in the direction of the movement.
- **Quaternion.Slerp:** Performs spherical linear interpolation between rotations for smooth turning.
- **Time.deltaTime:** Used for rotation to ensure consistent movement regardless of frame rate.
- **rb.MovePosition:** Physics-based movement that respects collisions and maintains proper physics interactions.
- **Time.fixedDeltaTime:** Ensures consistent movement in the physics system regardless of frame rate.

## Cross-Platform Considerations

- **Input System:** Unity's new Input System provides built-in support for multiple platforms including keyboard/mouse, touch, and gamepads.
- **Frame-Rate Independence:** All movement calculations use Time.deltaTime or Time.fixedDeltaTime to ensure consistent behavior across devices with different performance levels.
- **Physics Settings:** The script uses Unity's physics system, which may need platform-specific tuning for optimal performance.

## Integration with Input System

This script is designed to work with Unity's Input System package and expects an Input Action Asset with the following actions:

1. A "Move" action that returns a Vector2 value
2. A "Jump" action that returns a button press

The asset should be setup in the following manner:
- Action Map: "Player"
- Actions:
  - "Move" (Vector2 value)
  - "Jump" (Button)

### Unity 6 Input System Integration

In Unity 6, there are multiple approaches to connect the input actions to the character controller:

#### Method 1: Send Messages Approach (Recommended for Unity 6)

The most reliable method in Unity 6 is using the "Send Messages" behavior:

1. Create and configure an Input Actions asset with Move and Jump actions
2. Add a PlayerInput component to your character GameObject
3. Assign your Input Actions asset to the PlayerInput component
4. Set the "Behavior" dropdown to "Send Messages"
5. Ensure your PlayerCharacterController script has the matching method names:
   - `OnMove(InputValue value)`
   - `OnJump(InputValue value)`

This approach uses Unity's message system to automatically route input events to your methods without manual binding.

#### Method 2: Direct Code Subscription

For more explicit control, you can subscribe to input actions directly in code:

```csharp
private PlayerInput playerInput;
private InputAction moveAction;
private InputAction jumpAction;

private void Awake()
{
    rb = GetComponent<Rigidbody>();
    
    // Get the PlayerInput component
    playerInput = GetComponent<PlayerInput>();
    if (playerInput != null)
    {
        // Cache the actions
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        
        // Subscribe to the action events
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        jumpAction.performed += OnJumpPerformed;
    }
}

// Add unsubscribe logic in OnDestroy to prevent memory leaks
```

## Rigidbody Configuration Requirements

For optimal functionality, the Rigidbody component on the player character should be configured with:

- **Mass:** ~1.0 (adjust based on game feel)
- **Drag:** ~0.5 (prevents excessive sliding)
- **Angular Drag:** ~0.05 (default)
- **Use Gravity:** Enabled
- **Is Kinematic:** Disabled
- **Interpolate:** Interpolate (smoother movement)
- **Collision Detection:** Continuous (prevents tunneling through colliders)
- **Constraints:** Freeze Rotation (X, Y, Z) to prevent tipping

## Required Collider Setup

The player character requires:

1. A primary Collider component (typically CapsuleCollider) on the character GameObject
2. A small empty GameObject as a child positioned at the character's feet with the Transform reference set as `groundCheck`

## Integration with Animation System

While not implemented in this basic version, the character controller is designed to be extended with animation support:

```csharp
// Example animation integration (not included in current implementation)
private Animator animator;

private void Awake()
{
    rb = GetComponent<Rigidbody>();
    animator = GetComponent<Animator>();
}

private void Update()
{
    // Update animator parameters
    if (animator != null)
    {
        animator.SetFloat("Speed", movementInput.magnitude);
        animator.SetBool("IsGrounded", isGrounded);
    }
}
```

## Performance Considerations

- **Component Caching:** Rigidbody reference is cached in Awake to avoid GetComponent calls during gameplay
- **Physics Optimization:** Ground check uses a layer mask to limit collision checks
- **Input Efficiency:** Input values are only processed when they change (event-based)
- **Conditional Processing:** Movement calculations only occur when there is significant input

## Potential Extensions

The character controller is designed as a foundation that can be extended with:

1. **State Machine:** For more complex character states (idle, walk, run, jump, fall)
2. **Advanced Movement:** Double jump, wall jump, dashing, or sliding
3. **Combat Integration:** Attack animations and movement restrictions during attacks
4. **Network Synchronization:** For multiplayer functionality
5. **Footstep System:** Audio and particle effects based on surface type

## Troubleshooting Common Issues

1. **Character not moving:** Ensure Input System is properly configured and the character has a non-kinematic Rigidbody
2. **Character falling through floor:** Check that collision layers are properly set up and collision detection is set to Continuous
3. **Character unable to jump:** Verify the ground check is positioned correctly and the ground layer is included in the ground mask
4. **Jerky movement:** Adjust interpolation settings on the Rigidbody and ensure frame rate is stable

### Unity 6 Specific Troubleshooting

5. **Input not working in Unity 6:** 
   - Ensure the PlayerInput component's "Behavior" is set to "Send Messages"
   - Verify that method names in your code match exactly (case-sensitive)
   - Check the Action Map name in the PlayerInput component matches your Input Actions asset
   - Try restarting Unity if input bindings don't appear in the Inspector

6. **Cannot find functions in dropdown:**
   - Unity 6's Input System UI can sometimes fail to show script methods in dropdowns
   - Switch to "Send Messages" behavior instead of trying to manually wire up events
   - If using a namespace (like `BetterWYD.Development`), try temporarily removing it for testing

7. **Input delay or unresponsiveness:**
   - Check the "Default Value" settings in your Input Actions asset
   - Adjust the "Interactions" settings for your actions (e.g., try "Press" instead of "Tap")

## Implementation Checklist

For Unity 6 implementations, follow this checklist:

1. **Script Setup**
   - [ ] PlayerCharacterController script added to character GameObject
   - [ ] `OnMove` and `OnJump` methods correctly implemented
   - [ ] All required variables and references set

2. **Input System Setup**
   - [ ] Input Actions asset created with proper Move and Jump actions
   - [ ] PlayerInput component added to character GameObject
   - [ ] Behavior set to "Send Messages"
   - [ ] Default action map correctly set

3. **Physics Setup**
   - [ ] Rigidbody properly configured (see Rigidbody Configuration above)
   - [ ] Ground layer created and assigned to ground objects
   - [ ] GroundCheck GameObject positioned correctly at character's feet
   - [ ] Ground mask properly set to include only ground layers

4. **Testing**
   - [ ] Character rotates to face movement direction
   - [ ] Character moves at appropriate speed
   - [ ] Character only jumps when grounded
   - [ ] Collisions are properly detected