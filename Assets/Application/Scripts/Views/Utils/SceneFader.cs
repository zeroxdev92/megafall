using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Application.Scripts.Views.Utils
{
    public class SceneFader : MonoBehaviour {

    
        public Image fader;
        public float fadeInDuration = 1f;
        public float fadeOutDuration = 1f;
        public AnimationCurve curve;
        public bool playOnStart = true;

        private Color startColor;

        void Start()
        {
            if (playOnStart)
            {
                StartCoroutine(FadeIn());
            }

            startColor = fader.color;
        }

        public void FadeTo(string scene)
        {
            StartCoroutine(FadeOut(scene));
        }

        public void FadeTo(int buildIndex)
        {
            StartCoroutine(FadeOut(buildIndex));
        }

        IEnumerator FadeIn()
        {
            float t = fadeInDuration;

            while (t > 0)
            {
                t -= Time.deltaTime;

                float a = curve.Evaluate(t / fadeInDuration);
                fader.color = new Color(startColor.r, startColor.g, startColor.b, a);
                yield return 0;
            }
        }

        IEnumerator FadeOut(string scene)
        {
            float t = 0f;

            while (t < fadeOutDuration)
            {
                t += Time.deltaTime;

                float a = curve.Evaluate(t / fadeOutDuration);
                fader.color = new Color(startColor.r, startColor.g, startColor.b, a);
                yield return 0;
            }

            SceneManager.LoadScene(scene);
        }

        IEnumerator FadeOut(int buildIndex)
        {
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime;

                float a = curve.Evaluate(t);
                fader.color = new Color(startColor.r, startColor.g, startColor.b, a);
                yield return 0;
            }

            SceneManager.LoadScene(buildIndex);
        }
    }
}
