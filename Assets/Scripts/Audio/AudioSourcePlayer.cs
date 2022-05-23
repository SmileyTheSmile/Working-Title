using UnityEngine;

public class AudioSourcePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public bool IsPlaying => _audioSource.isPlaying;

    public void Play()
    {
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void SetVolume(float value)
    {
        _audioSource.volume = value;
    }

    public void SetPitch(float value)
    {
        _audioSource.pitch = value;
    }
}