using UnityEngine;
using TMPro;

public class LandingPadVisuals : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreMultiplierText;

    private void Awake(){
        LandingPad landingPad = GetComponent<LandingPad>();
        scoreMultiplierText.text = "x" + landingPad.GetScoreMultiplier().ToString();
	}
}
