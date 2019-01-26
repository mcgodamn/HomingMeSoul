using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AngerStudio.WeedProject
{
	public class MassRotator : MonoBehaviour
	{
		public Vector3 axis = Vector3.up;

		public float rotationSpeed = 5f;

		[Tooltip("This could work with the issue Animator locking properties.")]
		public bool withAnimator;

		public List<GameObject> targets;

		void Awake()
		{
			rCache = this.transform.rotation;
			targets = new List<GameObject>();
		}

		void Update ()
		{
			if (!withAnimator) foreach (GameObject g in targets) g.transform.Rotate(this.axis, this.rotationSpeed);
		}

		Quaternion rCache;

		void LateUpdate ()
		{
			if (withAnimator)
			{
				foreach (GameObject g in targets) 
				{
					rCache *= Quaternion.Euler(axis * rotationSpeed);
					this.transform.rotation = rCache;
				}
			}
		}
	}
}
