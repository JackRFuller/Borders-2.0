using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Image[] imageLives;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
