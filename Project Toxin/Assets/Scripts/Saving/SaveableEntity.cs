#if UNITY_EDITOR
using System;
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
        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

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
        [SerializeField] private bool useCustomID = false;
        [SerializeField] private string customID = "";

        private void Reset()
        {
            // Called when component is first added
            if (!useCustomID)
            {
                GenerateUniqueID();
            }
        }

        private void OnValidate()
        {
            // Called when loaded/modified
            if (!Application.isPlaying)
            {
                EditorApplication.delayCall += () =>
                {
                    if (this != null)
                    {
                        if (useCustomID && !string.IsNullOrEmpty(customID))
                        {
                            SetCustomID();
                        }
                        else if (!useCustomID)
                        {
                            GenerateUniqueID();
                        }
                    }
                };
            }
        }

        void Update()
        {
            // Backup check during editor time
            if (Application.IsPlaying(gameObject)) return;
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject)) return;

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.IsPartOfPrefabContents(gameObject)) return;
            
            if (!useCustomID)
            {
                GenerateUniqueID();
            }
        }

        private void SetCustomID()
        {
            SerializedObject s_object = new SerializedObject(this);
            SerializedProperty property = s_object.FindProperty("uniqueID");

            if (property.stringValue != customID)
            {
                property.stringValue = customID;
                s_object.ApplyModifiedProperties();
            }
            
            if (globalLookup == null)
            {
                globalLookup = new Dictionary<string, SaveableEntity>();
            }
            globalLookup[customID] = this;
        }

        private void GenerateUniqueID()
        {
            if (globalLookup == null)
            {
                globalLookup = new Dictionary<string, SaveableEntity>();
            }
            
            SerializedObject s_object = new SerializedObject(this);
            SerializedProperty property = s_object.FindProperty("uniqueID");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                s_object.ApplyModifiedProperties();
            }
            globalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string stringValue)
        {
            if(!globalLookup.ContainsKey(stringValue)) return true;

            if(globalLookup[stringValue] == this) return true;

            //------------EdgeCases------------
            //Handling case where an object has been destroyed but its ID is still in the lookup
            if(globalLookup[stringValue] == null)
            {
                globalLookup.Remove(stringValue);
                return true;
            }

            //Handling case where the object with this ID has been destroyed
            if(globalLookup[stringValue].GetUniqueIdentifier() != stringValue)
            {
                globalLookup.Remove(stringValue);
                return true;
            }

            //If we reach here, then the ID is not unique
            return false;
        }
#endif
    }
}