using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audio;
    public AudioClip[] clips;
    private void Awake()
    {
        var obj = FindObjectsOfType<AudioManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        
        StartMusic(clips[0]);
    }

    public void StartMusic(AudioClip clip)
    {
        _audio.clip = clip;
        _audio.Play();
        _audio.DOFade(1f, 3f);
    }

    public IEnumerator ChangeMusic1()
    {
        EndMusic();
        yield return new WaitForSeconds(2f);
        StartMusic(clips[0]);
    }
    
    public IEnumerator ChangeMusic2()
    {
        EndMusic();
        yield return new WaitForSeconds(2f);
        StartMusic(clips[1]);
    }

    public void EndMusic()
    {
        if (_audio.isPlaying)
        {
            _audio.DOFade(0f, 2f);
        }
    }
}
