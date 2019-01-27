using System;
using UnityEngine;
using NoBrainer.Utility;

[Serializable, CreateAssetMenu(menuName = "Variable/UnityObjects/GameObjectArray")]
public class GameObjectArrayVariable : GlobalVariable<GameObject[]>
{ }
