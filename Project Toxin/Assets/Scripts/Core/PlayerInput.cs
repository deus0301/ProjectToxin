using UnityEngine;
using UnityEngine.InputSystem;

namespace Toxin.Core
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputActions inputActions;

        private InputAction _move;
        private InputAction _jump;
        private InputAction _sprint;

        public FrameInput frame;

        void Awake()
        {
            inputActions = new PlayerInputActions();

            _move = inputActions.Player.Move;
            _jump = inputActions.Player.Jump;
            _sprint = inputActions.Player.Sprint;
            
        }
        
        void Update()
        {
            frame = GatherInput();
        }

        void OnEnable()
        {
            inputActions.Enable();
        }

        void OnDisable()
        {
            inputActions.Disable();
        }

        private FrameInput GatherInput()
        {
            return new FrameInput
            {
                Move = _move.ReadValue<Vector2>(),
                Jump = _jump.WasPressedThisFrame(),
                Sprint = _sprint.IsPressed(),
            };
        }
    }
    public struct FrameInput
    {
        public Vector2 Move;
        public bool Jump;
        public bool Sprint;
    }
}
