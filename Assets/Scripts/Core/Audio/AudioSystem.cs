using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

[Serializable] // to draw in editor
public class AudioListBinding // model
{
    public string SceneName;
    public List<IdAudio> IdAudios;
}

public class AudioSystem : Singleton<AudioSystem>, ISystem
{
    [SerializeField]
    private int _Priority;
    public int Priority { get => _Priority; }

    [SerializeField]
    private IdAudioSystemData _IdAudioSystemData;

    [SerializeField]
    private AudioMixer _AudioMixer;
    [SerializeField]
    private AudioSource _MusicSource;
    [SerializeField]
    private AudioSource _EffectsSource;

    public const string MASTER_KEY = "MasterVolume";
    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";
    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    // corrispondence scene <-> audio
    private Dictionary<string, List<IdAudio>> _sceneAudioClipDict;

    // id -> AudioClip
    private Dictionary<string, AudioClip> _currentEffectAudioDict;
    private Dictionary<string, AudioClip> _currentMusicAudioDict;

    private bool _isLoadingDone;
    private int _countAudioClipReady;

    [SerializeField]
    private float _minPitchValue;
    [SerializeField]
    private float _maxPitchValue;
    public void PlaySound(string idClip)
    {
        if (!_currentEffectAudioDict.ContainsKey(idClip))
        {
            Debug.LogError("UNLOADED");
            return;
        }
        _EffectsSource.pitch = 1;
        _EffectsSource.PlayOneShot(_currentEffectAudioDict[idClip]);
    }
    public void PlayMusic(string idClip)
    {
        if (!_currentMusicAudioDict.ContainsKey(idClip))
        {
            Debug.LogError("UNLOADED");
            return;
        }
        _MusicSource.pitch = 1;
        _MusicSource.clip = _currentMusicAudioDict[idClip];
        _MusicSource.Play();
    }

    public void PlaySoundWithPitch(string idClip, float velocity, float minVelocity, float maxVelocity)
    {
        if (!_currentEffectAudioDict.ContainsKey(idClip))
        {
            Debug.LogError("UNLOADED");
            return;
        }
        _EffectsSource.pitch = mapValue(velocity, minVelocity, maxVelocity, _minPitchValue, _maxPitchValue);
        _EffectsSource.PlayOneShot(_currentEffectAudioDict[idClip]);
    }
    public void PlayMusicWithPitch(string idClip, float velocity, float minVelocity, float maxVelocity)
    {
        if (!_currentMusicAudioDict.ContainsKey(idClip))
        {
            Debug.LogError("UNLOADED");
            return;
        }
        _MusicSource.pitch = mapValue(velocity, minVelocity, maxVelocity, _minPitchValue, _maxPitchValue);
        _MusicSource.clip = _currentMusicAudioDict[idClip];
        _MusicSource.Play();
    }
    public void ChangeMusicPitch(float velocity, float minVelocity, float maxVelocity)
    {
        _MusicSource.pitch = mapValue(velocity, minVelocity, maxVelocity, _minPitchValue, _maxPitchValue);
    }
    public void ChangeEffectPitch(float velocity, float minVelocity, float maxVelocity)
    {
        _EffectsSource.pitch = mapValue(velocity, minVelocity, maxVelocity, _minPitchValue, _maxPitchValue);
    }
    public void StopMusic()
    {
        _MusicSource.Stop();
    }

    public void Setup()
    {
        _minPitchValue = 1f;
        _maxPitchValue = 1.5f;
        _isLoadingDone = true;
        _currentEffectAudioDict = new Dictionary<string, AudioClip>();
        _currentMusicAudioDict = new Dictionary<string, AudioClip>();
        _sceneAudioClipDict = new Dictionary<string, List<IdAudio>>();
        foreach (AudioListBinding binding in _IdAudioSystemData.IdAudioBindings)
            _sceneAudioClipDict.Add(binding.SceneName, new List<IdAudio>(binding.IdAudios)); // new list because scriptable objects can be modified at runtime and the new changes remain

        SystemsCoordinator.Instance.SystemReady(this);
    }
    // setup the pool managers for a target scene
    public void SetupAudioClipForScene(string sceneTarget) // called by the flow system
    {
        LoadVolume();
        _isLoadingDone = false;
        if (!_sceneAudioClipDict.ContainsKey(sceneTarget))
        {
            _isLoadingDone = true;
            return;
        }

        // clear current effects and music
        _currentEffectAudioDict.Clear();
        _currentMusicAudioDict.Clear();

        List<IdAudio> requestedAudio = _sceneAudioClipDict[sceneTarget];
        foreach (IdAudio idAudio in requestedAudio) // this manager is a prefab, we need to instantiate it
        {
            if (!idAudio.IsMusic)
            {
                _currentEffectAudioDict.Add(idAudio.Id.Id, idAudio.AudioClip);
            }
            else
            {
                _currentMusicAudioDict.Add(idAudio.Id.Id, idAudio.AudioClip);
            }
        }
    }
    private float mapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax)
    {
        return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
    }

    private void LoadVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        _AudioMixer.SetFloat(MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
        _AudioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        _AudioMixer.SetFloat(MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}
