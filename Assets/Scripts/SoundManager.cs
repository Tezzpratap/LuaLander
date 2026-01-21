using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance { get; private set; }

	private static int soundVolume = 5;
	private static int SOUND_VOLUME_MAX = 10;

	public event EventHandler OnSoundVolumeChanged;

	[SerializeField] private AudioClip fuelPickupAudioClip;
	[SerializeField] private AudioClip coinPickupAudioClip;
	[SerializeField] private AudioClip landingAudioClip;
	[SerializeField] private AudioClip crashAudioClip;


	private void Awake(){
		Instance = this;
	}

	private void Start(){
        LanderController.Instance.OnFuelPickup += Lander_OnFuelPickup;
        LanderController.Instance.OnCoinPickup += Lander_OnCoinPickup;
		LanderController.Instance.OnLanded += Lander_OnLanded;
	}

	private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupAudioClip, Camera.main.transform.position , GetSoundVolumeNormalized());
	}

	private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
	}

	private void Lander_OnLanded(object sender, LanderController.OnLandedEventArgs e)
	{
		switch(e.landingTypes){
			case LanderController.LandingTypes.Success:
				AudioSource.PlayClipAtPoint(landingAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
				break;
			default:
				AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
				break;
		}
	}

	public void PlayCrashSound(){
		AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position);
	}

	public void ChangeSoundVolume(){
		soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
		OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
	}

	public int GetSoundVolume(){
		return soundVolume;
	}

	public float GetSoundVolumeNormalized(){
		return ((float) soundVolume) / SOUND_VOLUME_MAX;
	}
}
