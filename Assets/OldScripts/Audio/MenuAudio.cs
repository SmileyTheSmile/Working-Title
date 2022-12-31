using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    [SerializeField] private AudioSourcePlayer _buttonClickSound;
    [SerializeField] private AudioSourcePlayer _buttonSelectSound;
    [SerializeField] private AudioSourcePlayer _sliderChangedSound;

    public void PlayButtonClickSound()
    {
        _buttonClickSound.Play();
    }

    public void PlayButtonSelectSound()
    {
        _buttonSelectSound.Play();
    }

    public void PlaySliderValueChangedSound(float value)
    {
        _sliderChangedSound.SetPitch(value / 2 + 0.5f);
        _sliderChangedSound.Play();
    }
    
    public void SetVolume(float value)
    {
        _buttonClickSound.SetVolume(value);
        _buttonSelectSound.SetVolume(value);
        _sliderChangedSound.SetVolume(value);
    }
}