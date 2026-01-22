using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
<<<<<<< HEAD
using System;
=======
>>>>>>> abdc6864e9265bb0b95f4ae6b58983f7602a9677

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
<<<<<<< HEAD
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
	[SerializeField] private Button nextButton;

    private Action nextButtonCLickAction;
=======
    [SerializeField] private Button nextButton;
>>>>>>> abdc6864e9265bb0b95f4ae6b58983f7602a9677

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
<<<<<<< HEAD
            nextButtonCLickAction();
			//LanderController.Instance.ResetLander();
			Hide();
=======
            SceneManager.LoadScene(0);
            //LanderController.Instance.ResetLander();
            Hide();
>>>>>>> abdc6864e9265bb0b95f4ae6b58983f7602a9677
        });
    }

    private void Start()
    {
        LanderController.Instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Lander_OnLanded(object sender, LanderController.OnLandedEventArgs e)
    {
        gameObject.SetActive(true);
        if (e.landingTypes == LanderController.LandingTypes.Success)
        {
            titleTextMesh.text = "Successful Landing!";
<<<<<<< HEAD
            nextButtonCLickAction = GameManager.Instance.GoToNextLevel;
            nextButtonTextMesh.text = "Next Level";
		}
        else
        {
            titleTextMesh.text = "<color=#ff0000>Landing Failed!</color>";
            nextButtonCLickAction = GameManager.Instance.RetryLevel;
            nextButtonTextMesh.text = "Retry Level";
		}
=======
        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>Landing Failed!</color>";
        }
>>>>>>> abdc6864e9265bb0b95f4ae6b58983f7602a9677
        statsTextMesh.text = 
            Mathf.Round(e.landingSpeed * 2f) + "\n" +
            Mathf.Round(e.dotVector * 100f) + "\n" +
            "X" + e.scoreMultiplier + "\n" +
            e.score; 

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
<<<<<<< HEAD
		nextButton.Select();
	}
=======
    }
>>>>>>> abdc6864e9265bb0b95f4ae6b58983f7602a9677

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
