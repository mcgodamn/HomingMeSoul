using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.General
{
	public class GenericCollision2D : MonoBehaviour
	{
		public Collider2D col;
		public List<string> tagFilter;
		public CommonEvents.Collision2DEvent collisionEnter, collisionStay, collisionExit;

		void Awake ()
		{
			if (col == null) col = this.GetComponent<Collider2D>();
		}
		private void OnCollisionEnter2D(Collision2D collision)
		{			
			if (tagFilter.Contains(collision.gameObject.tag)) collisionEnter?.Invoke(collision);
		}
		private void OnCollisionStay2D(Collision2D collision)
		{
			if (tagFilter.Contains(collision.gameObject.tag)) collisionStay?.Invoke(collision);			
		}
		private void OnCollisionExit2D(Collision2D collision)
		{
			if (tagFilter.Contains(collision.gameObject.tag)) collisionExit?.Invoke(collision);			
		}
	}

}
