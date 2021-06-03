using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimateMe : MonoBehaviour
{   
    ExperimentResponseValues experimentResponseValues;
    int currentElement;
    float cylinderHeight = 930f - 33f;
    float maximumHeightForCylinder;
    private GameObject[] smallTubes;

    private List<ResponseValues> graphValues;

    private void Start()
    {
        currentElement = 0;
        smallTubes = GameObject.FindGameObjectsWithTag("SmallTube");
        graphValues = new List<ResponseValues>();
    }

    /// <summary>
    /// Samotná animácia, pridávanie dát do grafu
    /// </summary>
    /// <returns></returns>
    public IEnumerator SimulationAnimation()
    {
        var y = cylinderHeight / maximumHeightForCylinder;
        currentElement = 0;
        graphValues.Clear();

        var interval = (float)experimentResponseValues.response[experimentResponseValues.response.Count - 1].time - (float)experimentResponseValues.response[experimentResponseValues.response.Count - 2].time;

        WaitForSeconds wait  = new WaitForSeconds(interval);
        while (true) {
            if(currentElement >= experimentResponseValues.response.Count)
            {
                break;
            }

            wait = new WaitForSeconds(interval);

            for (int i = 0; i <= smallTubes.Count() - 1; i += 1)
            {
                smallTubes[i].GetComponent<SmallTubesAnimation>().FillTheTube();
                if (smallTubes[i].GetComponent<SmallTubesAnimation>().isDone == true)
                {
                    if (smallTubes.Count() == i + 1)
                    {
                        continue;
                    }
                    else if (smallTubes[i + 1].GetComponent<SmallTubesAnimation>().isDone == true)
                    {
                        continue;
                    }
                    smallTubes[i + 1].GetComponent<SmallTubesAnimation>().goodToGo = true;
                }
            }


            if (name == "h1")
            {
                Vector3 vec = new Vector3(transform.localScale.x, ((float)y * (float)experimentResponseValues.response[currentElement].h1) + 33.33334f, transform.localScale.z);
                transform.localScale = vec;

                if (currentElement % 5 == 0)
                {
                    graphValues.Add(experimentResponseValues.response[currentElement]);
                    GameObject.FindGameObjectWithTag("graph").GetComponent<Graph>().ShowGraph(graphValues, -1, (float _f) => (_f) + "s", (float _f) => (Mathf.RoundToInt(_f) + "cm"));
                }
                else if ((experimentResponseValues.response.Count - 1) == currentElement)
                {
                    graphValues.Add(experimentResponseValues.response[currentElement]);
                    GameObject.FindGameObjectWithTag("graph").GetComponent<Graph>().ShowGraph(graphValues, -1, (float _f) => (_f) + "s", (float _f) => (Mathf.RoundToInt(_f) + "cm"));
                }
            }
            else if (name == "h2")
            {
                Vector3 vec = new Vector3(transform.localScale.x, ((float)y * (float)experimentResponseValues.response[currentElement].h2) + 33.33334f, transform.localScale.z);
                transform.localScale = vec;
            }
            else if (name == "h3")
            {
                Vector3 vec = new Vector3(transform.localScale.x, ((float)y * (float)experimentResponseValues.response[currentElement].h3) + 33.33334f, transform.localScale.z);
                transform.localScale = vec;
            }

            currentElement++;
            yield return wait;
        }
    }

    /// <summary>
    /// Nastavenie parametrov simulácie
    /// </summary>
    /// <param name="parameter">Parametre simulácie</param>
    public void SetExperimentResponseValues(ExperimentResponseValues parameter)
    {
        experimentResponseValues = parameter;
        maximumHeightForCylinder = 62f;
    }
}