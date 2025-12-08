using System.Collections;
using System.Collections.Generic;
using Toxin.Animation;
using Toxin.Control;
using Toxin.Interaction;
using UnityEngine;

namespace Toxin.Dialogue
{
    [RequireComponent(typeof(Collider))]
    public class DialogueActivator : MonoBehaviour, IInteractable
    {
        [SerializeField]
        List<string> dialogueLines = new List<string>()
        {
            "Welcome to Project Toxin.",
            "This is a prototype for dialogue system.",
            "Good luck exploring!"
        };

        [SerializeField] private string characterName = "";

        [SerializeField] private Animator animator;
        private static readonly int TalkHash = Animator.StringToHash("Talk");

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                StartCoroutine(WaitForDialogueEnd(player));
            }
        }

        private IEnumerator WaitForDialogueEnd(PlayerController player)
        {
            print("Starting Dialogue...");

            animator.SetTrigger(TalkHash);
            player.GetComponent<PlayerAnimator>().TriggerTalk();
            player.SetControlEnabled(false);
            yield return Interact();
            player.SetControlEnabled(true);
            player.GetComponent<PlayerAnimator>().TriggerTalk();
            animator.SetTrigger(TalkHash);
        }

        public IEnumerator Interact()
        {
            yield return DialogueManager.Instance.StartDialogue(dialogueLines, characterName);
        }
    }
}