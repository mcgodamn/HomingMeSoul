using System;
using UnityEngine;
using NoBrainer.Utility;

namespace AngerStudio.HomingMeSoul.Game
{

    [Serializable, CreateAssetMenu(menuName = "Variable/Custom/GmaeConfig")]
    public class GameConfigVariable : GlobalVariable<GameConfig>
    { }
}