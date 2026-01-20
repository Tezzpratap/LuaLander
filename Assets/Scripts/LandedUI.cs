using UnityEngine;
using TMPro;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;

    private void Start()
    {
        LanderController.Instance.OnLanded += Lander_OnLanded;
        Hide();
    }

    private void Lander_OnLanded(object sender, LanderController.OnLandedEventArgs e)
    {
        gameObject.SetActive(true);
        if (e.LandingTypes == LanderController.LandingTypes.Success)
        {
            titleTextMesh.text = "Successful Landing!";
        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>Landing Failed!</color>";
        }
        statsTextMesh.text = 
            Mathf.Round(e.landignSpeed * 2f) + "\n" +
            Mathf.Round(e.dotVector * 100f) + "\n" +
            "X" + e.scoreMultiplier + "\n" +
            e.score; 

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
