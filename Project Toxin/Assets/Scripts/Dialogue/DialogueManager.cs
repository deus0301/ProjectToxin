using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

namespace Toxin.Dialogue
{
    [RequireComponent(typeof(DialogueInput))]
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI nameText;

        private DialogueInput dialogueInput;
        private UIFrameInput _frameInput;

        void Awake()
        {
            dialogueInput = GetComponent<DialogueInput>();
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            gameObject.SetActive(false);
        }

        void Update()
        {
            GatherInput();
        }

        public IEnumerator StartDialogue(List<string> lines, string name)
        {
            nameText.text = name;
            gameObject.SetActive(true);
            foreach (var line in lines)
            {
                Debug.Log(line);
                yield return DisplayLine(line);
            }
            yield return null;
            gameObject.SetActive(false);
        }

        void GatherInput()
        {
            _frameInput = dialogueInput.frame;
        }

        IEnumerator DisplayLine(string line)
        {
            for (int i = 0; i <= line.Length; i++)
            {
                dialogueText.text = line.Substring(0, i);
                yield return new WaitForSeconds(0.05f);
                if (InputReceived())
                {
                    dialogueText.text = line;
                    i = line.Length; 
                    yield return null; 
                    break;
                }
            }
            yield return new WaitUntil(() => InputReceived());
        }

        private bool InputReceived()
        {
            return _frameInput.Next;
        }
    }
}