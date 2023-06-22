using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;

    private void Awake() {
        if(Instance != this)
        {
            Instance = this;
        }
    }

    private void Start() {
        // Start Game
        SceneLoader.Instance.LoadSceneAsync(1);
    }
    
    public void RestartLevel(int levelBuildIndex)
    {
        SceneLoader.Instance.OnLoadFinished.AddListener(HandleFinishLevelRestart);
        SceneLoader.Instance.LoadSceneAsync(levelBuildIndex);
    }

    private void HandleFinishLevelRestart()
    {
        SceneLoader.Instance.OnLoadFinished.RemoveListener(HandleFinishLevelRestart);
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();

        if(!levelManager)
        {
            Debug.LogError("Unable to find a LevelManager in loaded scene.");
            return;
        }

        levelManager.TriggerLevelHasRestarted();
    }
}
