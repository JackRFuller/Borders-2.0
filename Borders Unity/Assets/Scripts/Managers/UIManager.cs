using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    [Header("Health")]
    public Image[] imageLives;

    [Header("Points")]
    public Text textPoints;
    private float lastScore;

    [Header("Game Over")]
    public Animation GameOverPanelIn;
    public Animation GameOverOptionsIn;
    public Text textLastScore;
    public Text textHighScore;

    [Header("Lerping")]
    public float timeTakenDuringLerp = 1F;
    private bool isScoreLerping;
    private float startPos = 0;
    private float timeStartedLerping;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (isScoreLerping)
        {
            LerpLastScore();
        }
	
	}

    public void AddLives(int numOfLives)
    {
        switch (numOfLives)
        {
            case (1):
                imageLives[numOfLives].enabled = true;
                break;
            case (2):
                imageLives[numOfLives].enabled = true;
                break;
        }
    }

    public void LoseLives(int numOfLives)
    {
        switch (numOfLives)
        {
            case (0):
                imageLives[numOfLives].enabled = false;
                break;
            case (1):
                imageLives[numOfLives].enabled = false;
                break;
            case (2):
                imageLives[numOfLives].enabled = false;
                break;           
        }
    }

    public void PointsUpdate(int _newScore)
    {
        textPoints.text = _newScore.ToString();
        lastScore = _newScore;
    }

    public IEnumerator GameOver()
    {
        textHighScore.text = PlayerPrefs.GetFloat("HighScore").ToString("F0");

        GameOverPanelIn.Play("MenuIn");
        yield return new WaitForSeconds(GameOverPanelIn.clip.length);
        StartLerpingLastScore();
    }

    void StartLerpingLastScore()
    {
        timeStartedLerping = Time.time;
        isScoreLerping = true;
    }

    void LerpLastScore()
    {
        float _timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = _timeSinceStarted / timeTakenDuringLerp;

        float _Score = Mathf.Lerp(startPos, lastScore, percentageComplete);
        textLastScore.text = _Score.ToString("F0");

        if(_Score > PlayerPrefs.GetFloat("HighScore"))
        {
            SetNewHighScore(_Score);
        }

        if (percentageComplete >= 1.0F)
        {
            isScoreLerping = false;
            BringInOptions();
        }
    }

    void SetNewHighScore(float _newHighScore)
    {
        PlayerPrefs.SetFloat("HighScore", _newHighScore);
        textHighScore.text = _newHighScore.ToString("F0");
    }

    void BringInOptions()
    {
        GameOverOptionsIn.Play();
    }

    
}
