using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulateButton : MonoBehaviour
{
    public Button button;

    /// <summary>
    /// Začatie celej simulačnej schémy
    /// </summary>
    public void Simulate() {
        GameObject.FindGameObjectWithTag("ApiClient").GetComponent<ApiResponseClient>().StartSimulation();
    }
}
