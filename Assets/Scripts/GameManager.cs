using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	private static int levelNumber = 1;
	private static int totalScore = 0;

	public static void ResetStaticData() //resetting static data when starting a new game
	{
		levelNumber = 1;
		totalScore = 0;
	}

	[SerializeField] private List<GameLevel> gameLevelList;
	[SerializeField] private CinemachineCamera cinemachineCamera;

	public event EventHandler OnGamePaused;
	public event EventHandler OnGameUnpaused;

	private int score;
	private float time;
	private bool isTimerActive;

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		LanderController.Instance.OnCoinPickup += Lander_CoinPickupEvent;
		LanderController.Instance.OnLanded += Lander_OnLanded;
		LanderController.Instance.OnStateChanged += Lander_OnStateChanged;

		GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
		LoadCurrentLevel();
	}

	private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e)
	{
		PauseUnpauseGame();
	}


	private void Update()
	{
		if (isTimerActive)
		{
			time += Time.deltaTime;
		}
	}

	private void LoadCurrentLevel()
	{
		GameLevel gameLevel = GetGameLevel();
		GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
		LanderController.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
		cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();  //will follow the target transform defined in the level
		CameraZoom.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
		//break;
	}

	private GameLevel GetGameLevel()
	{
		foreach (GameLevel gameLevel in gameLevelList)
		{
			if (gameLevel.GetLevelNumber() == levelNumber)
			{
				return gameLevel;
			}
		}
		return null;
	}


	private void Lander_CoinPickupEvent(object sender, System.EventArgs e)
	{
		AddScore(10);
		Debug.Log("Coin Picked Up!");
	}

	private void Lander_OnLanded(object sender, LanderController.OnLandedEventArgs e)
	{
		AddScore(e.score);
	}

	private void Lander_OnStateChanged(object sender, LanderController.OnStateChangedEventArgs e)
	{
		isTimerActive = e.state == LanderController.State.Normal;

		if (e.state == LanderController.State.Normal)
		{
			cinemachineCamera.Target.TrackingTarget = LanderController.Instance.transform;
			CameraZoom.Instance.SetNormalOrthographicSize();
		}
	}

	public void AddScore(int points)
	{
		score += points;
		Debug.Log("Score: " + score);
	}

	public int GetScore()
	{
		return score;
	}

	public float GetTime()
	{
		return time;
	}

	public int GetTotalScore()
	{
		return totalScore;
	}

	public void GoToNextLevel()
	{
		levelNumber++;
		totalScore += score; //adding current level score to total score

		if (GetGameLevel() == null)
		{
			//No more levels, go to main menu
			SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
		}
		else
		{
			SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
		}
	}

	public void RetryLevel()
	{
		SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
	}
	public int GetLevelNumber()
	{
		return levelNumber;
	}

	public void PauseUnpauseGame()
	{
		if (Time.timeScale == 1f)
		{
			PauseGam();
		}
		else
		{
			UnpauseGame();
		}
	}


	public void PauseGam()
	{
		Time.timeScale = 0f;
		OnGamePaused?.Invoke(this, EventArgs.Empty);
	}

	public void UnpauseGame()
	{
		Time.timeScale = 1f;
		OnGameUnpaused?.Invoke(this, EventArgs.Empty);
	}
}