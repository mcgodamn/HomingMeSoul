using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.UnityLib.VariableSystem;

namespace BA_Studio.UnityLib.VariableSystem
{
	///Use Execute() to perform all the operations.
	///A optional ID string is provided for distinguishing via script.
	public class VariableOperationBehaviour : MonoBehaviour
	{

		[TooltipAttribute("Use this to distinguish when more then 1 Manipulation is attached.")]
		[SerializeField]
		string optionalID;

		public string ID{ get {return ID;}}

		public List<VariableOperationItem> operations;

		public void Execute()
		{
			for (int i = 0; i < operations.Count; i++){
				operations[i].Execute();
			}
		}
	}
}

