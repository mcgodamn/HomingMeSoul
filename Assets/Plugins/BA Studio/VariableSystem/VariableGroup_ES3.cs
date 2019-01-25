using UnityEngine;
using System.Collections.Generic;

namespace BA_Studio.UnityLib.VariableSystem
{
	[System.Serializable]
	public class VariableGroup_ES3
	{
		public string groupID;
		public Dictionary<string, int> VarsInteger { get; private set; }
		public Dictionary<string, int[]> VarsIntegerArray { get; private set; }
		public Dictionary<string, float> VarsFloat { get; private set; }
		public Dictionary<string, float[]> VarsFloatArray { get; private set; }
		public Dictionary<string, bool> VarsBool { get; private set; }
		public Dictionary<string, bool[]> VarsBoolArray { get; private set; }
		public Dictionary<string, string> VarsString { get; private set; }
		public Dictionary<string, string[]> VarsStringArray { get; private set; }
		public Dictionary<string, VariableType> VarsTypeCache { get; private set; }

		public VariableGroup_ES3 ()
		{
			InitVars();
		}

		public VariableGroup_ES3 (string groupID)
		{
			InitVars();
			this.groupID = groupID;
		}

		public void InitVars(){
			if (VarsTypeCache == null) VarsTypeCache = new Dictionary<string, VariableType>();
			if (VarsInteger == null) VarsInteger = new Dictionary<string, int>();
			if (VarsFloat == null) VarsFloat = new Dictionary<string, float>();
			if (VarsBool == null) VarsBool = new Dictionary<string, bool>();
			if (VarsString == null) VarsString = new Dictionary<string, string>();
			if (VarsIntegerArray == null) VarsIntegerArray = new Dictionary<string, int[]>();
			if (VarsFloatArray == null) VarsFloatArray = new Dictionary<string, float[]>();
			if (VarsBoolArray == null) VarsBoolArray = new Dictionary<string, bool[]>();
			if (VarsStringArray == null) VarsStringArray = new Dictionary<string, string[]>();
		}

		public bool IfVariableExist (string key)
		{
			return VarsTypeCache.ContainsKey(key);
		}

		public VariableType GetVariableType (string key)
		{
			return VarsTypeCache[key];
		}

		public bool RemoveVariable (string key)
		{
			if (IfVariableExist(key))
			{
				switch (VarsTypeCache[key]){
					case VariableType.Float:
						VarsFloat.Remove(key);
						break;
					case VariableType.String:
						VarsString.Remove(key);
						break;
					case VariableType.Bool:
						VarsBool.Remove(key);
						break;
					case VariableType.FloatArray:
						VarsFloatArray.Remove(key);
						break;
					case VariableType.StringArray:
						VarsStringArray.Remove(key);
						break;
					case VariableType.BoolArray:
						VarsBoolArray.Remove(key);
						break;
				}
				VarsTypeCache.Remove(key);
				return true;
			}
			else return false;
		}

#region Single Variable

		public bool SetVariable (string key, int value, bool overwrite = true)
		{
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.Integer;
					VarsInteger[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.Integer);
				VarsInteger.Add(key, value);
				return true;
			}
		}

		public bool SetVariable (string key, float value, bool overwrite = true)
		{
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.Float;
					VarsFloat[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.Float);
				VarsFloat.Add(key, value);
				return true;
			}
		}

		public bool SetVariable (string key, bool value, bool overwrite = true)
		{
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.Bool;
					VarsBool[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.Bool);
				VarsBool.Add(key, value);
				return true;
			}
		}

		public bool SetVariable(string key, string value, bool overwrite = true)
		{
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.String;
					VarsString[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.String);
				VarsString.Add(key, value);
				return true;
			}
		}

		public void Get (string key, ref int value)
		{			
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting int: " + key);
			value = VarsInteger[key];
		}

		public void Get (string key, ref float value)
		{			
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting float: " + key);
			value = VarsFloat[key];
		}
		public void Get (string key, ref string value)
		{			
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting string: " + key);
			value = VarsString[key];
		}
		public void Get (string key, ref bool value)
		{			
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting bool: " + key);
			value = VarsBool[key];
		}

		public int GetInteger (string key)
		{
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting float: " + key);
			return VarsInteger[key];
		}

		public float GetFloat (string key)
		{
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting float: " + key);
			return VarsFloat[key];
		}
		public string GetString (string key)
		{
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting string: " + key);
			return VarsString[key];
		}

		public bool GetBool (string key)
		{
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting bool: " + key);
			return VarsBool[key];
		}

#endregion

// #region Utilities
// 			string GroupVariablesWorker<T>(T[] value) where T : struct{
// 				string temp = string.Empty;
// 				for (int i = 0; i < value.Length; i++){
// 					temp += value[i].ToString();
// 					if (i != value.Length - 1) temp += varArrBreak;
// 				}
// 				return temp;
// 			}

// 			string GroupVariablesWorker(string[] value){
// 				string temp = string.Empty;
// 				for (int i = 0; i < value.Length; i++){
// 					temp += value[i].ToString();
// 					if (i != value.Length - 1) temp += varArrBreak;
// 				}
// 				return temp;
// 			}

// 			T[] VariableArrayResolver<T>(string storedString) where T : struct{
// 				string[] spliter = new string[1];
// 				spliter[0] = varArrBreak;
// 				string[] temp = storedString.Split(spliter, System.StringSplitOptions.None);
// 				T[] returning = new T[temp.Length];
			
// 				for (int i = 0; i <temp.Length; i++)
// 				{
// 					returning[i] = (T) System.Convert.ChangeType(temp[i], typeof(T));
// 				}
// 				return returning;				
// 			}
		
// 			string[] VariableArrayResolver(string storedString){
// 				string[] spliter = new string[1];
// 				spliter[0] = varArrBreak;
// 				string[] temp = storedString.Split(spliter, System.StringSplitOptions.None);

// 				return temp;				
// 			}
// #endregion

#region Array Variable

		public bool SetVariable (string key, int[] value, bool overwrite = true)
		{			
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.IntegerArray;
					VarsIntegerArray[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.IntegerArray);
				VarsIntegerArray.Add(key, value);
				return true;
			}
		}

		public bool SetVariable (string key, float[] value, bool overwrite = true)
		{
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.FloatArray;
					VarsFloatArray[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.FloatArray);
				VarsFloatArray.Add(key, value);
				return true;
			}
		}

		public bool SetVariable (string key, bool[] value, bool overwrite = true){
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.BoolArray;
					VarsBoolArray[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.BoolArray);
				VarsBoolArray.Add(key, value);
				return true;
			}
		}

		public bool SetVariable (string key, string[] value, bool overwrite = true){
			if (VarsTypeCache.ContainsKey(key))
			{
				if (!overwrite) return false;
				else
				{
					VarsTypeCache[key] = VariableType.StringArray;
					VarsStringArray[key] = value;
					return true;
				}
			}
			else
			{
				VarsTypeCache.Add(key, VariableType.StringArray);
				VarsStringArray.Add(key, value);
				return true;
			}
		}

		public int[] GetIntegerArr (string key)
		{			
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting float array: " + key);
			return VarsIntegerArray[key];
		}

		public float[] GetFloatArr (string key)
		{
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting float array: " + key);
			return VarsFloatArray[key];
		}
		public string[] GetStringArr (string key)
		{
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting string array: " + key);
			return VarsStringArray[key];
		}

		public bool[] GetBoolArr (string key)
		{
			if (!IfVariableExist(key)) VariableProfile_ES3.ErrorLog("getting bool array: " + key);
			return VarsBoolArray[key];
		}
#endregion
	}
}