using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverViewController : ViewController
{
    [SerializeField]
    private GameEvent _OnRestartEvent;

    [SerializeField]
    private TextMeshProUGUI _ScoreValue;
    [SerializeField]
    private TextMeshProUGUI _BestScoreValue;

    protected override void OnSetup()
    {
        int best = 0;
        int score = LevelSystem.Instance.GetScore();
        // get best score
        if (PlayerPrefs.HasKey("BestScore"))
            best = PlayerPrefs.GetInt("BestScore");

        if (score >= best)
        {
            PlayerPrefs.SetInt("BestScore", score);
            best = score;
        }

        _BestScoreValue.text = best.ToString();
        _ScoreValue.text = score.ToString();
    }

    public void Restart()
    {
        _OnRestartEvent?.Invoke();
    }
}
