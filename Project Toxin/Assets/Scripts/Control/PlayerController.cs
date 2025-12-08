using UnityEngine;
using Toxin.Core;
using Toxin.Movement;
using Toxin.Animation;

namespace Toxin.Control
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerAnimator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;

        private Mover mover;
        private PlayerInput input;
        private PlayerAnimator playerAnimator;
        private FrameInput frameInput;

        void Awake()
        {
            mover = GetComponent<Mover>();
            input = GetComponent<PlayerInput>();
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            GatherInput();
            HandleMovement();
            HandleJump();
            UpdateAnimations();
        }

        private void GatherInput()
        {
            frameInput = input.frame;
        }

        private void HandleMovement()
        {
            mover.Move(frameInput.Move, frameInput.Sprint, cameraTransform);
        }

        private void HandleJump()
        {
            if (frameInput.Jump && mover.IsGrounded)
            {
                mover.Jump();
                playerAnimator.TriggerJump();
            }
        }

        private void UpdateAnimations()
        {
            playerAnimator.UpdateSpeed(mover.CurrentSpeed);
            playerAnimator.UpdateGrounded(mover.IsGrounded);
        }

        public bool IsGrounded() => mover.IsGrounded;
        public float GetCurrentSpeed() => mover.CurrentSpeed;
        public Vector3 GetVelocity() => mover.Velocity;

        public void SetControlEnabled(bool enabled)
        {
            print("PlayerController set to: " + enabled);
            this.enabled = enabled;
            mover.SetEnabled(enabled);
        }
    }
}