using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioClip;

    private LanderController lander;

    private void Awake()
    {
        lander = GetComponent<LanderController>();
	}

    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        //lander.OnDownForce += Lander_OnThrust;
        thrusterAudioClip.Pause();

        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
	}

    private void SoundManager_OnSoundVolumeChanged(object sender, System.EventArgs e)
    {
        thrusterAudioClip.volume = SoundManager.Instance.GetSoundVolumeNormalized();
    }


	private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioClip.isPlaying){
            thrusterAudioClip.Pause();
        }
	}
	private void Lander_OnUpForce(object sender, System.EventArgs e)
	{
		if (!thrusterAudioClip.isPlaying){
			thrusterAudioClip.Play();
        }
	}
	private void Lander_OnLeftForce(object sender, System.EventArgs e)
	{
		if (!thrusterAudioClip.isPlaying){
			thrusterAudioClip.Play();
        }
	}
	private void Lander_OnRightForce(object sender, System.EventArgs e)
	{
		if (!thrusterAudioClip.isPlaying){
			thrusterAudioClip.Play();
        }
	}

	

	/*private void Lander_OnThrust(object sender, System.EventArgs e)
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = thrusterAudioClip;
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
    }
    private void Lander_OnThrustStop(object sender, System.EventArgs e)
    {
        GetComponent<AudioSource>().Stop();
	}*/

}
