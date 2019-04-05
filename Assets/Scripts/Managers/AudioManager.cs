using UnityEngine.Audio;
using System;
using UnityEngine;

public enum Sounds { Sound, SoundFX}

public class AudioManager : MonoBehaviour {

    //singleton
    public static AudioManager instance;

    public Sound[] sounds;
    [Header("Buttons")]
    public Sound[] buttonsSounds;

    //random button sound
    private int lengthOfButtons;
    private int randomButton;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.looping;
            s.source.playOnAwake = s.playOnAwake;
        }

        foreach (Sound s in buttonsSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.looping;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    private void Start()
    {
        foreach (Sound s in sounds)
        {
            if (s.playOnAwake == true)
                s.source.Play();
        }

        lengthOfButtons = buttonsSounds.Length;
        randomButton = UnityEngine.Random.Range(0,lengthOfButtons);
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => (sound.soundName == soundName)&&(sound.soundIsOn));

        if(s == null)
        {
            Debug.LogWarning(soundName + " is not included in the manager, Please check if it is a typo.");
            return;
        }

        s.source.Play();
    }

    public void PlayButton()
    {
        Sound s = buttonsSounds[randomButton];

        if (!s.soundIsOn)
            return;

        if (s == null)
        {
            Debug.LogWarning("button index is not included in the manager, Please check if it is a typo.");
            return;
        }
        s.source.Play();

        //next buton sound
        randomButton = UnityEngine.Random.Range(0, lengthOfButtons);
    }

    public void StopSound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == soundName);

        if (s == null)
        {
            Debug.LogWarning(soundName + " is not included in the manager, Please check if it is a typo.");
            return;
        }

        s.source.Stop();
    }

    public void StopFXSounds()
    {
        Sound[] s = Array.FindAll(sounds, sound => sound.soundName != "Theme Song");

        Debug.Log(s.Length);
        for (int i = 0; i < s.Length; i++)
        {
            s[i].source.Stop();
            s[i].soundIsOn = false;
        }

        for (int i = 0; i < buttonsSounds.Length; i++)
        {
            buttonsSounds[i].source.Stop();
            buttonsSounds[i].soundIsOn = false;
        }

    }

    public void PlayFXSounds()
    {
        Sound[] s = Array.FindAll(sounds, sound => sound.soundName != "Theme Song");

        for (int i = 0; i < s.Length; i++)
        {
            s[i].soundIsOn = true;
        }

        for (int i = 0; i < buttonsSounds.Length; i++)
        {
            buttonsSounds[i].soundIsOn = true;
        }

    }

}


