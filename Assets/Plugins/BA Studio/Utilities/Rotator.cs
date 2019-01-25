using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngerStudio.WeedProject
{
	public class Rotator : MonoBehaviour
	{
		public Vector3 axis = Vector3.up;

		public float rotationSpeed = 5f;

		[Tooltip("This could work with the issue Animator locking properties.")]
		public bool withAnimator;

		void Awake()
		{
			rCache = this.transform.rotation;	
		}

		void Update ()
		{
			if (!withAnimator) this.transform.Rotate(this.axis, this.rotationSpeed);
		}

		Quaternion rCache;

		void LateUpdate ()
		{
			if (withAnimator)
			{
				rCache *= Quaternion.Euler(axis * rotationSpeed);
				this.transform.rotation = rCache;
			}
		}
	}
}
