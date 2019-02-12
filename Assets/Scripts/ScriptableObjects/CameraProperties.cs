using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Properties", menuName = "ElMasna3/Camera Properties")]
public class CameraProperties : ScriptableObject
{
    [Header("Camera Range")]
    public float minimumHeight;
    public float maximumHeight;

    [Header("Zoom Orthographic")]
    public float zoomOutMin;
    public float zoomOutMax;

    [Header("Zoom Prespective")]
    public float minimumZoom;
    public float maximumZoom;
}
