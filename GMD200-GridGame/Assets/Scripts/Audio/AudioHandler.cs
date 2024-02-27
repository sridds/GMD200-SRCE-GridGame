using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler : MonoBehaviour
{
    public enum FadeMode
    {
        None,
        Fade,
        CrossFade
    }

    public static AudioHandler instance;

    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundPrefab;

    [Header("Mix")]
    [SerializeField] private AudioMixerGroup musicMix;

    // Holds a queue of music volume actions
    private Queue<IEnumerator> musicCoroutineQueue = new Queue<IEnumerator>();
    private Coroutine activeMusicCoroutine = null;

    private float prePauseVolume;

    /// <summary>
    /// Sets up the instance
    /// </summary>
    private void Awake() => instance = this;

    private void Update()
    {
        UpdateMusicQueue();
    }

    private void UpdateMusicQueue()
    {
        // Continue going through the queue of music actions
        if (musicCoroutineQueue.Count > 0 && activeMusicCoroutine == null)
        {
            activeMusicCoroutine = StartCoroutine(musicCoroutineQueue.Dequeue());
        }
    }

    /// <summary>
    /// Changes the currently active music track.
    /// </summary>
    /// <param name="newTrack"></param>
    /// <param name="playAutomatically"></param>
    public void ChangeTrack(AudioClip newTrack, bool playAutomatically = false)
    {
        // Pauses the current music track and sets the clip to the new track
        musicSource.Pause();
        musicSource.clip = newTrack;

        // Play the music source if specified to play automatically
        if (playAutomatically) musicSource.Play();
    }

    /// <summary>
    /// Fades out current music track and fades in new music track
    /// </summary>
    /// <param name="newTrack"></param>
    /// <param name="fadeInTime"></param>
    /// <param name="fadeOutTime"></param>
    public void FadeChangeTrack(AudioClip newTrack, float fadeInTime, float fadeOutTime, float targetVolume) => musicCoroutineQueue.Enqueue(IFadeChangeTrack(newTrack, fadeInTime, fadeOutTime, targetVolume));

    /// <summary>
    /// Fades two music tracks at the same time
    /// </summary>
    /// <param name="newTrack"></param>
    /// <param name="fadeInTime"></param>
    /// <param name="fadeOutTime"></param>
    public void CrossFadeTrack(AudioClip newTrack, float fadeInTime, float fadeOutTime, float targetVolume) => musicCoroutineQueue.Enqueue(ICrossFadeTrack(newTrack, fadeInTime, fadeOutTime, targetVolume));

    /// <summary>
    /// Fades out the track that is currently playing
    /// </summary>
    /// <param name="time"></param>
    public void FadeMusic(float time, float volume) => musicCoroutineQueue.Enqueue(IFadeToVolume(musicSource, time, volume, true));

    public void FadeToPitch(float time, float pitch, bool useUnscaled = false) => StartCoroutine(IFadeToPitch(time, pitch, useUnscaled));


    private IEnumerator IFadeToPitch(float time, float pitch, bool useUnscaled)
    {
        float elapsed = 0.0f;
        float initial = musicSource.pitch;

        // lerp pitch
        while(elapsed < time)
        {
            musicSource.pitch = Mathf.Lerp(initial, pitch, elapsed / time);

            if (useUnscaled) elapsed += Time.unscaledDeltaTime;
            else elapsed += Time.deltaTime;

            yield return null;
        }

        musicSource.pitch = pitch;
    }


    /// <summary>
    /// Fades out the current music track and fades into the next one sequentially
    /// </summary>
    /// <param name="newTrack"></param>
    /// <param name="fadeInTime"></param>
    /// <param name="fadeOutTime"></param>
    /// <returns></returns>
    private IEnumerator IFadeChangeTrack(AudioClip newTrack, float fadeInTime, float fadeOutTime, float targetVolume)
    {
        // Fade out
        yield return StartCoroutine(IFadeToVolume(musicSource, fadeOutTime, 0.0f));
        ChangeTrack(newTrack, true);
        // Fade back in
        yield return StartCoroutine(IFadeToVolume(musicSource, fadeInTime, targetVolume));

        activeMusicCoroutine = null;
    }

    AudioSource tempSource = null;

    /// <summary>
    /// Creates a temporary music track to cross fade between the current music track and the new music track
    /// </summary>
    /// <param name="newTrack"></param>
    /// <param name="fadeInTime"></param>
    /// <param name="fadeOutTime"></param>
    /// <returns></returns>
    private IEnumerator ICrossFadeTrack(AudioClip newTrack, float fadeInTime, float fadeOutTime, float targetVolume)
    {
        GameObject go = new GameObject("Temp_MusicSource");
        tempSource = go.AddComponent<AudioSource>();

        // Fade out the music source
        StartCoroutine(IFadeToVolume(musicSource, fadeOutTime, 0.0f));

        // Play the temp
        tempSource.volume = 0.0f;
        tempSource.clip = newTrack;
        tempSource.outputAudioMixerGroup = musicMix;
        tempSource.Play();

        // Fade in the temp track
        StartCoroutine(IFadeToVolume(tempSource, fadeInTime, targetVolume));

        yield return new WaitForSeconds(fadeInTime + fadeOutTime);

        // After the wait, setup the music source to have the same parameters as the temp]
        musicSource.volume = targetVolume;
        musicSource.clip = newTrack;
        musicSource.time = tempSource.time;

        // Destroy the temporary music source and play music source
        Destroy(go);
        musicSource.Play();

        activeMusicCoroutine = null;
        tempSource = null;
    }

    /// <summary>
    /// Fades audio track to specified volume
    /// </summary>
    /// <param name="source"></param>
    /// <param name="time"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    private IEnumerator IFadeToVolume(AudioSource source, float time, float volume, bool setMusicCoroutineNull = false)
    {
        float elapsedTime = 0.0f;
        float initialVolume = source.volume;

        // Fades to the target volume
        while (elapsedTime < time)
        {
            source.volume = Mathf.Lerp(initialVolume, volume, elapsedTime / time);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        source.volume = volume;

        // This is here to specify whether this coroutine was called within the queue as an independent action or called from another coroutine.
        if (setMusicCoroutineNull) activeMusicCoroutine = null;
    }

    /// <summary>
    /// Processes audio data and plays according to the specified AudioData settings
    /// </summary>
    /// <param name="data"></param>
    public void ProcessAudioData(AudioData data)
    {
        // creates a new audio instance

        //GameObject go = ObjectPooler.SpawnObject(soundPrefab.gameObject, data.spawnPosition, Quaternion.identity, ObjectPooler.PoolType.AudioSource);
        AudioSource source = Instantiate(soundPrefab, data.spawnPosition, Quaternion.identity);
        //AudioSource source = go.GetComponent<AudioSource>();

        // Set volume
        source.volume = data.volume;

        // Randomize pitch if data wants to
        source.pitch = data.randomizePitch ? Random.Range(data.minPitch, data.maxPitch) : data.minPitch;
        source.clip = data.clip;

        // Play
        source.Play();
    }
}

[System.Serializable]
public class AudioData
{
    public AudioClip clip;

    [Header("Properties")]
    [Range(0, 1)] public float volume;

    [Header("Pitch")]
    [Tooltip("If set to false, the minPitch will be the default pitch")]
    public bool randomizePitch;

    [Range(-3, 3)] public float minPitch;

    [ShowIf(nameof(randomizePitch))]
    [AllowNesting]
    [Range(-3, 3)] public float maxPitch;

    [HideInInspector]
    public Vector3 spawnPosition;
}