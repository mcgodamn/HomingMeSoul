using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BA_Studio.General.Logic
{
	public interface IOperation
	{
		void Operate ();
	}
	
	public interface IOperationGroup
	{
		List<IOperation> operations { get; }

		void Operate ();
	}
}
