using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private Image fuelImage;

    private void Update()
    {
        UpdateStatsTextMesh();
    }
    private void UpdateStatsTextMesh()
    {
        speedUpArrowGameObject.SetActive(LanderController.Instance.GetSpeedY() > 0f);
        speedDownArrowGameObject.SetActive(LanderController.Instance.GetSpeedY() < 0f);
        speedRightArrowGameObject.SetActive(LanderController.Instance.GetSpeedX() > 0f);
        speedLeftArrowGameObject.SetActive(LanderController.Instance.GetSpeedX() < 0f);

        fuelImage.fillAmount = LanderController.Instance.GetFuelAmountNormalized(); 

        statsTextMesh.text = 
            GameManager.Instance.GetScore() + "\n" +
            Mathf.Round (GameManager.Instance.GetTime()) + "\n" +
            Mathf.Abs(Mathf.Round(LanderController.Instance.GetSpeedX() * 10f )) + "\n" +
            Mathf.Abs(Mathf.Round(LanderController.Instance.GetSpeedY() * 10f ))+ "\n" +
            LanderController.Instance.GetFuelAmount();
    }
}
