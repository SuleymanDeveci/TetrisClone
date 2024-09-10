using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Dictionary<string,AudioSource> AudiosDictionary = new Dictionary<string,AudioSource>();
    [SerializeField] private AudioSource lockSound;
    [SerializeField] private AudioSource rotateSound;
    [SerializeField] private AudioSource lineClearSound;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource gameOverSound;

    private void Start()
    {
        // bir dictionary tan�mlay�p t�m sesleri dic icerisine att�m bu sayede �almak istenen sese d��ar�dan eri�mek yerine PlaySound() methodu ile �alacaklar (single responsibility)
        AudiosDictionary.Clear();
        AudiosDictionary.Add("lockSound", lockSound);
        AudiosDictionary.Add("rotateSound", rotateSound);
        AudiosDictionary.Add("lineClearSound", lineClearSound);
        AudiosDictionary.Add("clickSound", clickSound);
        AudiosDictionary.Add("gameOverSound", gameOverSound);
    }

    public void PlaySound(string key)
    {
        if(AudiosDictionary.ContainsKey(key))
        {
            AudiosDictionary[key].Play();
        }
    }


}
