using UnityEngine;
using UnityEngine.InputSystem;

namespace Toxin.Dialogue
{
    public class DialogueInput : MonoBehaviour
    {
        private UIInputActions inputActions;

        public UIFrameInput frame;

        private InputAction _next;

        void Awake()
        {
            inputActions = new UIInputActions();    

            _next = inputActions.UI.Next;        
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

        private UIFrameInput GatherInput()
        {
            return new UIFrameInput
            {
                Next = _next.WasPressedThisFrame(),
            };
        }
    }
    public struct UIFrameInput
    {
        public bool Next;
    }
}
