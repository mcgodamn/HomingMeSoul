using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BA_Studio.UnityLib.Utilities;

namespace BA_Studio.UnityLib.General
{
	public class GenericCollision2D : MonoBehaviour
	{
		public GameObjectFilter filter;

		public CommonEvents.Collision2DEvent collisionEnter, collisionStay, collisionExit;

		private void OnCollisionEnter2D(Collision2D collision)
		{			
			if (!this.enabled) return;
			if (filter.Match(collision.gameObject)) collisionEnter?.Invoke(collision);
		}
		private void OnCollisionStay2D(Collision2D collision)
		{
			if (!this.enabled) return;
			if (filter.Match(collision.gameObject)) collisionStay?.Invoke(collision);		
		}
		private void OnCollisionExit2D(Collision2D collision)
		{
			if (!this.enabled) return;
			if (filter.Match(collision.gameObject)) collisionExit?.Invoke(collision);		
		}
	}

}
