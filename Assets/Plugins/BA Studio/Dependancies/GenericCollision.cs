using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.General
{
	public class GenericCollision : MonoBehaviour
	{
		public GameObjectFilter filter;

		public CommonEvents.CollisionEvent collisionEnter, collisionStay, collisionExit;

		private void OnCollisionEnter(Collision collision)
		{
			if (!this.enabled) return;
			if (filter.Match(collision.gameObject)) collisionEnter?.Invoke(collision);
		}
		private void OnCollisionStay(Collision collision)
		{
			if (!this.enabled) return;
			if (filter.Match(collision.gameObject)) collisionStay?.Invoke(collision);	
		}
		private void OnCollisionExit(Collision collision)
		{
			if (!this.enabled) return;
			if (filter.Match(collision.gameObject)) collisionExit?.Invoke(collision);	
		}
	}

}
