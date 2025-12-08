using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using Toxin.SceneManagement;

namespace Toxin.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public static SavingSystem Instance;
        public const string defaultSaveFile = "save";
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            if(Instance == this)
            {
                StartCoroutine(LoadAtStart());
            }
        }

        private IEnumerator LoadAtStart()
        {            
            yield return Fader.Instance.FadeOut(0.1f);
            yield return LoadLastScene(defaultSaveFile);
            yield return Fader.Instance.FadeIn(1);
        }
        
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                int buildIndex = (int)state["lastSceneBuildIndex"];
                if(buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }       
            }
            RestoreState(state);
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile) ?? new Dictionary<string, object>();
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }
        
        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Loading " + path);

            if (!File.Exists(path))
            {
                print("No save file found at " + path);
                return new Dictionary<string, object>();
            }
            
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Length == 0)
            {
                print("Save file is empty, creating a new state dictionary.");
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }
        
        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity entity in FindObjectsByType(typeof(SaveableEntity), FindObjectsSortMode.None))
            {
                string id = entity.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    entity.RestoreState(state[id]);
                }
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity entity in FindObjectsByType(typeof(SaveableEntity), FindObjectsSortMode.None))
            {
                state[entity.GetUniqueIdentifier()] = entity.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }
        
        public string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

    }
}
