using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MaryGlitchSettings", menuName = "Heartucate/Mary Glitch Settings", order = 1)]
public class MaryGlitchSettings : ScriptableObject
{
    [Header("BandSettings")]
    [Tooltip("Main band settings")]
    public float BandAxisLength = 0.075f;
    public float BandAxisCenter = 0.075f;
    public float BandSpeed = 0.075f;
    public float TransitionLength = 0.075f;
    public Vector3 BandAxis = Vector3.up;

    [Header("Grid")]
    [Tooltip("Child band settings")]
    public float BandSpeed1 = 0.075f;
    public float BandOffset1 = 0.075f;
    public float BandSpeed2 = 0.075f;
    public float BandOffset2 = 0.075f;
    public float BandSpeed3 = 0.075f;
    public float BandOffset3 = 0.075f;
    public float BandSpeed4 = 0.075f;
    public float BandOffset4 = 0.075f;

}
