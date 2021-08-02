using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using UnityEngine;

namespace Application.Scripts.Views.Utils
{
	public class SetupHorizontalPosition : MonoBehaviour {

		public HorizontalPosition horizontalPosition;

		// Use this for initialization
		void Start () {

			float x = SpawnLocationManager.instance.GetHorizontalPosition(horizontalPosition);

			transform.position = new Vector3(x, transform.position.y, transform.position.z);

		}
	
	
	}
}
