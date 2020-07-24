using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public static AudioController instance = null;

    public static List<AudioClip> audioClips;
    static public AudioSource fxsSource;
    static public AudioSource musicSource;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }

        // THE 1st AUDIOSOURCE COMPONENT IS FOR THE AMBIENT MUSIC, the 2nd FOR FXs
        fxsSource = GetComponents<AudioSource>()[1];
        //Debug.Log(GetComponents<AudioSource>());
        audioClips = GetComponent<AudioPool>().audioClips;

        musicSource = GetComponents<AudioSource>()[0];


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void playConnectorPrompt()
    {
        fxsSource.PlayOneShot(audioClips[6]);
    }

    static public void playConnectorEnter()
    {
        fxsSource.PlayOneShot(audioClips[7]);
    }

    static public void playPlayerSpawn()
    {
        fxsSource.PlayOneShot(audioClips[0]);
    }

    static public void playPlayerBump()
    {
        fxsSource.PlayOneShot(audioClips[2]);
    }
    static public void playPlayerLight()
    {
        fxsSource.PlayOneShot(audioClips[1]);
    }
    static public void playPinGizmo()
    {
        fxsSource.PlayOneShot(audioClips[3]);
    }
    static public void playPinRead()
    {
        fxsSource.PlayOneShot(audioClips[4]);
    }
    static public void playPinWrite()
    {
        fxsSource.PlayOneShot(audioClips[5]);
    }

    static public void switchAmbientMusic(bool state)
    {
        if (state)
        {
            musicSource.UnPause();
        }
        else
        {
            musicSource.Pause();
        }
    }
}
