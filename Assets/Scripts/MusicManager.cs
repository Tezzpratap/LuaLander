using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance { get; private set; }

	private const float MUSIC_VOLUME_MAX = 10f;

	private static float musicTime;
    private static float musicVolume = 4f;

    private AudioSource musicAudioSource;

	public static event EventHandler OnMusicVolumeChanged;


	private void Awake()
    {	
	Instance = this;
		musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
	}

	private void Start(){
		musicAudioSource.volume = GetMusicVolumeNormalized();
	}


	private void Update(){
        musicTime = musicAudioSource.time;
	}

	public void ChangeMusicVolume()
	{
		musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
		musicAudioSource.volume = GetMusicVolumeNormalized();
		OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
	}

	public float GetMusicVolume()
	{
		return musicVolume;
	}

	public float GetMusicVolumeNormalized()
	{
		return ((float)musicVolume) / MUSIC_VOLUME_MAX;
	}
}
