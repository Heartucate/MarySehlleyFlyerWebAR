using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimedEvent
{
    [Tooltip("Zeit in Sekunden nach StartGame()")]
    public float delay;

    public UnityEvent onEvent;
}


public class GameTimer : MonoBehaviour
{
    [Header("Timed Events")]
    public List<TimedEvent> events = new List<TimedEvent>();

    private Coroutine timerCoroutine;

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        // Falls bereits ein Timer läuft, wird er beendet
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(RunTimer());
    }

    public void ResetAnimations()
    {
        // Alle Nodes inklusive dieses Objekts durchsuchen
        MonoBehaviour[] components = FindObjectsOfType<MonoBehaviour>(true);

        foreach (MonoBehaviour component in components)
        {
            if (component == null)
                continue;

            // Nur Nodes innerhalb dieser Hierarchie berücksichtigen
            if (component.transform != transform &&
                !component.transform.IsChildOf(transform))
                continue;

            // ResetAnimation() aufrufen, falls die Methode existiert
            var method = component.GetType().GetMethod(
                "ResetAnimation",
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance,
                null,
                System.Type.EmptyTypes,
                null
            );

            if (method != null)
            {
                method.Invoke(component, null);
            }
        }
    }


    private IEnumerator RunTimer()
    {
        // Events nach Delay sortieren
        List<TimedEvent> sortedEvents = new List<TimedEvent>(events);
        sortedEvents.Sort((a, b) => a.delay.CompareTo(b.delay));

        // Tatsächlichen Startzeitpunkt merken
        float startTime = Time.time;

        foreach (TimedEvent timedEvent in sortedEvents)
        {
            // Bis zum absoluten Zielzeitpunkt warten
            float targetTime = startTime + timedEvent.delay;

            while (Time.time < targetTime)
            {
                yield return null;
            }

            timedEvent.onEvent?.Invoke();
        }

        timerCoroutine = null;
    }


    public void StopGame()
    {
        ResetAnimations();

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }
}