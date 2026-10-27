using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioLoop
    {
        [Header("Audio")]
        public AudioClip clip;

        [Header("Settings")]
        [Range(0f, 1f)]
        public float referenceVolume = 1f;

        [Min(0f)]
        public float fadeInTime = 1f;

        [Min(0f)]
        public float fadeOutTime = 1f;

        public bool isLooping = true;

        public bool keepPlayingWhenNewLoopTriggered = false;
    }

    [Header("Audio Loops")]
    public AudioLoop[] audioLoops;

    private List<PlayingLoop> playingLoops = new List<PlayingLoop>();


    private class PlayingLoop
    {
        public AudioLoop settings;
        public AudioSource source;
    }


    public void PlayLoop(int index)
    {
        if (index < 0 || index >= audioLoops.Length)
        {
            Debug.LogWarning($"AudioLoop Index {index} existiert nicht.");
            return;
        }

        AudioLoop newLoop = audioLoops[index];

        if (newLoop.clip == null)
        {
            Debug.LogWarning($"AudioLoop {index} hat keinen AudioClip.");
            return;
        }


        // Alle bisherigen Loops überprüfen
        for (int i = playingLoops.Count - 1; i >= 0; i--)
        {
            PlayingLoop playingLoop = playingLoops[i];

            if (playingLoop.source == null)
            {
                playingLoops.RemoveAt(i);
                continue;
            }

            // Darf dieser Loop weiterlaufen?
            if (playingLoop.settings.keepPlayingWhenNewLoopTriggered)
            {
                continue;
            }

            // Nein -> Fade Out
            StartCoroutine(
                FadeOutAndDestroy(
                    playingLoop
                )
            );

            playingLoops.RemoveAt(i);
        }


        // Neuen Loop starten
        StartNewLoop(newLoop);
    }


    private void StartNewLoop(AudioLoop loop)
    {
        // Neue AudioSource erstellen
        GameObject audioObject = new GameObject(
            "AudioLoop_" + loop.clip.name
        );

        audioObject.transform.SetParent(transform);


        AudioSource source = audioObject.AddComponent<AudioSource>();

        source.clip = loop.clip;
        source.loop = loop.isLooping;
        source.playOnAwake = false;
        source.volume = 0f;


        // AudioSource starten
        source.Play();


        // In aktive Loops aufnehmen
        PlayingLoop playingLoop = new PlayingLoop();

        playingLoop.settings = loop;
        playingLoop.source = source;

        playingLoops.Add(playingLoop);


        // Fade In
        StartCoroutine(
            FadeIn(
                source,
                loop.referenceVolume,
                loop.fadeInTime
            )
        );


        // Wenn es KEIN Loop ist, warten wir auf das Ende
        if (!loop.isLooping)
        {
            StartCoroutine(
                WaitForClipEnd(playingLoop)
            );
        }
    }


    private IEnumerator WaitForClipEnd(PlayingLoop playingLoop)
    {
        AudioSource source = playingLoop.source;

        if (source == null)
            yield break;


        // Warten, bis der Clip fertig ist
        yield return new WaitWhile(() =>
            source != null && source.isPlaying
        );


        if (source == null)
            yield break;


        // Aus Liste entfernen
        playingLoops.Remove(playingLoop);


        // AudioSource löschen
        Destroy(source.gameObject);
    }


    public void StopLoop()
    {
        print("Stopping all loops...");
        for (int i = playingLoops.Count - 1; i >= 0; i--)
        {
            PlayingLoop playingLoop = playingLoops[i];

            if (playingLoop.source == null)
                continue;

            StartCoroutine(
                FadeOutAndDestroy(
                    playingLoop

                )
            );
            print($"Stopped loop: {playingLoop.settings.clip.name}");
        }

        playingLoops.Clear();
    }


    private IEnumerator FadeIn(
        AudioSource source,
        float targetVolume,
        float duration
    )
    {
        if (source == null)
            yield break;


        if (duration <= 0f)
        {
            source.volume = targetVolume;
            yield break;
        }


        float elapsed = 0f;


        while (elapsed < duration)
        {
            if (source == null)
                yield break;

            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            source.volume = Mathf.Lerp(
                0f,
                targetVolume,
                t
            );

            yield return null;
        }


        if (source != null)
        {
            source.volume = targetVolume;
        }
    }


    private IEnumerator FadeOutAndDestroy(
        PlayingLoop playingLoop
    )
    {
        AudioSource source = playingLoop.source;

        if (source == null)
            yield break;


        float duration = playingLoop.settings.fadeOutTime;


        if (duration <= 0f)
        {
            source.Stop();
            Destroy(source.gameObject);
            yield break;
        }


        float startVolume = source.volume;
        float elapsed = 0f;


        while (elapsed < duration)
        {
            if (source == null)
                yield break;

            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            source.volume = Mathf.Lerp(
                startVolume,
                0f,
                t
            );

            yield return null;
        }


        if (source != null)
        {
            source.Stop();
            Destroy(source.gameObject);
        }
    }
}
