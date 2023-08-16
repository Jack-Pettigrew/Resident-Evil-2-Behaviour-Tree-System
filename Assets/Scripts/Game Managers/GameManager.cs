using DD.Core.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    
    public static GameState GameState { private set; get; } = GameState.STARTING_GAME;

    // RUNTIME COMPONENTS
    private static PauseMenu pauseMenu;

    // RUNTIME EVENTS
    public static event Action<bool> OnGamePause;
    public static event Action OnGameQuitting;

    private void Awake() {
        if(Instance != this)
        {
            Instance = this;
        }
    }

    private void Start() {
        // Start Game
        SceneLoader.Instance.LoadSceneAsync(1, () => GameState = GameState.MAIN_MENU);
    }

    public static void SetGameState(GameState state)
    {
        GameState = state;
    }
    
    public void ReturnToMainMenu()
    {
        SceneLoader.Instance.LoadSceneAsync(1, HandleFinishReturnToMenu);
    }
    
    public void RestartLevel(int levelBuildIndex)
    {
        // SceneLoader.Instance.OnLoadFinished.AddListener(HandleFinishLevelRestart);
        SceneLoader.Instance.LoadSceneAsync(levelBuildIndex, HandleFinishLevelRestart);
    }

    private void HandleFinishLevelRestart()
    {
        // SceneLoader.Instance.OnLoadFinished.RemoveListener(HandleFinishLevelRestart);
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();

        if(!levelManager)
        {
            Debug.LogError("Unable to find a LevelManager in loaded scene.");
            return;
        }

        levelManager.HandleLevelRestarted();
        GameState = GameState.PLAYING;
    }

    private void HandleFinishReturnToMenu()
    {
        InputManager.Instance.CursorToggle(true);
        GameState = GameState.MAIN_MENU;
    }

    public static void PauseGame()
    {        
        if(GameState == GameState.STARTING_GAME || GameState == GameState.MAIN_MENU || GameState == GameState.PAUSED) return;
        
        if(!pauseMenu)
        {
            pauseMenu = FindObjectOfType<PauseMenu>(true);

            if(!pauseMenu)
            {
                Debug.LogWarning("No PauseMenu object found. Pausing logic skipped.");
                return;
            }
        }

        Time.timeScale = 0.0f;
        pauseMenu.ShowMenu();
        GameState = GameState.PAUSED;
        OnGamePause?.Invoke(true);
    }

    public static void UnpauseGame()
    {                
        if(GameState == GameState.STARTING_GAME || GameState == GameState.MAIN_MENU || GameState == GameState.PLAYING) return;
        
        if(!pauseMenu)
        {
            pauseMenu = FindObjectOfType<PauseMenu>(true);

            if(!pauseMenu)
            {
                Debug.LogWarning("No PauseMenu object found. Pausing logic skipped.");
                return;
            }
        }

        pauseMenu.HideMenu();
        Time.timeScale = 1.0f;
        GameState = GameState.PLAYING;
        OnGamePause?.Invoke(false);
    }

    public static void QuitGame()
    {
        OnGameQuitting?.Invoke();
        Application.Quit();
    }
}

[System.Serializable]
public enum GameState
{
    STARTING_GAME,
    MAIN_MENU,
    PAUSED,
    PLAYING
}