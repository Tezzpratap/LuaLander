using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LanderController : MonoBehaviour
{
	public static LanderController Instance { get; private set; }
	public const float GRAVITY_NORMAL = 0.7f;

	public event EventHandler OnBeforeForce;
	public event EventHandler OnUpForce; //event handlers for thruster effects
	public event EventHandler OnLeftForce;
	public event EventHandler OnRightForce;
	public event EventHandler OnDownForce;
	public event EventHandler OnThrustStop;
	//public event EventHandler CoinPickupEvent;
	public event EventHandler OnCoinPickup;
	public event EventHandler OnFuelPickup;
	public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<OnLandedEventArgs> OnLanded;
	public class OnLandedEventArgs : EventArgs
	{
		public LandingTypes landingTypes;
		public int score;
		public float dotVector;
		public float landingSpeed;
		public float scoreMultiplier;
	}

	public class OnStateChangedEventArgs : EventArgs
	{
		public State state;
    }


    public enum LandingTypes
	{
		Success,
		WrongLanding,
		TooSteepAngle,
		TooFastLanding,
	}

	public enum State
	{
		WaitingToStart,
		Normal,
		GameOver,
    }

    private Rigidbody2D landerRigidbody2D;
	private float fuelAmount;
    private float fuelAmountMax = 10f;
	private State state;
	
	public float Thrust = 1000f;
	public float RotationThrustRight = +500f;
	public float RotationThrustLeft = -500f;
	public float softLandingSpeed = 3f;
	public float maxLandingAngle = 0.9f;

	private void Awake(){
		Instance = this;
		state = State.WaitingToStart;
        fuelAmount = fuelAmountMax;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
		landerRigidbody2D.gravityScale = 0f; // disable gravity when no input
    }


    private void FixedUpdate()
    {
		OnThrustStop?.Invoke(this, EventArgs.Empty);	


		switch (state)
		{
			default:
			case State.WaitingToStart:

                if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    ConsumeFuel();
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL; // enable gravity when input
					state = State.Normal;
					SetState(State.Normal);
                }

                break;
			case State.Normal:

                if ((Keyboard.current.spaceKey.isPressed) || (Keyboard.current.upArrowKey.isPressed))
                {
                    landerRigidbody2D.AddForce(Thrust * transform.up * Time.deltaTime);
                    //Debug.Log("Thrusting");
                    OnUpForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
                }
                if (Keyboard.current.downArrowKey.isPressed)
                {
                    landerRigidbody2D.AddForce(Thrust * -transform.up * Time.deltaTime);
                    //Debug.Log("Revers Thrusting");
                    OnDownForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
                }
                if (Keyboard.current.leftArrowKey.isPressed)
                {
                    landerRigidbody2D.AddTorque(RotationThrustLeft * Time.deltaTime);
                    //Debug.Log("Left Left");
                    OnLeftForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
                }
                if (Keyboard.current.rightArrowKey.isPressed)
                {
                    landerRigidbody2D.AddTorque(RotationThrustRight * Time.deltaTime);
                    //Debug.Log("Rotating Right");
                    OnRightForce?.Invoke(this, EventArgs.Empty); //invoke event for thruster effects
                }

                break;
			case State.GameOver:
				break;
        }


		if (fuelAmount <= 0f)
		{
			return;
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude; // to get the value of relative velocity to calculate score
        float dotVector = Vector2.Dot(Vector2.up, transform.up); // dot product to get the angle of landing

        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad)){
			Debug.Log("Landed on Terrain");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingTypes = LandingTypes.WrongLanding,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
            });
			SetState(State.GameOver);
            return;
		}

		if (collision.relativeVelocity.magnitude > softLandingSpeed)
		{
			Debug.Log("Hard landing! Velocity: " + collision.relativeVelocity.magnitude);
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
                landingTypes = LandingTypes.TooFastLanding,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
            });
            SetState(State.GameOver);
            return;
        }
		/*else
		{
			Debug.Log("Soft landing. Velocity: " + collision.relativeVelocity.magnitude);
		}*/
		
		
		if (dotVector < maxLandingAngle)
		{
			Debug.Log("Landed at a very step angle! Angle dot product: " + dotVector);
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                score = 0,
				landingTypes = LandingTypes.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
            });
            SetState(State.GameOver);
            return;

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

		OnLanded?.Invoke(this, new OnLandedEventArgs {
			score = score,
			landingTypes = LandingTypes.Success,
			dotVector = dotVector,
			landingSpeed = relativeVelocityMagnitude,
			scoreMultiplier = landingPad.GetScoreMultiplier(),
		});

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FuelPickup fuelPickup))
		{
			fuelAmount += 20f; // increase fuel amount by 20 units
			Destroy(collision.gameObject);
            //FuelPickup.DestroySelf // destroy the fuel pickup object
            Debug.Log("Fuel Picked Up! Current Fuel: " + fuelAmount);
			if (fuelAmount > fuelAmountMax)
			{
				fuelAmount = fuelAmountMax;
            }
        }

        if (collision.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            fuelAmount += 20f; // increase fuel amount by 20 units
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);
            //CoinPickup.DestroySelf // destroy the fuel pickup object
            Debug.Log("Fuel Picked Up! Current Fuel: " + fuelAmount);
        }

    }

	private void SetState(State state)
	{
		this.state = state;
		OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
		{
			state = state,
		});
	}

    private void ConsumeFuel(){
	    float fuelConsumptionRate = 1f; // fuel consumption rate per second
		fuelAmount -= fuelConsumptionRate * Time.deltaTime;
	}

	public float GetFuelAmount(){
		return fuelAmount;
    }

	public float GetFuelAmountNormalized()
	{ 
		return fuelAmount / fuelAmountMax;
    }

    public float GetSpeedX(){
		return landerRigidbody2D.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocityY;
    }
}
