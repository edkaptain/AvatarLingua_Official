using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    public List<AudioClip> audioClips = new List<AudioClip>();
    private Queue<AudioClip> soundQueue = new Queue<AudioClip>();
    private Coroutine queueCoroutine;

    private void OnValidate()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;
    }

    public void PlaySound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            Debug.LogWarning("Sound name is null or empty.");
            return;
        }

        string path = $"Sounds/{soundName}";
        AudioClip clip = Resources.Load<AudioClip>(path);


        if (clip == null)
        {
            Debug.LogError($"Sound '{soundName}' not found in Resources folder.");
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    public void ClickPlay()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[0]);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        soundQueue.Enqueue(clip);

        if(queueCoroutine == null)
        {
            queueCoroutine = StartCoroutine(ProcessSoundQueue());
        }
    }

    private IEnumerator ProcessSoundQueue()
    {
        while(soundQueue.Count > 0)
        {
            AudioClip nextClip = soundQueue.Dequeue();

            audioSource.clip = nextClip;
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        queueCoroutine = null;
    }

}
