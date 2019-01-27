using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DegreeUtility
{

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static float GetFaceDegree(Vector2 origin, Vector2 position)
    {
        Vector2 direction = position - origin;
        return Vector2.SignedAngle(direction, Vector2.right);
        
    }
}