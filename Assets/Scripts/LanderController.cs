using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LanderController : MonoBehaviour
{

	public event EventHandler OnUpForce; //event handlers for thruster effects
	public event EventHandler OnLeftForce; 
	public event EventHandler OnRightForce;
	public event EventHandler OnDownForce;
	public event EventHandler OnThrustStop;

	private Rigidbody2D landerRigidbody2D;
	private float fuelAmount = 10f;
	
	public float Thrust = 1000f;
	public float RotationThrustRight = +500f;
	public float RotationThrustLeft = -500f;
	public float softLandingSpeed = 3f;
	public float maxLandingAngle = 0.9f;

	private void Awake(){
        landerRigidbody2D = GetComponent<Rigidbody2D>();
	}


    private void FixedUpdate()
    {
		OnThrustStop?.Invoke(this, EventArgs.Empty);	

		if (fuelAmount <= 0f)
		{
			return;
		}

		if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed){
			ConsumeFuel();
		}

			if ((Keyboard.current.spaceKey.isPressed) || (Keyboard.current.upArrowKey.isPressed))
        {
            landerRigidbody2D.AddForce( Thrust * transform.up * Time.deltaTime);
			Debug.Log("Thrusting");
			OnUpForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
		}
		if (Keyboard.current.downArrowKey.isPressed)
		{
			landerRigidbody2D.AddForce(Thrust * -transform.up * Time.deltaTime);
			Debug.Log("Revers Thrusting");
			OnDownForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
		}
		if (Keyboard.current.leftArrowKey.isPressed)
		{
			landerRigidbody2D.AddTorque(RotationThrustLeft * Time.deltaTime);
			Debug.Log("Left Left");
			OnLeftForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
		}
		if (Keyboard.current.rightArrowKey.isPressed)
		{
			landerRigidbody2D.AddTorque(RotationThrustRight * Time.deltaTime);
			Debug.Log("Rotating Right");
			OnRightForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.TryGetComponent(out LandingPad landingPad)){
			Debug.Log("Landed on Terrain");
			return;
		}


		float relativeVelocityMagnitude = collision.relativeVelocity.magnitude; // to get the value of relative velocity to calculate score

		if (collision.relativeVelocity.magnitude > softLandingSpeed)
		{
			Debug.Log("Hard landing! Velocity: " + collision.relativeVelocity.magnitude);
		}
		/*else
		{
			Debug.Log("Soft landing. Velocity: " + collision.relativeVelocity.magnitude);
		}*/
		
		float dotVector = Vector2.Dot(Vector2.up, transform.up);
		if (dotVector < maxLandingAngle)
		{
			Debug.Log("Landed at a very step angle! Angle dot product: " + dotVector);
		}
		/*else
		{
			Debug.Log("Landed at a good angle. Angle dot product: " + dotVector);
		}*/

		else
		{
			Debug.Log("Successful Landing! -> Landed on the landing pad!");
		}


		//Score system

		float maxScoreAmountLandingAngle = 100;
		float scoreDotVectorMultiplier = 10f;
		float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

		float maxScoreAmoutLandingSpeed = 100;
		float landingSpeedScore = (softLandingSpeed - relativeVelocityMagnitude) * maxScoreAmoutLandingSpeed;

		Debug.Log("Landing Angle Score: " + landingAngleScore);
		Debug.Log("Landing Speed Score: " + landingSpeedScore);

		int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());

	}


	private void ConsumeFuel(){
	    float fuelConsumptionRate = 1f; // fuel consumption rate per second
		fuelAmount -= fuelConsumptionRate * Time.deltaTime;
	}
}
