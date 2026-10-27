using System.Collections;
using UnityEngine;

public class AudioLoopStartTrigger : MonoBehaviour
{
    [Header("Audio Loop Manager")]
    [SerializeField]
    private AudioLoopManager audioLoopManager;

    [Header("Loop Settings")]
    [SerializeField]
    private int loopIndex = 0;

    [Min(0f)]
    [SerializeField]
    private float delayInSeconds = 0f;

    [SerializeField]
    private bool stopPlaying = false;

    private void Start()
    {
        StartCoroutine(StartLoopDelayed());
    }

    private IEnumerator StartLoopDelayed()
    {
        // Warten, bevor der Loop gestartet wird
        if (delayInSeconds > 0f)
        {
            yield return new WaitForSeconds(delayInSeconds);
        }

        if (audioLoopManager == null)
        {
            Debug.LogWarning("AudioLoopManager ist nicht zugewiesen.");
            yield break;
        }

        if (stopPlaying)
        {
            audioLoopManager.StopLoop();
            yield break;
        }

        audioLoopManager.PlayLoop(loopIndex);
    }
}

