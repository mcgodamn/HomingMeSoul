using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace BA_Studio.UnityLib.General {

	/// Based on 3x3 numpad.
	public enum BasicDirections : byte {
		Left = 4,
		UpperLeft = 7,
		Up = 8,
		UpperRight = 9,
		Right = 6,
		LowerRight = 3,
		Down = 2,
		LowerLeft = 1,
		Custom = 0
	}

	public enum FourDirections : byte
	{
		Left = 4,
		Up = 8,
		Right = 6,
		Down = 2
	}

	public enum CardinalDirections {
		West,
		NorthWest,
		North,
		NorthEast,
		East,
		SouthEast,
		South,
		SouthWest,
		Custom
	}

	public enum CommonDirectionToDirection {
		LeftToRight,
		RightToLeft,
		TopToBottom,
		BottomToTop
	}

	public enum UpdateFrequency
	{
		Update,
		FixedUpdate,
		Millisecond,
		Second,
		Minute,
		Hour,
		Custom
	}

	public static class CommonLibUtil {
		
		public static Vector2 DirectionToVector (CardinalDirections Direction) {
			switch (Direction) {
				case CardinalDirections.West:
					return Vector2.left;
				case CardinalDirections.NorthWest:
					return new Vector2(-1, 1).normalized;
				case CardinalDirections.North:
					return Vector2.up;
				case CardinalDirections.NorthEast:
					return new Vector2(1, 1).normalized;
				case CardinalDirections.East:
					return Vector2.right;
				case CardinalDirections.SouthEast:
					return new Vector2(1, -1).normalized;
				case CardinalDirections.South:
					return Vector2.down;
				case CardinalDirections.SouthWest:
					return new Vector2(-1, -1).normalized;
				default:
					return Vector2.zero;
			}
		}

		public static Vector2 DirectionToVector (BasicDirections Direction) {
			switch (Direction) {
				case BasicDirections.Left:
					return Vector2.left;
				case BasicDirections.UpperLeft:
					return new Vector2(-1, 1).normalized;
				case BasicDirections.Up:
					return Vector2.up;
				case BasicDirections.UpperRight:
					return new Vector2(1, 1).normalized;
				case BasicDirections.Right:
					return Vector2.right;
				case BasicDirections.LowerRight:
					return new Vector2(1, -1).normalized;
				case BasicDirections.Down:
					return Vector2.down;
				case BasicDirections.LowerLeft:
					return new Vector2(-1, -1).normalized;
				default:
					return Vector2.zero;
			}
		}

		public static Vector2 DirectionToVector (FourDirections Direction) {
			switch (Direction) {
				case FourDirections.Left:
					return Vector2.left;
				case FourDirections.Up:
					return Vector2.up;
				case FourDirections.Right:
					return Vector2.right;
				case FourDirections.Down:
					return Vector2.down;
				default:
					return Vector2.zero;
			}
		}
		// public static Quaternion DirectionToRotation (BasicDirections direction)
		// {
		// 	switch (direction) {
		// 		case BasicDirections.Left:
		// 			return Vector2.left;
		// 		case BasicDirections.UpperLeft:
		// 			return new Vector2(-1, 1).normalized;
		// 		case BasicDirections.Up:
		// 			return Vector2.up;
		// 		case BasicDirections.UpperRight:
		// 			return new Vector2(1, 1).normalized;
		// 		case BasicDirections.Right:
		// 			return Vector2.right;
		// 		case BasicDirections.LowerRight:
		// 			return new Vector2(1, -1).normalized;
		// 		case BasicDirections.Down:
		// 			return Vector2.down;
		// 		case BasicDirections.LowerLeft:
		// 			return new Vector2(-1, -1).normalized;
		// 		default:
		// 			return Vector2.zero;
		// 	}
		// }

		public static Dictionary<string, T> ListToDicitonary<T> (List<T> list, System.Func<T, string> getID)
		{
			Dictionary<string, T> newDict = new Dictionary<string, T>();
			foreach (T item in list)
			{
				newDict.Add(getID(item), item);
			}
			return newDict;
		}
	}
		
	public enum ConditionGroupType
	{
		AND,
		OR
	}
	
	public enum Condition
	{
		IsEqualTo,
		IsNotEqualTo,
		IsGreaterThen,
		IsSmallerThen,
		IsGreaterOrEqualTo,
		IsSmallerOrEqualTo
	}
}