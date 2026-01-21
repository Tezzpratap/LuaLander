using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
	[SerializeField] private Button soundVolumeButton;
	[SerializeField] private Button musicVolumeButton;
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
	[SerializeField] private TextMeshProUGUI musicVolumeTextMesh;

	private void Awake()
    {
        soundVolumeButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = "Sound Volume: " + SoundManager.Instance.GetSoundVolume();
		});
        musicVolumeButton.onClick.AddListener(() => {
			MusicManager.Instance.ChangeMusicVolume();
			soundVolumeTextMesh.text = "Music Volume: " + MusicManager.Instance.GetMusicVolume();
		});
		resumeButton.onClick.AddListener(() => {
            GameManager.Instance.UnpauseGame();
		});
        mainMenuButton.onClick.AddListener(() => {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
	}

    private void Start(){
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        soundVolumeTextMesh.text = "Sound Volume: " + SoundManager.Instance.GetSoundVolume();
		musicVolumeTextMesh.text = "Music Volume: " + MusicManager.Instance.GetMusicVolume();
		Hide();
	}

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
	}

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }


	private void Show()
    {
        gameObject.SetActive(true);
		resumeButton.Select();
	}

    private void Hide(){
        gameObject.SetActive(false);
	}

}
