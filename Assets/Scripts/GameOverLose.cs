using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLose : MonoBehaviour
{
   public void RestartButton()
    {
        SceneManager.LoadScene("DeadZoneTest");
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("Win Screen 1");
    }
}
