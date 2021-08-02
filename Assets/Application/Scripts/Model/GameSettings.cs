using UnityEngine;

namespace Application.Scripts.Model
{
    public static class GameSettings
    {
        public static void SetTutorial(bool show)
        {
            PlayerPrefs.SetInt("Tutorial", show ? 1 : 0);
        }

        public static bool GetTutorial()
        {
            return PlayerPrefs.GetInt("Tutorial", 1) == 1;
        }

        public static void SetScore(int valor)
        {
            PlayerPrefs.SetInt("score", valor);
        }

        public static int GetScore()
        {
            return PlayerPrefs.GetInt("score");
        }

        public static void SetMaxScore(int valor)
        {
            PlayerPrefs.SetInt("maxScore", valor);
        }

        public static int GetMaxScore()
        {
            return PlayerPrefs.GetInt("maxScore");
        }

        public static int GetSound()
        {
            return PlayerPrefs.GetInt("sound") == 1 ? 0 : 1;
        }

        public static int GetMusic()
        {
            return PlayerPrefs.GetInt("music") == 1
                ? 0
                : 1;
        }

        public static void SetSound(int valor)
        {
            PlayerPrefs.SetInt("sound", valor == 1 ? 0 : 1);
        }

        public static void SetMusic(int valor)
        {
            PlayerPrefs.SetInt("music", valor == 1 ? 0 : 1);
        }

        public static int GetCoins()
        {
            return PlayerPrefs.GetInt("coins");
        }

        public static void SetCoins(int valor)
        {
            PlayerPrefs.SetInt("coins", valor);
        }

        public static int GetGameCoins()
        {
            return PlayerPrefs.GetInt("gameCoins");
        }

        public static void SetGameCoins(int valor)
        {
            PlayerPrefs.SetInt("gameCoins", valor);
        }
    }
}