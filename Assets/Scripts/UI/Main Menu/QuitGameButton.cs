using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    public void Quit()
    {
        GameManager.QuitGame();
    }
}
