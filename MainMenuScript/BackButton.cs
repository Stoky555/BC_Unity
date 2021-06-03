using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    /// <summary>
    /// Návrat do menu
    /// </summary>
   public void GoBackButton()
    {
        SceneManager.LoadScene(0);
    }
}
