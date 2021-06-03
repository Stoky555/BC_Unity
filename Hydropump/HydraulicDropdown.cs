using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class HydraulicDropdown : MonoBehaviour
{
    List<string> ValidOptions = new List<string>();
    SelectOptionsHydraulic selectList;

    public Dropdown Select;
    public GameObject Content;

    /// <summary>
    /// Ziska a nastavi data ktore sa maju zobrazovat v dropdown selecte
    /// </summary>
    async void Start()
    {
        selectList = await ApiHelper.GetExperimentsList();
        PopulateList(selectList);
    }

    /// <summary>
    /// Nastavi data do dropdown selectu
    /// </summary>
    /// <param name="Options">The current gesture.</param>
    public void PopulateList(SelectOptionsHydraulic options)
    {
        ValidOptions.Clear();
        Select.ClearOptions();

        ValidOptions.Add("");

        foreach (var currentElement in options.experiments)
        {
            ValidOptions.Add(currentElement.experiment);
        }

        Select.AddOptions(ValidOptions);

        //GameObject.FindGameObjectWithTag("ApiClient").GetComponent<ApiResponseClient>().HandleApiClient(selectList.experiments[0].id);
    }

    /// <summary>
    /// Ak bola vybrata moznost z dropdwn selectu vykresli  formular simulacie
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    public void HandleInputData(int val)
    {
        if(Select.value == 0)
        {
            GameObject.FindGameObjectWithTag("ApiClient").GetComponent<ApiResponseClient>().HandleApiClient(-1);
            return;
        }
        GameObject.FindGameObjectWithTag("ApiClient").GetComponent<ApiResponseClient>().HandleApiClient(selectList.experiments[Select.value - 1].id);
    }
}
