using UnityEngine;

public class CurveAnimator3D : MonoBehaviour
{
    [Header("Animation")]
    public AnimationCurve positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float duration = 1f;
    public bool playOnStart = false;

    [Header("Target Transform (Local)")]
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public Vector3 targetScale = Vector3.one;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private Quaternion targetRotationQuaternion;

    private bool isPlaying;
    private float timer;
    private void Awake()
    {
        CacheStartValues();
    }

    private void Start()
    {
        if (playOnStart)
            PlayAnimation();
    }

    public void CacheStartValues()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        startScale = transform.localScale;
        
    }

    public void ResetAnimation()
    {
        isPlaying = false;
        timer = 0f;

        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        transform.localScale = startScale;
    }

    public void PlayAnimation()
    {
        if (isActiveAndEnabled)
        {
            ResetAnimation();
            // Zielrotation in lokale Euler-Rotation umwandeln
            targetRotationQuaternion = Quaternion.Euler(targetRotation);

            timer = 0f;
            isPlaying = true;
        }
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        timer += Time.deltaTime;

        float t = duration > 0f
            ? Mathf.Clamp01(timer / duration)
            : 1f;

        // Lokale Position
        float posT = positionCurve.Evaluate(t);

        transform.localPosition = Vector3.LerpUnclamped(
            startPosition,
            targetPosition,
            posT
        );

        // Lokale Rotation
        float rotT = rotationCurve.Evaluate(t);

        transform.localRotation = Quaternion.SlerpUnclamped(
            startRotation,
            targetRotationQuaternion,
            rotT
        );

        // Lokale Skalierung
        float scaleT = scaleCurve.Evaluate(t);

        transform.localScale = Vector3.LerpUnclamped(
            startScale,
            targetScale,
            scaleT
        );

        if (t >= 1f)
            isPlaying = false;
    }
}
