using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public enum gameState
    {
        Paused,
        InProgress,
        GameOver,
    }
    public gameState currentGameState;

    [Header("Managers")]
    public UIManager uiScript;
    public PauseManager pmScript;
    public ScreenShake cameraShake;   
     

    [Header("Lives")]
    public int numOfLives = 3;
    const int maxNumOfLives = 3;

    [Header("Points")]
    public int points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoseLives()
    {
        numOfLives--;

        StartCoroutine(cameraShake.Shake());

        uiScript.LoseLives(numOfLives);
        if(numOfLives == 0)
        {
            StartCoroutine(uiScript.GameOver());
            pmScript.TriggerPauseState();
            //currentGameState = gameState.GameOver;
        }
    } 

    public void AddLives()
    {
        if(numOfLives < maxNumOfLives)
        {
            uiScript.AddLives(numOfLives);
            numOfLives++;            
        }
    }

    public void AddPoints()
    {
        if(currentGameState == gameState.InProgress)
        {
            points++;
            uiScript.PointsUpdate(points);
        }
    }
}
