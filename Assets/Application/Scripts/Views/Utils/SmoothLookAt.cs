﻿using UnityEngine;

namespace Application.Scripts.Views.Utils
{
	[AddComponentMenu("Camera-Control/Smooth Look At")]

	public class SmoothLookAt : MonoBehaviour
	{
		public Transform target;
		public float damping = 6.0f;
		public bool smooth = true;

	
		// Update is called once per frame
		void LateUpdate () 
		{
			if (target) {
				if (smooth)
				{
					// Look at and dampen the rotation
					var rotation = Quaternion.LookRotation(target.position - transform.position);
					rotation.y = 0;
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
				}
				else
				{
					// Just lookat
					transform.LookAt(target);
				}
			}
		}
	}
}
