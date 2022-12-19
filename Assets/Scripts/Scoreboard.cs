using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{

    private int score = 0;
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = $"SCORE: {score}";
    }

    public void IncreaseScore(int points) {
        score += points;
        text.text = $"SCORE: {score}";
    }
}
