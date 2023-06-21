using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int sceneBuildIndexToLoad = 1;
    
    private void Start() {
        SceneLoader.Instance.LoadSceneAsync(sceneBuildIndexToLoad);
    }
}
