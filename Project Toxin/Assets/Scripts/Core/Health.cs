using System.Collections;
using UnityEngine;
namespace Toxin.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        bool isDead;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (health <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(float damage, float duration)
        {
            if (isDead) return;
            
        }

        void Die()
        {
            if (animator == null || isDead) return;

            //animator.SetTrigger("Die");
            isDead = true;
            StartCoroutine(DeathRoutine());
        }

        IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(3f);
            print("Lmao mrityu");
        }

        public bool IsDead() => isDead;
    }
}
