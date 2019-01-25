using UnityEngine;

namespace BA_Studio.UnityLib.VariableSystem
{
    [System.Serializable]
    public class VariableConditionItem
    {

        public string groupID;

        public string key = "Key";

        public VariableType type = VariableType.Float;
        public int vInt = 0;

        public float vFloat = 0f;
        
        public VariableCondition cond = VariableCondition.IsEqualTo;

        public string vString = "";
        public bool vBool = false;

        [TooltipAttribute("Negative condition will reverse the check result of this its condition.")]
        public bool isNegative= false;

        public bool targetVar = false;
        public string targetGroupID, targetKey;


        ///Return false when the designated variable key does not exist.
        public bool Check(){
            if (VariableProfile_ES3.Instance.CheckIfVariableExist(this.groupID, this.key)) return false;
            bool result = false;
            switch (this.type){
                case VariableType.Bool:
                    result = CheckBool();
                    break;
                case VariableType.Float:
                    result = CheckFloat();
                    break;
                case VariableType.String:
                    result = CheckString();
                    break;
            }
            if (isNegative) result = !result;
            return result;

        }

        public bool CheckFloat(){
            float value;                
            if (targetVar) value = VariableProfile_ES3.Instance.GetFloat(this.targetGroupID, this.targetKey);
            else value = this.vFloat;
            float profileValue = VariableProfile_ES3.Instance.GetFloat(this.groupID ,this.key);
            switch (this.cond){
                case VariableCondition.IsEqualTo:
                    return profileValue == value;
                case VariableCondition.IsGreaterThen:
                    return profileValue > value;
                case VariableCondition.IsGreaterOrEqualTo:
                    return profileValue >= value;
                case VariableCondition.IsNotEqualTo:
                    return profileValue != value;
                case VariableCondition.IsSmallerThen:
                    return profileValue < value;
                case VariableCondition.IsSmallerOrEqualTo:
                    return profileValue <= value;   
                default:
                    return false;
            }
        }

        public bool CheckString(){
            string value;                
            if (targetVar) value = VariableProfile_ES3.Instance.GetString(this.targetGroupID, this.targetKey);
            else value = this.vString;
            string profileValue = VariableProfile_ES3.Instance.GetString(this.groupID ,this.key);
            switch (this.cond){
                case VariableCondition.IsEqualTo:
                    return profileValue == value;
                case VariableCondition.IsNotEqualTo:
                    return profileValue != value;
                default:
                    return false;
            }
        }

        public bool CheckBool()
        {
            bool compared;                
            if (targetVar) compared = VariableProfile_ES3.Instance.GetBool(this.targetGroupID, this.targetKey);
            else compared = this.vBool;
            return VariableProfile_ES3.Instance.GetBool(this.groupID, this.key) == compared;
        }
        
        public VariableConditionItem(string groupID, string key, VariableCondition cond, float value){
            this.groupID = groupID;
            this.key = key;
            this.type = VariableType.Float;
            this.cond = cond;
            this.vFloat = value;
        }
        public VariableConditionItem(string groupID, string key, VariableCondition cond, string value){
            this.groupID = groupID;
            this.key = key;
            this.type = VariableType.String;
            this.cond = cond;
            this.vString = value;
        }
        public VariableConditionItem(string groupID, string key, VariableCondition cond, bool value){
            this.groupID = groupID;
            this.key = key;
            this.type = VariableType.Bool;
            this.cond = cond;
            this.vBool = value;
        }

        public string ValueToString ()
        {
            switch (this.type)
            {
                case VariableType.Bool:
                    return this.vBool.ToString();
                case VariableType.Float:
                    return this.vFloat.ToString();
                case VariableType.String:
                    return this.vString;
                default:
                    return string.Empty;
            }
        }
        
        //for inspector
        [HideInInspector]
        public bool unfolded = true;
// #if UNITY_EDITOR
//         public void OnEditorGUILayout () {
//             GUILayout.BeginVertical();                
//                 this.type = (VariableType) EditorGUILayout.EnumPopup("Type", this.type);
//                 switch (type) {
//                     case VariableType.Bool:
//                         this.groupID = EditorGUILayout.TextField(this.groupID);
//                         EditorGUILayout.PrefixLabel("Use Variable");
//                         this.targetVar = EditorGUILayout.Toggle(this.targetVar);
//                         if (this.targetVar) {
//                             this.targetGroupID = EditorGUILayout.TextField("Target Group ID", this.targetGroupID);
//                             this.targetKey = EditorGUILayout.TextField("Target Key ID", this.targetKey);
//                         }
//                         else {
//                             this.vBool = EditorGUILayout.Toggle("Value", this.vBool);
//                         }
//                         break;
//                     case VariableType.Float:
//                         this.groupID = EditorGUILayout.TextField(this.groupID);
//                         EditorGUILayout.PrefixLabel("Use Variable");
//                         this.targetVar = EditorGUILayout.Toggle(this.targetVar);
//                         this.cond = (VariableCondition) EditorGUILayout.EnumPopup("Condition", this.cond);
//                         if (this.targetVar) {
//                             this.targetGroupID = EditorGUILayout.TextField("Target Group ID", this.targetGroupID);
//                             this.targetKey = EditorGUILayout.TextField("Target Key ID", this.targetKey);
//                         }
//                         else {
//                             this.vBool = EditorGUILayout.Toggle("Value", this.vBool);
//                         }
//                         break;
//                     case VariableType.String:
//                         break;
//                 }
//             GUILayout.EndVertical();
//         }

//         public void OnEditorGUILayout (Rect rect) {
//             GUILayout.BeginArea(rect);
//                 GUILayout.BeginVertical();                
//                     this.type = (VariableType) EditorGUILayout.EnumPopup("Type", this.type);
//                     switch (type) {
//                         case VariableType.Bool:
//                             this.groupID = EditorGUILayout.TextField(this.groupID);
//                             EditorGUILayout.PrefixLabel("Use Variable");
//                             this.targetVar = EditorGUILayout.Toggle(this.targetVar);
//                             if (this.targetVar) {
//                                 this.targetGroupID = EditorGUILayout.TextField("Target Group ID", this.targetGroupID);
//                                 this.targetKey = EditorGUILayout.TextField("Target Key ID", this.targetKey);
//                             }
//                             else {
//                                 this.vBool = EditorGUILayout.Toggle("Value", this.vBool);
//                             }
//                             break;
//                         case VariableType.Float:
//                             this.groupID = EditorGUILayout.TextField(this.groupID);
//                             EditorGUILayout.PrefixLabel("Use Variable");
//                             this.targetVar = EditorGUILayout.Toggle(this.targetVar);
//                             this.cond = (VariableCondition) EditorGUILayout.EnumPopup("Condition", this.cond);
//                             if (this.targetVar) {
//                                 this.targetGroupID = EditorGUILayout.TextField("Target Group ID", this.targetGroupID);
//                                 this.targetKey = EditorGUILayout.TextField("Target Key ID", this.targetKey);
//                             }
//                             else {
//                                 this.vBool = EditorGUILayout.Toggle("Value", this.vBool);
//                             }
//                             break;
//                         case VariableType.String:
//                             break;
//                     }
//                 GUILayout.EndVertical();
//                 GUILayout.EndArea();
//         }
// #endif
    }
    
}