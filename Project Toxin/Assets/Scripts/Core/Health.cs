using System.Collections;
using Toxin.Saving;
using UnityEngine;
namespace Toxin.Core
{
    public class Health : MonoBehaviour, ISaveable
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
            if (health <= 0 || transform.position.y <= -2)
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
            yield return new WaitForSeconds(1f);
            print("Lmao mrityu");
            yield return SavingSystem.Instance.LoadLastScene(SavingSystem.defaultSaveFile);

        }

        public bool IsDead() => isDead;

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
