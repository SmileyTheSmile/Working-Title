using UnityEngine;

public class AudioSourcePlayer : MonoBehaviour
{
    public bool IsPlaying => _audioSource.isPlaying;
    
    [SerializeField] private AudioSource _audioSource;

    public void Play() => _audioSource.Play();
    public void PlayOneShot() => _audioSource.PlayOneShot(_audioSource.clip);
    public void Stop() => _audioSource.Stop();
    public void SetVolume(float value) => _audioSource.volume = value;
    public void SetPitch(float value) => _audioSource.pitch = value;
}