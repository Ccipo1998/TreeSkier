using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _AudioMixer;
    [SerializeField]
    private Slider _MasterSlider;
    [SerializeField]
    private Slider _MusicSlider;
    [SerializeField]
    private Slider _SFXSlider;

    
    private void Awake()
    {
        _MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        _MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        _SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        _MasterSlider.value = PlayerPrefs.GetFloat(AudioSystem.MASTER_KEY, 1f);
        _MusicSlider.value = PlayerPrefs.GetFloat(AudioSystem.MUSIC_KEY, 1f);
        _SFXSlider.value = PlayerPrefs.GetFloat(AudioSystem.SFX_KEY, 1f);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioSystem.MASTER_KEY, _MasterSlider.value);
        PlayerPrefs.SetFloat(AudioSystem.MUSIC_KEY, _MusicSlider.value);
        PlayerPrefs.SetFloat(AudioSystem.SFX_KEY, _SFXSlider.value);
    }
    private void SetMasterVolume(float value)
    {
        _AudioMixer.SetFloat(AudioSystem.MIXER_MASTER,Mathf.Log10(value) * 20);
    }
    private void SetMusicVolume(float value)
    {
        _AudioMixer.SetFloat(AudioSystem.MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    private void SetSFXVolume(float value)
    {
        _AudioMixer.SetFloat(AudioSystem.MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
