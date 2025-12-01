#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace Toxin.Saving
{
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueID = "";

        public string GetUniqueIdentifier()
        {
            return uniqueID;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state; 
        }

        public void RestoreState(object state)
        {
            print("Restoring state for " + GetUniqueIdentifier());

            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                string type = saveable.GetType().ToString();
                if(stateDict.ContainsKey(type)) saveable.RestoreState(stateDict[type]);
            }
        }

#if UNITY_EDITOR
        void Update()
        {
            if (Application.IsPlaying(gameObject)) return;

            if (PrefabUtility.IsPartOfPrefabAsset(gameObject)) return;

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.IsPartOfPrefabContents(gameObject)) return;
            
            GenerateUniqueID();
        }

        private void GenerateUniqueID()
        {
            SerializedObject s_object = new SerializedObject(this);
            SerializedProperty property = s_object.FindProperty("uniqueID");

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                s_object.ApplyModifiedProperties();
            }
        }
#endif
    }
}