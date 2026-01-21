using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private Button playButton;
	[SerializeField] private Button quitButton;

	private void Awake()
	{
		Time.timeScale = 1f; // Ensure time scale is normal when in main menu

		playButton.onClick.AddListener(() =>
		{
			GameManager.ResetStaticData();
			SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
		});
		quitButton.onClick.AddListener(() =>
		{
			Application.Quit();
		});
	}

	private void Start()
	{
		playButton.Select();
	}
}