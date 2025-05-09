using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Slider musicSlider;
    // public Slider sfxSlider;
    // public Toggle fullscreenToggle;

    public float musicVolume = 0.5f;

    [SerializeField] AudioSource[] allAudioSource;

    void Start()
    {
        // Set nilai awal dari static variables ke UI elements
        // sfxSlider.value = GameSettings.sfxVolume;
        // fullscreenToggle.isOn = GameSettings.isFullscreen;
        PressSettingsButton();
        PressConfirmButton();
    }

    public void OnMusicVolumeChanged(float volume)
    {
        musicVolume = volume;
        Debug.Log("Music Volume: " + musicVolume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        GameSettings.sfxVolume = volume;
        Debug.Log("SFX Volume: " + GameSettings.sfxVolume);
    }

    public void OnFullscreenToggled(bool isFullScreen)
    {
        GameSettings.isFullscreen = isFullScreen;
        Debug.Log("Fullscreen: " + GameSettings.isFullscreen);
        // Anda mungkin perlu menerapkan perubahan fullscreen di sini atau di scene lain
    }

    public void PressConfirmButton()
    {
        GameSettings.musicVolume = musicVolume;

        foreach (AudioSource audio in allAudioSource)
        {
            audio.volume = musicVolume;
        }
    }

    public void PressSettingsButton()
    {
        musicSlider.value = GameSettings.musicVolume;
    }


}
