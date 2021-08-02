using Application.Scripts.Views.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Scripts.Views.Gameplay
{
    public class TutorialAnimation : MonoBehaviour {

        public Text tutorialTitleText, gotItText;

        public RectTransform verticalHand;
        public RectTransform verticalFace;
        public RectTransform horizontalHand;
        public RectTransform horizontalFace;
    

        // Use this for initialization
        void Start () {

            tutorialTitleText.text = LanguageManager.instance.GetText("tutorialtitle");
            gotItText.text = LanguageManager.instance.GetText("gotit");

            VerticalAnimation();
            HorizontalAnimation();

        }


        private void VerticalAnimation()
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(verticalHand.DOAnchorPosY(70f, 0.4f, false).SetLoops(2, LoopType.Yoyo));
            mySequence.Join(verticalFace.DOAnchorPosY(120f, 0.4f, false).SetDelay(0.1f).SetLoops(2, LoopType.Yoyo));
            mySequence.AppendInterval(0.2f);
            mySequence.Append(verticalHand.DOAnchorPosY(-70f, 0.4f, false).SetLoops(2, LoopType.Yoyo));
            mySequence.Join(verticalFace.DOAnchorPosY(-120f, 0.4f, false).SetDelay(0.1f).SetLoops(2, LoopType.Yoyo));

            mySequence.SetEase(Ease.Linear);
            mySequence.SetLoops(-1, LoopType.Restart);
        }

        private void HorizontalAnimation()
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(horizontalHand.DOAnchorPosX(40f, 0.4f, false).SetLoops(2, LoopType.Yoyo));
            mySequence.Join(horizontalFace.DOAnchorPosX(200f, 0.4f, false).SetDelay(0.1f).SetLoops(2, LoopType.Yoyo));
            mySequence.AppendInterval(0.2f);
            mySequence.Append(horizontalHand.DOAnchorPosX(-40f, 0.4f, false).SetLoops(2, LoopType.Yoyo));
            mySequence.Join(horizontalFace.DOAnchorPosX(20f, 0.4f, false).SetDelay(0.1f).SetLoops(2, LoopType.Yoyo));

            mySequence.SetEase(Ease.Linear);
            mySequence.SetLoops(-1, LoopType.Restart);
        }
    }
}
