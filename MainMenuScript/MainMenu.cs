using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Prepnutie scény na Towercoptéru
    /// </summary>
   public void PlayTowercopter()
    {
        SceneManager.LoadScene(4);
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlayPendulum()
    {
        //SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Prepnutie scény na Hydraulickú sústavu
    /// </summary>
    public void PlayHydroPump() {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Vypnutie aplikácie
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

}


