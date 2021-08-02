using System.Collections;
using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using Application.Scripts.Views.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Application.Scripts.Views.Menues
{
    public class MainMenu : MonoBehaviour
    {
        public SceneFader sceneFader;
        public Animator playerAnimator;
        public CanvasGroup UIGroup;
        public Canvas settingsCanvas;


        private bool animationEnded = false;


        void Start()
        {
            AudioManager.instance.StopSound(Constants.Audio.GAME_THEME);
            AudioManager.instance.PlaySound(Constants.Audio.MENU_THEME, true, false);


            StartCoroutine(Intro());
        }

        void Update()
        {
            if (settingsCanvas.isActiveAndEnabled)
                return;

#if UNITY_EDITOR
            if (animationEnded && Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
            {
                Play();
            }
#endif

            if (animationEnded)
            {
                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    if (EventSystem.current.currentSelectedGameObject != null)
                        return;

                    Play();
                }
            }
        }

        IEnumerator Intro()
        {
            yield return new WaitForSeconds(4.10f);

            UIGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);
            animationEnded = true;
        }

        private void Play()
        {
            StartCoroutine(PlaySequence());
        }

        private IEnumerator PlaySequence()
        {
            playerAnimator.SetTrigger("Jump");

            yield return new WaitForSeconds(0.4f);

            sceneFader.FadeTo(Constants.Scenes.GAMEPLAY);
        }

        public void Exit()
        {
            Debug.Log("Quit Game");
            UnityEngine.Application.Quit();
        }
    }
}