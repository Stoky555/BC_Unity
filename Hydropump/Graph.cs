using GoogleARCore.Examples.ObjectManipulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;

    private GameObject lastCircleGameObject;
    private GameObject lastCircleGameObject2;
    private GameObject lastCircleGameObject3;

    private int allowOnlyOnce = 1;
    private AndyPlacementManipulator andy;

    private void Start()
    {
        andy = GameObject.FindGameObjectWithTag("objectGenerator").GetComponent<AndyPlacementManipulator>();
    }

    /// <summary>
    /// Nastavenie zákldaných parametrov, ktoré potrebujeme
    /// </summary>
    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();
    }
    
    /// <summary>
    /// Vytvorenie pointu na grafe 
    /// </summary>
    /// <param name="anchoredPosition"></param>
    /// <returns></returns>
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(20, 20);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    /// <summary>
    /// Odstránenie hodnôt z grafu
    /// </summary>
    public void DestroyGraph()
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
    }

    /// <summary>
    /// Zobrazenie grafu
    /// </summary>
    /// <param name="valuesList"></param>
    /// <param name="maxVisibleValueAmount"></param>
    /// <param name="getAxisLabelX"></param>
    /// <param name="getAxisLabelY"></param>
    public void ShowGraph(List<ResponseValues> valuesList, int maxVisibleValueAmount = -1, Func<float, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if(andy.AndyList.Count > 1 && (allowOnlyOnce % andy.AndyList.Count != 1))
        {
            allowOnlyOnce++;
            return;
        }

        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (float _f) { return _f.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return _f.ToString(); };
        }

        if(maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valuesList.Count;
        }

        DestroyGraph();

        float graphWidth = graphContainer.rect.width;
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 0f;
        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        lastCircleGameObject = null;
        lastCircleGameObject2 = null;
        lastCircleGameObject3 = null;

        for (int i = Mathf.Max(valuesList.Count - maxVisibleValueAmount, 0); i < valuesList.Count; i++)
        {
            float value = Mathf.Max((float)valuesList[i].h1, (float)valuesList[i].h2, (float)valuesList[i].h3);
            if (value > yMaximum)
            {
                yMaximum = value;
            }
        }

        yMaximum = yMaximum * 1.1f;

        int xIndex = 0;
        for(int i = Mathf.Max(valuesList.Count - maxVisibleValueAmount, 0); i < valuesList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;

            CreateXAxisDotsAndConnections(valuesList, i, yMaximum, xPosition, graphHeight);

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -20f);

            labelX.GetComponent<Text>().text = getAxisLabelX((float)valuesList[i].time);

            gameObjectList.Add(labelX.gameObject);

            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-3f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);

        }
    }

    /// <summary>
    /// Vytvára spojenia na grafe
    /// </summary>
    /// <param name="dotPositionA"></param>
    /// <param name="dotPositionB"></param>
    /// <returns></returns>
    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 5f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));

        return gameObject;
    }

    /// <summary>
    /// Metoda nastavuje parametre pre vytvaranie bodov a spojnic bodov
    /// </summary>
    public void CreateXAxisDotsAndConnections(List<ResponseValues> valueList, int index, float yMaximum, float xPosition, float graphHeight)
    {
        /*if (valueList.Count > index)
        {
            float yPosition = ((float)valueList[index].h1 / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circleGameObject);
            if (lastCircleGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                dotConnectionGameObject.GetComponent<Image>().color = Color.green;
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastCircleGameObject = circleGameObject;
        }
        if (valueList.Count > index)
        {
            float yPosition = ((float)valueList[index].h2 / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circleGameObject);
            if (lastCircleGameObject2 != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject2.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                dotConnectionGameObject.GetComponent<Image>().color = Color.blue;
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastCircleGameObject2 = circleGameObject;
        }
        if (valueList.Count > index)
        {
            float yPosition = ((float)valueList[index].h3 / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circleGameObject);
            if (lastCircleGameObject3 != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject3.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                dotConnectionGameObject.GetComponent<Image>().color = Color.red;
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastCircleGameObject3 = circleGameObject;
        }*/

        if (valueList.Count > index)
        {
            CreateXAxisDots(ref lastCircleGameObject, yMaximum, graphHeight, xPosition, valueList[index].h1, Color.green);
            CreateXAxisDots(ref lastCircleGameObject2, yMaximum, graphHeight, xPosition, valueList[index].h2, Color.blue);
            CreateXAxisDots(ref lastCircleGameObject3, yMaximum, graphHeight, xPosition, valueList[index].h3, Color.red);
        }
    }

    /// <summary>
    /// Spájanie pointov grafu
    /// </summary>
    /// <param name="lastCircle"></param>
    /// <param name="yMaximum"></param>
    /// <param name="graphHeight"></param>
    /// <param name="xPosition"></param>
    /// <param name="value"></param>
    /// <param name="color"></param>
    private void CreateXAxisDots(ref GameObject lastCircle, float yMaximum, float graphHeight, float xPosition, double value, Color color)
    {
        float yPosition = ((float)value / yMaximum) * graphHeight;
        GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
        gameObjectList.Add(circleGameObject);
        if (lastCircle != null)
        {
            GameObject dotConnectionGameObject = CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            dotConnectionGameObject.GetComponent<Image>().color = color;
            gameObjectList.Add(dotConnectionGameObject);
        }
        lastCircle = circleGameObject;
    }

    /// <summary>
    /// Metoda vrati uhol otocenia (0-360) z vektoru
    /// </summary>
    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
