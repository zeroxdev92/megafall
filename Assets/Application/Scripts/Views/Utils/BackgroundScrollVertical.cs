using UnityEngine;

namespace Application.Scripts.Views.Utils
{
	[RequireComponent(typeof(Renderer))]
	public class BackgroundScrollVertical : MonoBehaviour {

		public float speed = 0;

		private Renderer rend;

		void Start()
		{
			rend = GetComponent<Renderer>();
		}

		void Update () 
		{
			rend.material.mainTextureOffset = new Vector2(0,(rend.material.mainTextureOffset.y + speed * Time.deltaTime));
		}
	}
}
