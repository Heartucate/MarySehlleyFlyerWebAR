using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    [Header("General Settings")]
    public bool playOnStart = true;
    public bool loop = true;

    [Header("References")]
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    [Header("Position")]
    public bool animatePosition = true;
    public float positionSpeed = 1f;
    public AnimationCurve positionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Vector3 positionOffset = new Vector3(0, 100, 0);

    [Header("Rotation")]
    public bool animateRotation = true;
    public float rotationSpeed = 1f;
    public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Vector3 rotationOffset = new Vector3(0, 0, 360);

    [Header("Scale")]
    public bool animateScale = true;
    public float scaleSpeed = 1f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Vector3 scaleOffset = new Vector3(0.2f, 0.2f, 0.2f);

    [Header("Alpha")]
    public bool animateAlpha = true;
    public float alphaSpeed = 1f;
    public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Range(-1f, 1f)]
    public float alphaOffset = 1f;

    private Vector3 startPosition;
    private Vector3 startRotation;
    private Vector3 startScale;
    private float startAlpha;

    private float positionTime;
    private float rotationTime;
    private float scaleTime;
    private float alphaTime;

    private bool isPlaying;

    void Awake()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        startPosition = rectTransform.anchoredPosition;
        startRotation = rectTransform.localEulerAngles;
        startScale = rectTransform.localScale;

        if (canvasGroup != null)
            startAlpha = canvasGroup.alpha;
    }

    void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    void Update()
    {
        if (!isPlaying)
            return;

        AnimatePosition();
        AnimateRotation();
        AnimateScale();
        AnimateAlpha();
    }

    void AnimatePosition()
    {
        if (!animatePosition)
            return;

        positionTime += Time.deltaTime * positionSpeed;

        float progress = GetProgress(positionTime, loop, positionCurve);

        rectTransform.anchoredPosition = startPosition + positionOffset * progress;
    }

    void AnimateRotation()
    {
        if (!animateRotation)
            return;

        rotationTime += Time.deltaTime * rotationSpeed;

        float progress = GetProgress(rotationTime, loop, rotationCurve);

        rectTransform.localEulerAngles = startRotation + rotationOffset * progress;
    }

    void AnimateScale()
    {
        if (!animateScale)
            return;

        scaleTime += Time.deltaTime * scaleSpeed;

        float progress = GetProgress(scaleTime, loop, scaleCurve);

        rectTransform.localScale = startScale + scaleOffset * progress;
    }

    void AnimateAlpha()
    {
        if (!animateAlpha || canvasGroup == null)
            return;

        alphaTime += Time.deltaTime * alphaSpeed;

        float progress = GetProgress(alphaTime, loop, alphaCurve);

        canvasGroup.alpha = Mathf.Lerp(startAlpha, alphaOffset, progress);
    }

    float GetProgress(float time, bool shouldLoop, AnimationCurve curve)
    {
        float progress;

        if (shouldLoop)
        {
            progress = time % 1f;
        }
        else
        {
            progress = Mathf.Clamp01(time);
        }

        return curve.Evaluate(progress);
    }

    public void Play()
    {
        isPlaying = true;
    }

    public void Stop()
    {
        isPlaying = false;
    }

    public void ResetAnimation()
    {
        positionTime = 0f;
        rotationTime = 0f;
        scaleTime = 0f;
        alphaTime = 0f;

        rectTransform.anchoredPosition = startPosition;
        rectTransform.localEulerAngles = startRotation;
        rectTransform.localScale = startScale;

        if (canvasGroup != null)
            canvasGroup.alpha = startAlpha;
    }


}