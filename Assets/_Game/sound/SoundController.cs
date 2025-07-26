
using System.Collections.Generic;
using UnityEngine;


public class SoundController : Singleton<SoundController>
{
    [SerializeField] private List<Sounds> sounds;
    [SerializeField] private List<Sounds> musics;


    private void Start()
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i].audio = gameObject.AddComponent<AudioSource>();
            sounds[i].audio.clip = sounds[i].clip;
            sounds[i].audio.priority = 1;
            sounds[i].audio.playOnAwake = false;
            sounds[i].audio.volume = sounds[i].volume;
            //  sounds[i].audio.mute = !IsSound();
        }

        for (int i = 0; i < musics.Count; i++)
        {
            musics[i].audio = gameObject.AddComponent<AudioSource>();
            musics[i].audio.clip = musics[i].clip;
            musics[i].audio.playOnAwake = false;
            musics[i].audio.volume = musics[i].volume;
            //   musics[i].audio.mute = !IsMusic();
        }
        PlayMusic(SoundName.Music, loop: true, 0.4f);
    }



    public void PlaySound(SoundName name, bool isUsePitch = false)
    {
        if (!IsSound())
        {
            return;
        }


        Sounds sound = sounds.Find(x => x.name == name);
        if (isUsePitch && sound != null)
        {
            sound.audio.priority = 1;
            sound.audio.pitch = Random.Range(0.8f, 1.2f);
        }
        sound?.audio.Play();
    }

    public void PlayMusic(SoundName name, bool loop = true, float volume = 1f)
    {
        if (!IsSound())
        {
            return;
        }

        Sounds music = musics.Find(x => x.name == name);
        if (music != null)
        {
            music.volume = volume;
            music.audio.priority = 128;
            music.audio.loop = loop;
            music.audio.Play();
        }
    }

    public void StopSound(SoundName name)
    {
        Sounds sound = sounds.Find(x => x.name == name);
        sound?.audio.Stop();
    }

    public void StopMusic(SoundName name)
    {
        Sounds music = musics.Find(x => x.name == name);
        music?.audio.Stop();
    }

    public void SoundUpdate(bool mute)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i].audio.mute = mute;
        }
        MusicUpdate(mute);
    }

    public void MusicUpdate(bool mute)
    {
        for (int i = 0; i < musics.Count; i++)
        {
            musics[i].audio.mute = mute;
        }
    }

    public bool IsSound()
    {
        return PlayerPrefs.GetInt("Sound",1) == 1;
    }
    /*
       public bool IsMusic()
       {
           return DBController.Instance.USER_SETTINGS.music;
       }

       public void SettingSound(bool state)
       {
           DBController.Instance.USER_SETTINGS.SetSound(state);
           SoundUpdate(!DBController.Instance.USER_SETTINGS.sound);
       }

       public void SettingMusic(bool state)
       {
           DBController.Instance.USER_SETTINGS.SetMusic(state);
           MusicUpdate(!DBController.Instance.USER_SETTINGS.music);
       }*/
}

[System.Serializable]
public class Sounds
{
    public SoundName name;
    [Range(0, 1)]
    public float volume = 1;
    public AudioClip clip;
    [HideInInspector] public AudioSource audio;
}

public enum SoundName
{
    Music = 0,
    Click = 1,
    Up = 2,
    Down = 3,
    LevelComplete = 4,
    Fail = 5,
    EnemyDie = 6,
}