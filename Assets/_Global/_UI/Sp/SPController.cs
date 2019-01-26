using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPController : MonoBehaviour
{
    public IntReference spR;

    public Slider slider;

    void Update ()
    {
        slider.value = spR.Value/100f;
    }
}
