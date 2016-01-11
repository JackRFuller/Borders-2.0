using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    [Header("Managers")]
    public UIManager uiScript;
    public ScreenShake cameraShake;

    [Header("Lives")]
    public int numOfLives = 3;
    const int maxNumOfLives = 3;

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
            Debug.Log("Game Over!");
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
}
