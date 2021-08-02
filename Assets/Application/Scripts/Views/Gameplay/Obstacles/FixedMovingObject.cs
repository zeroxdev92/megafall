using Application.Scripts.Views.Managers;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay.Obstacles
{
	public class FixedMovingObject : MonoBehaviour 
	{
		// Update is called once per frame
		public virtual void Update () 
		{
			transform.position += ManagerGame.instancia.GetVelocidad() * Vector3.up * Time.deltaTime;
		}
	}
}
