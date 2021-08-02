using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Scripts.Views.Menues
{
    public class LanguageMenu : MonoBehaviour {

        public Button spanishButton, englishButton;
        public Text closeText;
    
        void Start()
        {
            closeText.text = LanguageManager.instance.GetText("close");

            string langKey = PlayerPrefs.GetString(Constants.Language.LANGUAGE);

            if (langKey.Equals(Constants.Language.LANG_ESP_KEY))
            {
                spanishButton.interactable = false;
            }
            else
            {
                englishButton.interactable = false;
            }
        }

        public void ToogleLanguage(string langKey)
        {
            if (langKey.Equals(Constants.Language.LANG_ESP_KEY))
            {
                englishButton.interactable = true;
                spanishButton.interactable = false;
            }
            else
            {
                englishButton.interactable = false;
                spanishButton.interactable = true;
            }

            PlayerPrefs.SetString(Constants.Language.LANGUAGE, langKey);

            LanguageManager.instance.ChangeLanguage(langKey);

            closeText.text = LanguageManager.instance.GetText("close");
        }
    }
}
