using DG.Tweening;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay
{
	public class TeddyBearAnimation : MonoBehaviour {

		void Start () {

			Sequence mySequence = DOTween.Sequence();

			mySequence.Append(transform.DOMoveY(-16f, 1.5f).SetEase(Ease.Linear));
			mySequence.Join(transform.DOLocalRotate(new Vector3(5f, 190f, 162f), 1.5f).SetEase(Ease.Linear));
        
		}
	
	
	}
}
