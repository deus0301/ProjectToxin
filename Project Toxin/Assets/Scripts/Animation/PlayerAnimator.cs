using UnityEngine;

namespace Toxin.Animation
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private float animationDampTime = 0.1f;

        private Animator animator;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int JumpHash = Animator.StringToHash("Jump");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
        private static readonly int TalkHash = Animator.StringToHash("Talk");

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void UpdateSpeed(float speed)
        {
            animator.SetFloat(SpeedHash, speed, animationDampTime, Time.deltaTime);
        }

        public void TriggerJump()
        {
            animator.SetTrigger(JumpHash);
        }

        public void TriggerTalk()
        {
            animator.SetTrigger(TalkHash);
        }

        public void UpdateGrounded(bool isGrounded)
        {
            animator.SetBool(IsGroundedHash, isGrounded);
        }

        public void SetAnimationSpeed(float speed)
        {
            animator.speed = speed;
        }

        public void PlayAnimation(string stateName, int layer = 0)
        {
            animator.Play(stateName, layer);
        }

        public void CrossFade(string stateName, float duration = 0.2f, int layer = 0)
        {
            animator.CrossFade(stateName, duration, layer);
        }
    }
}