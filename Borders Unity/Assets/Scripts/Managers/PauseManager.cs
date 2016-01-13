using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {

    [Header("Managers")]
    public LevelManager lmScript;
    public WaveManager wmScript;
    public PlayerMovement pmScript;

    [Header("UI Items")]
    public Animation PauseMenu;

    public void TriggerPauseState()
    {
        switch (lmScript.currentGameState)
        {
            case (LevelManager.gameState.InProgress ):
                PauseGame();
                break;
            case (LevelManager.gameState.Paused):
                StartCoroutine(UnPauseGame());
                break;
        }
    }

    void PauseGame()
    {
        PauseMenu.Play("MenuIn");
        lmScript.currentGameState = LevelManager.gameState.Paused;
        wmScript.PauseGame();
        pmScript.PauseGame();
    }

    IEnumerator UnPauseGame()
    {
        PauseMenu.Play("MenuOut");
        yield return new WaitForSeconds(PauseMenu.GetClip("MenuOut").length);
        lmScript.currentGameState = LevelManager.gameState.InProgress;
        wmScript.UnPauseGame();
        pmScript.UnPauseGame();
    }
}
