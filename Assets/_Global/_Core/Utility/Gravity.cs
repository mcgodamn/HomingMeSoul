using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gravity
{
    public static Vector3 GetGravity(Vector3 nowPosition, float speed)
    {
        AngerStudio.HomingMeSoul.Game.GameCore gameCore = AngerStudio.HomingMeSoul.Game.GameCore.Instance;
        float gravityMultiplier = gameCore.config.Value.gravityMultiplier; 


        Vector3 origin = gameCore.scoreBase.transform.position;
        Vector3 newVector = Vector3.zero;
        float distance;
        float dx = nowPosition.x - origin.x;
        float dy = nowPosition.y - origin.y;
        float dz = nowPosition.z - origin.z;

        distance = Mathf.Sqrt(dx * dx + dy * dy + dz * dz);

        dx = origin.x - nowPosition.x;
        dy = origin.y - nowPosition.y;
        dz = origin.z - nowPosition.z;

        newVector.x = dx / distance;
        newVector.y = dy / distance;
        newVector.z = dz / distance;
        return (speed * newVector);
    }

}
