using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BA_Studio.UnityLib.Utilities;

namespace BA_Studio.UnityLib.General
{
	[RequireComponent(typeof(Collider))]
	public class GenericTrigger : MonoBehaviour
	{
		public GameObjectFilter filter;

		public CommonEvents.ColliderEvent triggerEnter, triggerStay, triggerExit;
		private void OnTriggerEnter (Collider other)
		{
			if (!this.enabled) return;
			if (filter.Match(other.gameObject)) triggerEnter?.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			if (!this.enabled) return;
			if (filter.Match(other.gameObject)) triggerStay?.Invoke(other);
		}
		
		private void OnTriggerExit(Collider other)
		{
			
			if (!this.enabled) return;
			if (filter.Match(other.gameObject)) triggerExit?.Invoke(other);
		}
	}

}
