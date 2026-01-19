using UnityEngine;
using TMPro;


public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;

    private void Update()
    {
        UpdateStatsTextMesh();
    }
    private void UpdateStatsTextMesh()
    {
        statsTextMesh.text = 
            GameManager.Instance.GetScore() + "\n" +
            GameManager.Instance.GetTime() + "\n" +
            LanderController.Instance.GetSpeedX() + "\n" +
            LanderController.Instance.GetSpeedY() + "\n" +
            LanderController.Instance.GetFuelAmount();
    }
}
