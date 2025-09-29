using UnityEngine;
using Toxin.Core;
using UnityEngine.Scripting.APIUpdating;

namespace Toxin.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform camera;

        private CharacterController controller;
        private PlayerInput input;
        private FrameInput _frameInput;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInput>();
        }

        void FixedUpdate()
        {
            Move();
        }

        void Update()
        {
            GatherInput();
        }

        void GatherInput()
        {
            _frameInput = input.frame;
        }
        void Move()
        {
            float moveX = _frameInput.Move.x;
            float moveZ = _frameInput.Move.y;

            Vector3 motion = Quaternion.Euler(0, camera.eulerAngles.y, 0) * new Vector3(moveX, 0, moveZ).normalized;

            controller.Move(motion * speed * Time.fixedDeltaTime);
        }
    }
}
