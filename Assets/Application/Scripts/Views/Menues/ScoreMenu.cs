using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using Application.Scripts.Views.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Scripts.Views.Menues
{
    public class ScoreMenu : MonoBehaviour
    {

        public SceneFader sceneFader;
        public Text scoreText;
        public Image scoreBG;
        public Sprite scoreESPBG, highscoreBG, highscoreESPBG;
        public Text coinsText;

        void Start()
        {
            AudioManager.instance.StopSound(Constants.Audio.GAME_THEME);
            AudioManager.instance.PlaySound(Constants.Audio.MENU_THEME, true);

            int score = GameSettings.GetScore();
            int highScore = GameSettings.GetMaxScore();

            scoreText.text = score.ToString();

            string lang = PlayerPrefs.GetString(Constants.Language.LANGUAGE);

            if (lang.Equals(Constants.Language.LANG_ESP_KEY))
                scoreBG.sprite = scoreESPBG;

            if (score > highScore)
            {
                GameSettings.SetMaxScore(score);

                if (lang.Equals(Constants.Language.LANG_ESP_KEY))
                {
                    scoreBG.sprite = highscoreESPBG;
                }
                else
                {
                    scoreBG.sprite = highscoreBG;
                }

            }

            coinsText.text = GameSettings.GetGameCoins().ToString();

        }


        public void Restart()
        {
            sceneFader.FadeTo(Constants.Scenes.GAMEPLAY);
        }

        public void Quit()
        {
            sceneFader.FadeTo(Constants.Scenes.MAIN_MENU);
        }
    }
}
