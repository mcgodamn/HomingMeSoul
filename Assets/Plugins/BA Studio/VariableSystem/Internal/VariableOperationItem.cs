using UnityEngine;

namespace BA_Studio.UnityLib.VariableSystem
{
    [System.Serializable]
    public class VariableOperationItem
    {
        public string groupID;
        public string key = "Key";

        public VariableType type = VariableType.Float;
        public int vInt = 0;

        public float vFloat = 0f;
        
        public VariableOperation opt = VariableOperation.Set;

        public string vString = "";
        public bool vBool = false;

        public bool targetVar = false;

        public string targetGroupID, targetKey;

        //for inspector
        [HideInInspector]
        public bool unfolded = true;

        public void Execute ()
        {
            switch (this.type){
                case VariableType.Bool:
                    if (targetVar) this.vBool = VariableProfile_ES3.Instance.GetBool(this.targetGroupID, this.targetKey);
                    VariableProfile_ES3.Instance.SetVar(this.groupID, this.key, this.vBool);
                    break;
                case VariableType.Float:
                    if (targetVar) this.vFloat = VariableProfile_ES3.Instance.GetFloat(this.targetGroupID, this.targetKey);
                    switch (opt){
                        case VariableOperation.Add:                                
                            VariableProfile_ES3.Instance.SetVar(
                                this.groupID,
                                this.key,
                                VariableProfile_ES3.Instance.GetFloat(this.groupID, this.key) + this.vFloat );
                            break;
                        case VariableOperation.Subtract:                                
                            VariableProfile_ES3.Instance.SetVar(
                                this.groupID,
                                this.key,
                                VariableProfile_ES3.Instance.GetFloat(this.groupID, this.key) - this.vFloat );
                            break;
                        case VariableOperation.Multiply:                                
                            VariableProfile_ES3.Instance.SetVar(
                                this.groupID,
                                this.key,
                                VariableProfile_ES3.Instance.GetFloat(this.groupID, this.key) * this.vFloat );
                            break;
                        case VariableOperation.Divide:                                
                            VariableProfile_ES3.Instance.SetVar(
                                this.groupID,
                                this.key,
                                VariableProfile_ES3.Instance.GetFloat(this.groupID, this.key) / this.vFloat );
                            break;
                        case VariableOperation.Set:                                
                            VariableProfile_ES3.Instance.SetVar(
                                this.groupID,
                                this.key,
                                this.vFloat );
                            break;
                    }
                    break;
                case VariableType.String:
                    if (targetVar) this.vString = VariableProfile_ES3.Instance.GetString(this.targetGroupID, this.targetKey);
                    VariableProfile_ES3.Instance.SetVar(this.groupID, this.key, this.vString);
                    break;
                default:
                    break;

            }
        }

        public VariableOperationItem(string groupID, string key, VariableOperation opt, float value){
            this.groupID = groupID;
            this.key = key;
            this.type = VariableType.Float;
            this.opt = opt;
            this.vFloat = value;
        }
        public VariableOperationItem (string groupID, string key, VariableOperation opt, string value)
        {
            this.groupID = groupID;
            this.key = key;
            this.type = VariableType.String;
            this.opt = opt;
            this.vString = value;
        }
        public VariableOperationItem (string groupID, string key, VariableOperation opt, bool value)
        {
            this.groupID = groupID;
            this.key = key;
            this.type = VariableType.Bool;
            this.opt = opt;
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

    }
}