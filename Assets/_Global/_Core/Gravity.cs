using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gravity
{
    public static Vector3 getGravity(Vector3 nowPosition)
    {
        Vector3 origin = Vector3.zero;
        Vector3 newVector = Vector3.zero;
        float distance;
        float speed = 1;
        float dx = nowPosition.x - origin.x;
        float dy = nowPosition.y - origin.y;
        float dz = nowPosition.z - origin.z;

        if(dx < 0)dx = -dx;
        if (dy < 0)dy = -dy;
        if (dz < 0)dz = -dz;

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
