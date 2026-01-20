using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;
    private float time;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LanderController.Instance.CoinPickupEvent += Lander_CoinPickupEvent;
        LanderController.Instance.OnLanded += Lander_OnLanded;
        LanderController.Instance.OnStateChanged += Lander_OnStateChanged;
    }

    private void Update()
    {
        time += Time.deltaTime;
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
        if (e.state == LanderController.State.WaitingToStart)
        {
            score = 0;
            time = 0f;
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
}
