using UnityEngine;
using TMPro;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
	[SerializeField] private ParticleSystem middleThrusterParticleSystem;
	[SerializeField] private ParticleSystem rightThrusterParticleSystem;
    [SerializeField] private GameObject landerExplosionVFX;

    private LanderController lander;

	private void Awake(){
		lander = GetComponent<LanderController>();

		lander.OnUpForce += Lander_OnUpForce;
		lander.OnLeftForce += Lander_OnLeftForce;
		lander.OnRightForce += Lander_OnRightForce;
        lander.OnDownForce += Lander_OnRightForce;
		lander.OnThrustStop += Lander_OnThrustStop;

		SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
		SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
		SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
		//SetEnabledThrusterParticleSystem(StopThrusterParticleSystem, false);

	}

	private void State() {
		lander.OnLanded += Lander_OnLanded;
    }

	private void Lander_OnLanded(object sender, LanderController.OnLandedEventArgs e)
	{
		switch (e.landingTypes)
		{
			case LanderController.LandingTypes.Success:
			case LanderController.LandingTypes.WrongLanding:
			case LanderController.LandingTypes.TooSteepAngle:
			case LanderController.LandingTypes.TooFastLanding:
			Instantiate(landerExplosionVFX, transform.position, Quaternion.identity);
				gameObject.SetActive(false);
			break;
        }
    }


    private void Lander_OnUpForce(object sender, System.EventArgs e){
		SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
		SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, true);
		SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
	}

	private void Lander_OnLeftForce(object sender, System.EventArgs e)
	{
		SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
		SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
		SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
	}

	private void Lander_OnRightForce(object sender, System.EventArgs e)
	{
		SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
		SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
		SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
	}

	private void Lander_OnThrustStop(object sender, System.EventArgs e)
	{
		SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
		SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
		SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
	}

	private void SetEnabledThrusterParticleSystem(ParticleSystem partcleSystem, bool enabled){
		ParticleSystem.EmissionModule emissionModule = partcleSystem.emission;
		emissionModule.enabled = enabled;
	}
}