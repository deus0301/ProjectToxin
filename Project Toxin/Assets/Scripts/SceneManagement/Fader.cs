using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Toxin.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        public static Fader Instance;
        private CanvasGroup loadingScreen;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                loadingScreen = GetComponent<CanvasGroup>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while(loadingScreen.alpha > 0)
            {
                loadingScreen.alpha -= Time.deltaTime/time;
                yield return null;
            }
            loadingScreen.alpha = 0;
        }
        public IEnumerator FadeOut(float time)
        {
            while(loadingScreen.alpha < 1)
            {
                loadingScreen.alpha += Time.deltaTime/time;
                yield return null;
            }
            loadingScreen.alpha = 1;
        }
        
    }
}