using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.General
{
	[RequireComponent(typeof(Collider2D))]
	public class GenericTrigger2D : MonoBehaviour
	{
		public GameObjectFilter filter;

		public Collider2D triggerZone;

		public CommonEvents.Collider2DEvent triggerEnter, triggerStay, triggerExit;

		void Awake ()
		{
			if (triggerZone == null) triggerZone = this.GetComponent<Collider2D>();
		}

		private void OnTriggerEnter2D (Collider2D other)
		{
//			Debug.Log("other.tag:" + other.tag);
//			Debug.Log("tagFilter.Contains(other.tag): " +tagFilter.Contains(other.tag));
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
