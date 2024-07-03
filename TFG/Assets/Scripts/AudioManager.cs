using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioSource _audioSource;

    private void Awake()
    {
        if (_audioSource != null)
        {
            Debug.LogWarning("Duplicate AudioManager detected, destroying this instance.");
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                Debug.LogError("No AudioSource component found on " + gameObject.name);
            }
            else
            {
                Debug.Log("AudioManager initialized with AudioSource.");
            }
        }
    }

    public void PlayMusic()
    {
        if (_audioSource == null)
        {
            Debug.LogError("Cannot play music, AudioSource is null.");
            return;
        }

        if (_audioSource.isPlaying)
        {
            Debug.Log("Music is already playing.");
            return;
        }

        _audioSource.Play();
        Debug.Log("Music started playing.");
    }

    public void StopMusic()
    {
        if (_audioSource == null)
        {
            Debug.LogError("Cannot stop music, AudioSource is null.");
            return;
        }

        _audioSource.Stop();
        Debug.Log("Music stopped.");
    }
}
