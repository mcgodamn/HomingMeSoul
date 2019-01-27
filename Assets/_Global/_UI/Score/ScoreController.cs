using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public IntReference scoreR;

    public TMPro.TextMeshProUGUI text;

    void Update ()
    {
        text.text = scoreR.Value.ToString("000");
    }
}
