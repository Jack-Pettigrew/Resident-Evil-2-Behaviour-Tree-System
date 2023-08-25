using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DD.Core.Control;

public class LevelManager : MonoBehaviour
{
    public UnityEvent OnLevelRestarted;
    public UnityEvent OnLevelEnded;
                
    private void Start() {
        // Level started request most up to date settings
        SettingsManager.Instance?.ApplySavedSettings();

        AudioListener.pause = false;
    }

    public void EndLevel()
    {
        SceneLoader.Instance.LoadSceneAsync(2);
    }

    public void RequestReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
        
    /// <summary>
    /// Requests the GameManager to restart the level.
    /// </summary>
    public void RequestLevelRestart()
    {
        GameManager.Instance.RestartLevel(SceneLoader.Instance.ActiveScene.Value.buildIndex);
    }
    
    /// <summary>
    /// Called by the GameManager to perform level restart logic.
    /// This may contain logic to skip intro cutscenes or move the player.
    /// </summary>
    public void HandleLevelRestarted()
    {
        OnLevelRestarted?.Invoke();
    }

    public void RequestPauseGame()
    {
        GameManager.PauseGame();
    }

    public void RequestUnpauseGame()
    {
        GameManager.UnpauseGame();
    }
}
