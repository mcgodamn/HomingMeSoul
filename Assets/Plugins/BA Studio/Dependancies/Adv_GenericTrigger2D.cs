using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.General
{
	[RequireComponent(typeof(Collider2D))]
	public class Adv_GenericTrigger2D : MonoBehaviour
	{
		public GameObjectFilter filter;

		public bool requirePositionInbound;

		public Collider2D triggerZone;

		public CommonEvents.Collider2DEvent triggerEnter, triggerStay, triggerExit, triggerStayPosOutside;

		void Awake ()
		{
			if (triggerZone == null) triggerZone = this.GetComponent<Collider2D>();
		}

		private void OnTriggerEnter2D (Collider2D other)
		{
//			Debug.Log("other.tag:" + other.tag);
//			Debug.Log("tagFilter.Contains(other.tag): " +tagFilter.Contains(other.tag));
			if (!this.enabled) return;
			if (filter.Match(other.gameObject) && (requirePositionInbound? (triggerZone.OverlapPoint(other.transform.position)) : true)) triggerEnter?.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (!this.enabled) return;
			if (filter.Match (other.gameObject))
			{
				if (requirePositionInbound)
				{
					if (triggerZone.OverlapPoint(other.transform.position)) triggerStay.Invoke(other);
					else triggerStayPosOutside.Invoke(other);
				}
				else triggerStay?.Invoke(other);
			}
		}
		
		private void OnTriggerExit2D(Collider2D other)
		{
			
			if (!this.enabled) return;
			if (filter.Match(other.gameObject)) triggerExit?.Invoke(other);
		}
	}

}
