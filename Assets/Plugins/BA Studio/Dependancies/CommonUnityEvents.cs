using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BA_Studio.UnityLib.General.CommonEvents
{
	[System.SerializableAttribute]
	public class IntEvent : UnityEvent<int> {}

	[System.SerializableAttribute]
	public class Int2Event : UnityEvent<int, int> {}

	[System.SerializableAttribute]
	public class BoolEvent : UnityEvent<bool> {}

	[System.SerializableAttribute]
	public class ReturnBoolEvent : UnityEvent<BoolWrapper> {}

	[System.SerializableAttribute]
	public class StringEvent : UnityEvent<string> {}

	[System.SerializableAttribute]
	public class FloatEvent : UnityEvent<float> {}	

	[System.SerializableAttribute]
	public class LayerMaskEvent : UnityEvent<LayerMask> {}

	[System.SerializableAttribute]
	public class SceneEvent : UnityEvent<Scene> {}

	[System.SerializableAttribute]
	public class GameObjectEvent : UnityEvent<GameObject> {}

	[System.SerializableAttribute]
	public class Collider2DEvent : UnityEvent<Collider2D> {}

	[System.SerializableAttribute]
	public class Collision2DEvent : UnityEvent<Collision2D> {}

	[System.SerializableAttribute]
	public class ColliderEvent : UnityEvent<Collider> {}

	[System.SerializableAttribute]
	public class CollisionEvent : UnityEvent<Collision> {}
	
	[System.SerializableAttribute]
	public class BaseEventDataEvent : UnityEvent<UnityEngine.EventSystems.BaseEventData> {}

	public class BoolWrapper
	{
		public bool value;
	}
}