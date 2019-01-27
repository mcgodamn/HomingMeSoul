using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using BA_Studio.SingletonLocator;

namespace BA_Studio.UnityLib.VariableSystem
{
	[System.Serializable]
    public class VariableProfile_ES3
	{

		public static VariableProfile_ES3 Instance { get { return SingletonLocator<VariableProfile_ES3>.Instance; }}
		
		public Dictionary<string, VariableGroup_ES3> VarGroups { get; private set; }

        public VariableProfile_ES3 (bool staticSingleton = true)
        {
			if (VarGroups == null) VarGroups = new Dictionary<string, VariableGroup_ES3>();
			if (staticSingleton) SingletonLocator<VariableProfile_ES3>.Instance = this;
        }

		public static void ErrorLog(string action)
		{			
				Debug.LogWarning("VariableSystem:: VariableProfileCache:: An error occured when " + action);
		}

		public static void AutoFixedErrorLog(string action)
		{			
				Debug.LogWarning("VariableSystem:: VariableProfileCache:: A potential error found but auto fixed when " + action);
		}

		///Return false when a group with same name already exist.
		bool AddGroup (string groupID)
		{
			if (VarGroups.ContainsKey(groupID)) return false;
			VarGroups.Add(groupID, new VariableGroup_ES3(groupID));
			return true;
		}

		// public string TranslateVarString (string input){			
		// 	string[] matchedGroups = Regex.Matches(input, @"{\%[\w\s:]+\%}")
		// 								  .Cast<Match>()
		// 								  .Select(m => m.Value)
		// 								  .ToArray();
		// 	foreach (string s in matchedGroups) {
		// 		string[] target;
		// 		target = s.Split(new char[]{':'}, 2, System.StringSplitOptions.None);
		// 		input.Replace(s, this.GetVarAsString(target[0], target[1]));
		// 	}
		// 	return input;
		// }


#region Variable Getters

		public float GetFloat(string groupID, string key, float defaultValue = 0f)
		{
			try
			{
				return VarGroups[groupID].GetFloat(key);
			}
			catch
			{
				return defaultValue;
			}
		}
		public string GetString(string groupID, string key, string defaultValue = null)
		{
			try
			{
				return VarGroups[groupID].GetString(key);
			}
			catch
			{
				return defaultValue;
			}
		}

		public bool GetBool(string groupID, string key, bool defaultValue = false)
		{
			try
			{
				return VarGroups[groupID].GetBool(key);
			}
			catch
			{
				return defaultValue;
			}
		}
		// public float[] GetFloatArr(string groupID, string key){
		// 	return VarGroups[groupID].GetFloatArr(key);
		// }
		// public string[] GetStringArr(string groupID, string key){
		// 	return VarGroups[groupID].GetStringArr(key);
		// }

		// public bool[] GetBoolArr(string groupID, string key){
		// 	return VarGroups[groupID].GetBoolArr(key);
		// }

		public string GetVarAsString (string groupID, string key)
		{
			if (!VarGroups.ContainsKey(groupID)) return null;
			if (VarGroups[groupID].IfVariableExist(key))
			{
				VariableGroup_ES3 targetGroup = VarGroups[groupID];
				switch (targetGroup.GetVariableType(key))
				{
					case VariableType.Float:
						return targetGroup.GetFloat(key).ToString();
					case VariableType.String:
						return targetGroup.GetString(key).ToString();
					case VariableType.Bool:
						return targetGroup.GetBool(key).ToString();
					case VariableType.FloatArray:
						return targetGroup.GetFloatArr(key).ToString();
					case VariableType.StringArray:
						return targetGroup.GetStringArr(key).ToString();
					case VariableType.BoolArray:
						return targetGroup.GetBoolArr(key).ToString();
					default:
						return null;
				}			
			}
			else return null;
		}
#endregion

#region Variable Setter
		public void SetVar(string groupID, string key, int value)
		{
			if (!VarGroups.ContainsKey(groupID)){
				AddGroup(groupID);
				AutoFixedErrorLog("setting float: " + key + " in group: " + groupID + " but the group did not exist.");
			}
			VarGroups[groupID].SetVariable(key, value);
		}

		public void SetVar(string groupID, string key, float value)
		{
			if (!VarGroups.ContainsKey(groupID)){
				AddGroup(groupID);
				AutoFixedErrorLog("setting float: " + key + " in group: " + groupID + " but the group did not exist.");
			}
			VarGroups[groupID].SetVariable(key, value);
		}
		public void SetVar(string groupID, string key, string value)
		{
			if (!VarGroups.ContainsKey(groupID)){
				AddGroup(groupID);
				AutoFixedErrorLog("setting string: " + key + " in group: " + groupID + " but the group did not exist.");
			}
			VarGroups[groupID].SetVariable(key, value);
		}
		public void SetVar(string groupID, string key, bool value)
		{
			if (!VarGroups.ContainsKey(groupID)){
				AddGroup(groupID);
				AutoFixedErrorLog("setting bool: " + key + " in group: " + groupID + " but the group did not exist.");
			}
			VarGroups[groupID].SetVariable(key, value);
		}
		// public void SetVar(string groupID, string key, bool[] value){
		// 	if (!VarGroups.ContainsKey(groupID)){
		// 		AddGroup(groupID);
		// 		AutoFixedErrorLog("setting bool array: " + key + " in group: " + groupID + " but the group did not exist.");
		// 	}
		// 	VarGroups[groupID].SetVariableArr(key, value);
		// }
		// public void SetVar(string groupID, string key, float[] value){
		// 	if (!VarGroups.ContainsKey(groupID)){
		// 		AddGroup(groupID);
		// 		AutoFixedErrorLog("setting float array: " + key + " in group: " + groupID + " but the group did not exist.");
		// 	}
		// 	VarGroups[groupID].SetVariableArr(key, value);
		// }
		// public void SetVar(string groupID, string key, string[] value){
		// 	if (!VarGroups.ContainsKey(groupID)){
		// 		AddGroup(groupID);
		// 		AutoFixedErrorLog("setting string array: " + key + " in group: " + groupID + " but the group did not exist.");
		// 	}
		// 	VarGroups[groupID].SetVariableArr(key, value);
		// }

		public bool CheckIfVariableExist(string groupID, string key){
			if (!VarGroups.ContainsKey(groupID)) return false;
			else return VarGroups[groupID].IfVariableExist(key);
		}

#endregion

	}
}