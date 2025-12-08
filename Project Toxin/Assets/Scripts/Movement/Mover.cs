using Toxin.Control;
using Toxin.Saving;
using UnityEngine;

namespace Toxin.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour, ISaveable
    {
        [Header("Movement Settings")]
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] private float turnSmoothTime = 0.1f;

        [Header("Jumping")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 2f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float checkRadius = 0.2f;

        private CharacterController controller;
        private Vector3 velocity;
        private float turnSmoothVelocity;
        private bool isGrounded;

        // Public properties
        public bool IsGrounded => isGrounded;
        public float CurrentSpeed => new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        public Vector3 Velocity => controller.velocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            CheckGround();
            ApplyGravity();
        }

        public void Move(Vector2 inputDirection, bool isSprinting, Transform cameraTransform)
        {
            if (inputDirection.magnitude < 0.1f) return;

            Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;
            
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float currentSpeed = isSprinting ? baseSpeed * sprintMultiplier : baseSpeed;

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        public void Jump()
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        private void ApplyGravity()
        {
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void CheckGround()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
            }
        }
        public object CaptureState()
        {
            return new SerializableVector3(transform.position); 
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                Debug.LogWarning($"[SaveableEntity] State is null for {name}");
                return;
            }

            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
                playerController.enabled = false;

            SerializableVector3 position = (SerializableVector3)state;
            transform.position = position.ToVector3();

            if (playerController != null)
                playerController.enabled = true;
        }
    }
}