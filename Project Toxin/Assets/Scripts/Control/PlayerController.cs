using UnityEngine;
using Toxin.Core;
using UnityEngine.Scripting.APIUpdating;

namespace Toxin.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] float turnSmoothTime = 0.01f;

        [Header("Jumping")]
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 2f;

        [Header("Assign in Inspector")]
        [SerializeField] private Transform camera;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float checkSize = 0.2f;

        private CharacterController controller;
        private PlayerInput input;
        private FrameInput _frameInput;
        private Animator animator;
        private bool isGrounded;
        private Vector3 velocity;

        private float baseSpeed;
        float turnSmoothVelocity;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
            baseSpeed = speed;
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            GatherInput();
            Move();
            GroundCheck();
        }

        void GroundCheck()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, checkSize, groundLayer);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        void GatherInput()
        {
            _frameInput = input.frame;
        }
        void Move()
        {

            float moveX = _frameInput.Move.x;
            float moveZ = _frameInput.Move.y;

            Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;

            Vector3 flatVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
            float animSpeed = flatVelocity.magnitude;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                float currentSpeed = _frameInput.Sprint ? baseSpeed * sprintMultiplier : baseSpeed;

                controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

                flatVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
                animSpeed = flatVelocity.magnitude;
            }

            if (isGrounded && _frameInput.Jump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("Jump");
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);


            animator.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);
        }

        public bool IsGrounded() => isGrounded;
    }
}
