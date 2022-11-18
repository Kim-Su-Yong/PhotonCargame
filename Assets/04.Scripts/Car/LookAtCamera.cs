using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform CanvasTr;
    private Transform mainCamTr;
    void Start()
    {
        CanvasTr = GetComponent<RectTransform>();
        mainCamTr = Camera.main.transform;
    }

    void Update()
    {
        CanvasTr.LookAt(mainCamTr);
    }
}
