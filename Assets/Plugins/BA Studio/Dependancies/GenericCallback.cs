using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.General
{
	public class GenericCallback : MonoBehaviour
	{

		public UnityEvent onAwake, onStart, onUpdate, onFixedUpate, onEnable, onDisable;

		void Awake ()
		{
			onAwake?.Invoke();
		}
		void Start ()
		{
			onStart?.Invoke();
		}
		void Update ()
		{
			onUpdate?.Invoke();
		}
		void FixedUpdate ()
		{
			onFixedUpate?.Invoke();
		}
		void OnEnable ()
		{
			onEnable?.Invoke();
		}
		void OnDisable ()
		{
			onDisable?.Invoke();
		}
	}
}
