using UnityEngine;
using UnityEngine.InputSystem;

namespace BetterWYD.Development
{
    /// <summary>
    /// Player Character Controller for the BetterWYD project.
    /// Handles movement, rotation, and basic character interactions.
    /// </summary>
    public class PlayerCharacterController : MonoBehaviour
    {
        /// <summary>
        /// Movement and rotation settings for the character.
        /// </summary>
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f; // Units per second
        [SerializeField] private float rotationSpeed = 10f; // Degrees per second
        [SerializeField] private float jumpForce = 5f; // Force applied when jumping

        /// <summary>
        /// Ground detection settings.
        /// </summary>
        [Header("Ground Check")]        
        [SerializeField] private Transform groundCheck; // Position to check for ground
        [SerializeField] private float groundDistance = 0.4f; // Distance to check for ground
        [SerializeField] private LayerMask groundMask; // Layer mask for ground detection

        // Private member variables
        private Vector2 movementInput; // Player movement input 
        private bool jumpInput; // Player jump input
        private Rigidbody rb; // Rigidbody component
        private bool isGrounded; // Is the player grounded (used for jump control)

        /// <summary>
        /// Initializes the Rigidbody component.
        /// </summary>
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Handles movement input from the Input System.
        /// </summary>
        /// <param name="value">Input value containing movement vector</param>
        public void OnMove(InputValue value)
        {
            // Store the movement input value from the Input System
            movementInput = value.Get<Vector2>();
        }

        /// <summary>
        /// Handles jump input from the Input System.
        /// </summary>
        /// <param name="value">Input value containing jump button state</param>
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

        /// <summary>
        /// Fixed update is used for physics calculations.
        /// </summary>
        private void FixedUpdate()
        {
            // Check if the player is grounded
            CheckGrounded();
            
            // Move the player based on input
            HandleMovement();
        }

        /// <summary>
        /// Checks if the player is touching the ground.
        /// </summary>
        private void CheckGrounded()
        {
            // Use a sphere cast to check if the player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        /// <summary>
        /// Handles player movement and rotation based on input.
        /// </summary>
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
    }
}

