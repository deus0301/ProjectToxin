using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Toxin.Control;
using Toxin.Saving;

namespace Toxin.SceneManagement
{
    public class LabDoor : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int loadIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        private float loadTime = 3f;
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player")) StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            yield return StartCoroutine(Fader.Instance.FadeOut(loadTime/2));

            SavingSystem.Instance.Save(SavingSystem.defaultSaveFile);

            yield return SceneManager.LoadSceneAsync(loadIndex);

            SavingSystem.Instance.Load(SavingSystem.defaultSaveFile);
            
            LabDoor otherDoor = GetOther();
            print("Using Door: " + otherDoor.name);
            UpdatePlayer(otherDoor);

            SavingSystem.Instance.Save(SavingSystem.defaultSaveFile);

            yield return StartCoroutine(Fader.Instance.FadeIn(loadTime/2));
            
            Destroy(gameObject);

        }
        
        LabDoor GetOther()
        {
            LabDoor[] doors = FindObjectsByType<LabDoor>(FindObjectsSortMode.None);
            foreach(LabDoor door in doors)
            {
                if(door.gameObject == gameObject || door.destination != destination) continue;

                return door;
            }
            return null;
        }

        void UpdatePlayer(LabDoor door)
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            var controller = player.GetComponent<CharacterController>();
            var playerController = player.GetComponent<PlayerController>();
            
            if (controller != null)
                controller.enabled = false;
            
            if (playerController != null)
                playerController.enabled = false;

            player.transform.position = door.spawnPoint.position;
            player.transform.rotation = door.spawnPoint.rotation;

            if (controller != null)
                controller.enabled = true;
            
            if (playerController != null)
                playerController.enabled = true;
        }
    }    
}

