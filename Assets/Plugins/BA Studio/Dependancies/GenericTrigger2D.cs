using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.General
{
	[RequireComponent(typeof(Collider2D))]
	public class GenericTrigger2D : MonoBehaviour
	{
		public GameObjectFilter filter;

		public CommonEvents.Collider2DEvent triggerEnter, triggerStay, triggerExit;

		private void OnTriggerEnter2D (Collider2D other)
		{
			if (!this.enabled) return;
			if (filter.Match(other.gameObject)) triggerEnter?.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (!this.enabled) return;
			if (filter.Match(other.gameObject)) triggerStay?.Invoke(other);
		}
		
		private void OnTriggerExit2D(Collider2D other)
		{
			
			if (!this.enabled) return;
			if (filter.Match(other.gameObject)) triggerExit?.Invoke(other);
		}
	}

}
