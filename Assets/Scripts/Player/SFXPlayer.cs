using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public List<AudioClip> listAudioClip;
    //0: Sonido de la Moneda
    //1: Sonido de Disparo
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Funcion para especificar y reproducir un audioclip
    void ChangeAudioClip(AudioClip audio)
    {
        audioSource.Stop();//Se puede omitir
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void PlayCoinCollectAudio()
    {
        ChangeAudioClip(listAudioClip[0]);
    }

    public void PlayPlayerJump()
    {
        ChangeAudioClip(listAudioClip[1]);
    }


    public void PlayPlayerRun()
    {
        ChangeAudioClip(listAudioClip[2]);
    }
    public void PlayPlayerHit()
    {
        ChangeAudioClip(listAudioClip[3]);
    }
    public void PlayPlayerDeath()
    {
        ChangeAudioClip(listAudioClip[4]);
    }
    public void PlayPlayerAtack()
    {
        ChangeAudioClip(listAudioClip[5]);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
