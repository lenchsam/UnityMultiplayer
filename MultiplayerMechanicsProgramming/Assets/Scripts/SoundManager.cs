using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    AudioSource _audioSource;
    
    private AudioClip _soundToPlay
    {
		get { return _audioSource.clip; }
		set { _audioSource.clip = value; }
	}

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip soundToPlay){
        _soundToPlay = soundToPlay;
        _audioSource.Play();
    }
}
