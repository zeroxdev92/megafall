using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Scripts.Views.Menues
{
    public class AudioMenu : MonoBehaviour {

        public Sprite musicOn, musicOff, soundOn, soundOff;
        public GameObject musicButton, soundButton;

        private Image musicImage, soundImage;


        void Start()
        {
            musicImage = musicButton.GetComponent<Image>();
            soundImage = soundButton.GetComponent<Image>();

            if (GameSettings.GetMusic() == 1)
            {
                musicImage.sprite = musicOn;
            }
            else
            {
                musicImage.sprite = musicOff;
            }

            if (GameSettings.GetSound() == 1)
            {
                soundImage.sprite = soundOn;
            }
            else
            {
                soundImage.sprite = soundOff;
            }
        }

        public void ToogleSound()
        {
            if (GameSettings.GetSound() == 1)
            {
                soundImage.sprite = soundOff;
                GameSettings.SetSound(0);

            }
            else
            {
                soundImage.sprite = soundOn;
                GameSettings.SetSound(1);
            }
        }

        public void ToogleMusic()
        {
            if (GameSettings.GetMusic() == 1)
            {
                musicImage.sprite = musicOff;
                GameSettings.SetMusic(0);
                AudioManager.instance.AdjustThemeVolume(0);
            }
            else
            {
                musicImage.sprite = musicOn;
                GameSettings.SetMusic(1);
                AudioManager.instance.ResetVolume();
            }
        }
    }
}
