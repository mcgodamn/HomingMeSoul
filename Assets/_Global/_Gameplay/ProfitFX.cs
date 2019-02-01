using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Events;
using BA_Studio.UnityLib.GameObjectPool;

public class ProfitFX : MonoBehaviour
{
    public GameObject linePrefab;
    LineRenderer lineInstance;

    public float durationPerPath = 0.6f;

    public int segmentPerPath = 10;

    void Awake ()
    {
        lineInstance = GameObject.Instantiate(linePrefab).GetComponent<LineRenderer>();
        lineInstance.gameObject.SetActive(false);
    }

    public void StartLine (Vector3[] path, bool loop = true, System.Action onStart = null, System.Action onDone = null)
    {
        if (loop)
        {
            Vector3[] t = new Vector3[path.Length];
            path.CopyTo(t, 0);
            path = new Vector3[t.Length + 1];
            t.CopyTo((path as Vector3[]), 0);
        }
        Timing.RunCoroutine(FX(path, onStart, onDone));
    }

    IEnumerator<float> FX (Vector3[] path, System.Action onStart = null, System.Action onDone = null)
    {
        lineInstance.gameObject.SetActive(true);
        Vector3[] pathPoints = new Vector3[path.Length * segmentPerPath];
        int currentIndex = 0;
        float durationPerPoint = durationPerPath / segmentPerPath;
        onStart?.Invoke();
        lineInstance.positionCount = pathPoints.Length;

        for (int i = 0; i < path.Length; i += segmentPerPath)
        {
            CaculatePathPoints(path[i], path[i + 1], segmentPerPath).CopyTo(pathPoints, i * segmentPerPath);
        }

        for (int i = 0; i < pathPoints.Length - 1; i++)
        {
            lineInstance.SetPosition(currentIndex, pathPoints[currentIndex]);
            currentIndex += 1;
            yield return Timing.WaitForSeconds(durationPerPoint);
        }
        lineInstance.gameObject.SetActive(false);
        onDone?.Invoke();
    }

    Vector3[] CaculatePathPoints (Vector3 from, Vector3 to, int segments)
    {
        Vector3 path = from - to;
        Vector3[] result = new Vector3[segments];
        path /= (float) segments;
        for (int i = 1; i < segments; i++)
        {
            result[i - 1] = from + path * i;
        }
        result[segments - 1] = to;
        return result;
    }
}
