using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Application.Scripts.Views.Menues
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject panelPausa;
        public GameObject btnPausa;
        public GameObject panelConfirmExit;
        public Text pauseTitle, continueTitle, restartTitle, quitTitle, confirmExitText;
    
        void Start()
        {
            pauseTitle.text = LanguageManager.instance.GetText("pause");
            continueTitle.text = LanguageManager.instance.GetText("continue");
            restartTitle.text = LanguageManager.instance.GetText("restart");
            quitTitle.text = LanguageManager.instance.GetText("quit");
            confirmExitText.text = LanguageManager.instance.GetText("confirmexit");
        }

        public void PauseGame()
        {
            panelPausa.SetActive(true);
            btnPausa.SetActive(false);

            Time.timeScale = 0f;
        }

        public void Continue()
        {
            panelPausa.SetActive(false);
            btnPausa.SetActive(true);


            Time.timeScale = 1f;
        }

        public void Exit()
        {
            panelConfirmExit.SetActive(true);
        }

        public void CancelExit()
        {
            panelConfirmExit.SetActive(false);
        }

        public void ConfirmExit()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(Constants.Scenes.MAIN_MENU);
        }

        public void Restart()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
