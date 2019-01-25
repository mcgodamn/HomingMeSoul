using System.Collections.Generic;

namespace BA_Studio.UnityLib.VariableSystem
{
    [System.Serializable]
    public class VariableConditionGroup
    {
        public VariableConditionGroupType groupType;

        public List<VariableConditionItem> conditions;

        public VariableConditionGroup () {
            this.groupType = VariableConditionGroupType.AND;
            this.conditions = new List<VariableConditionItem>();
        }

        public VariableConditionGroup (VariableConditionGroup source) {
            this.groupType = source.groupType;
            this.conditions = new List<VariableConditionItem>(source.conditions);
        }

        public bool Check(){
            switch (groupType){
                case VariableConditionGroupType.AND:
                    bool temp = true;
                    for (int i = 0; i < conditions.Count; i++){
                        temp = temp & conditions[i].Check();
                    }
                    return temp;
                case VariableConditionGroupType.OR:
                    for (int i = 0; i < conditions.Count; i++){
                        if (conditions[i].Check()) return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}