using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
	[SerializeField] private Button nextButton;

    private Action nextButtonCLickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonCLickAction();
			//LanderController.Instance.ResetLander();
			Hide();
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
            nextButtonCLickAction = GameManager.Instance.GoToNextLevel;
            nextButtonTextMesh.text = "Next Level";
		}
        else
        {
            titleTextMesh.text = "<color=#ff0000>Landing Failed!</color>";
            nextButtonCLickAction = GameManager.Instance.RetryLevel;
            nextButtonTextMesh.text = "Retry Level";
		}
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
		nextButton.Select();
	}

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
