using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BA_Studio.UnityLib.VariableSystem;
using System.Linq;


namespace BA_Studio.UnityLib.VariableSystem
{
	///Use Check() to perform the check.
	public class VariableCheckBehaviour : MonoBehaviour
	{
		public bool DEBUG;

		[TooltipAttribute("Use this to distinguish when more then 1 Manipulation is attached.")]
		[SerializeField]
		string optionalID;

		public string ID{ get { return ID; }}

		public UnityEvent OnConditionMet;

		public VariableConditionGroupType conditionGroupType;

		public List<VariableConditionItem> conditions;

		public void Check()
		{
			switch (conditionGroupType){
				case VariableConditionGroupType.AND:
					bool temp = true;
					for (int i = 0; i < conditions.Count; i++){
						temp = temp & conditions[i].Check();
					}
					if (temp)
					{
						if (DEBUG) Debug.Log("Condition(OR) Met!");
						OnConditionMet?.Invoke();
					}
					break;
				case VariableConditionGroupType.OR:
					for (int i = 0; i < conditions.Count; i++){
						if (conditions[i].Check())
						{
							if (DEBUG) Debug.Log("Condition(OR) Met!");
							OnConditionMet?.Invoke();
						}
					}
					break;
			}
		}

		public static VariableCheckBehaviour GetSpecified(string ID, GameObject targetGO)
		{
			return targetGO.GetComponents<VariableCheckBehaviour>().First(e => e.optionalID == ID);
		}
	}

}
