using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class AudioAgent : MonoBehaviour
{
    public AudioClip[] AudioClips;

    //Agent's local volume
    public float AgentSEVolume = 1f;
    public float AgentBGVolume = 1f;

    public class AudioPlayer
    {
        public AudioPlayer(AudioSource _source) { isSoundEffect = false; source = _source; }
        public bool isSoundEffect { get; set; }
        public AudioSource source { get; private set; }

        public float volume = 1.0f; //Local volume to the player
    }

    public Dictionary<string, AudioPlayer> AudioLibrary { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        AudioLibrary = new Dictionary<string, AudioPlayer>();
        for (int i = 0; i < AudioClips.Length; i++)
        {
            InitialiseAudio(AudioClips[i].name, AudioClips[i]);
        }
    }

    private void Update()
    {
        foreach (var item in AudioLibrary)
        {
            if (item.Value.isSoundEffect)
                item.Value.source.volume = GetSoundEffectVolume() * item.Value.source.volume * AgentSEVolume;
            else
                item.Value.source.volume = GetBackgroundVolume() * item.Value.source.volume * AgentBGVolume;
        }
    }
    private void InitialiseAudio(string title, AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.playOnAwake = false;
        source.volume = 1.0f;
        source.priority = 255;

        AudioLibrary.Add(title, new AudioPlayer(source));
    }

    public bool PlaySoundEffect(string title, bool isLooping = false, int priority = 255)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            AudioLibrary[title].source.loop = isLooping;
            AudioLibrary[title].source.priority = priority;
            AudioLibrary[title].isSoundEffect = true;
            AudioLibrary[title].source.Play();
            return true;
        }
        return false;
    }
    public bool PlayBackground(string title, bool isLooping = false, int priority = 255)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            AudioLibrary[title].source.loop = isLooping;
            AudioLibrary[title].source.priority = priority;
            AudioLibrary[title].isSoundEffect = false;
            AudioLibrary[title].source.Play();
            return true;
        }
        return false;
    }

    public bool SetLocalVolume(string title, float volume)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            AudioLibrary[title].volume = volume;
            return true;
        }
        return false;
    }

    public bool SetPitch(string title, float pitch)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            player.source.pitch = pitch;
            return true;
        }
        return false;
    }

    public bool IsAudioStopped(string title)
    {
        AudioPlayer player;

        if (AudioLibrary.TryGetValue(title, out player))
        {
            return !player.source.isPlaying;
        }
        Debug.LogError($"StopAudio failed because audio source was not found.");
        return true;
    }

    public void StopAudio(string title)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            player.source.Stop();
            return;
        }
        Debug.LogError($"StopAudio failed because audio source was not found.");
    }

    public void StopAllAudio()
    {
        foreach (var item in AudioLibrary)
        {
            item.Value.source.Stop();
        }
    }

    private float GetSoundEffectVolume()
    {
        return GameManager.MasterVolume * GameManager.SoundEffectVolume;
    }

    private float GetBackgroundVolume()
    {
        return GameManager.MasterVolume * GameManager.BackGroundVolume;
    }
}
