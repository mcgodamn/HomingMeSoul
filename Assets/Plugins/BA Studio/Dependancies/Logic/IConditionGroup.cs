using System.Collections.Generic;
using BA_Studio.UnityLib.General;

namespace BA_Studio.General.Logic
{
    [System.Serializable]
    public enum ConditionGroupType
    {
        AND,
        OR
    }
    
    public interface IConditionGroup
	{
        ConditionGroupType groupType { get; }
		List<ICondition> conditions { get; }
		bool Check();
	}
}
