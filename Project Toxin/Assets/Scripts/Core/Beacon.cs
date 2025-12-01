using UnityEngine;
using Toxin.Saving;
using Unity.VisualScripting;
using TMPro;

namespace Toxin.Core
{
    public class Beacon : MonoBehaviour
    {
        [SerializeField] private float saveRadius;
        [SerializeField] private LayerMask beaconLayer;
        [SerializeField] private TextMeshProUGUI savePrompt;
        [SerializeField] private TextMeshProUGUI loadPrompt;

        private PlayerInput input;
        private bool nearBeacon;
        private FrameInput _frameInput;

        void Awake()
        {
            input = GetComponent<PlayerInput>();
            loadPrompt.enabled = false;
        }
        // Update is called once per frame
        void Update()
        {
            GatherInput();
            Save();
            Load();
        }

        private void Load()
        {
            if (_frameInput.Load)
            {
                SavingSystem.Instance.Load(SavingSystem.defaultSaveFile);
            }
        }

        private void Save()
        {
            nearBeacon = Physics.CheckSphere(transform.position, saveRadius, beaconLayer);
            if (nearBeacon)
            {
                EnableSave();
            }
            else
            {
                savePrompt.enabled = false;
            }
        }

        void GatherInput()
        {
            _frameInput = input.frame;
        }
        void EnableSave()
        {
            savePrompt.enabled = true;
            if (_frameInput.Save)
            {
                SavingSystem.Instance.Save(SavingSystem.defaultSaveFile);
                loadPrompt.enabled = true;
            }
        }
    }
}

