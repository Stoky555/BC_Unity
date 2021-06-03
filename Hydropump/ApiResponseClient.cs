using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ApiResponseClient : MonoBehaviour
{
    [SerializeField] Sprite someBgSprite;
    [SerializeField] Sprite someScrollbarSprite;
    [SerializeField] Sprite someDropDownSprite;
    [SerializeField] Sprite someCheckmarkSprite;
    [SerializeField] Sprite someMaskSprite;

    private Dictionary<string, List<double>> dropdowns = new Dictionary<string, List<double>>();
    private List<double> dropdownValues = new List<double>();

    private int currentExperimentId;

    public ExperimentResponseValues experimentResponseValues;
    public List<Tuple<string, string, string>> inputfieldsNames;

    public GameObject loading;
    public GameObject sideMenu;
    public GameObject Graph;

    // Start is called before the first frame update
    void Start()
    {
        inputfieldsNames = new List<Tuple<string, string, string>>();
    }

    /// <summary>
    /// Vytváranie vstupného poľa
    /// </summary>
    /// <param name="sidemenu"></param>
    /// <param name="input"></param>
    /// <param name="uiResources"></param>
    /// <returns></returns>
    private GameObject CreateNewInputField(GameObject sidemenu, Inputs input, DefaultControls.Resources uiResources)
    {
        uiResources.inputField = someBgSprite;
        GameObject uiInputField = DefaultControls.CreateInputField(uiResources);
        uiInputField.name = input.schema_var;
        uiInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 70);
        uiInputField.transform.SetParent(sidemenu.transform.Find("Content"), false);
        uiInputField.transform.GetChild(0).GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        uiInputField.transform.Find("Placeholder").GetComponent<Text>().fontSize = 44;
        uiInputField.GetComponent<InputField>().text = input.inputvals[0].name;
        uiInputField.transform.Find("Text").GetComponent<Text>().fontSize = 50;

        return uiInputField;
    }

    /// <summary>
    /// Vytváranie popisku pre polia
    /// </summary>
    /// <param name="uiResources"></param>
    /// <param name="currentElementIn"></param>
    /// <returns></returns>
    private GameObject CreateTextLabel(DefaultControls.Resources uiResources, Inputs currentElementIn) {
        GameObject uiInputFieldText = DefaultControls.CreateText(uiResources);
        uiInputFieldText.name = "InputLabel";
        uiInputFieldText.GetComponent<Text>().fontSize = 50;
        uiInputFieldText.GetComponent<Text>().color = Color.white;
        uiInputFieldText.GetComponent<Text>().text = currentElementIn.showed_var;
        uiInputFieldText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        uiInputFieldText.GetComponent<RectTransform>().position = new Vector3(0, 76);
        uiInputFieldText.GetComponent<RectTransform>().sizeDelta = new Vector2(400, (float)66.7);

        return uiInputFieldText;
    }

    /// <summary>
    /// Vytváranie rozbaľovacieho listu
    /// </summary>
    /// <param name="sidemenu"></param>
    /// <param name="uiResources"></param>
    /// <returns></returns>
    private GameObject CreateDropdown(GameObject sidemenu, DefaultControls.Resources uiResources)
    {
        uiResources.standard = someBgSprite;
        uiResources.background = someScrollbarSprite;
        uiResources.dropdown = someDropDownSprite;
        uiResources.checkmark = someCheckmarkSprite;
        uiResources.mask = someMaskSprite;
        GameObject uiDropdown = DefaultControls.CreateDropdown(uiResources);
        uiDropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 70);
        uiDropdown.transform.SetParent(sidemenu.transform.Find("Content"), false);
        uiDropdown.transform.GetChild(0).GetComponent<Text>().resizeTextForBestFit = true;
        uiDropdown.transform.GetChild(0).GetComponent<Text>().resizeTextMinSize = 5;
        uiDropdown.transform.GetChild(0).GetComponent<Text>().resizeTextMaxSize = 50;
        uiDropdown.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        uiDropdown.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 50);
        uiDropdown.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 50);
        uiDropdown.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>().fontSize = 40;
        uiDropdown.transform.GetChild(2).GetChild(1).GetComponent<Scrollbar>().size = 1;
        return uiDropdown;
    }

    /// <summary>
    /// Handler pre vytváranie dynamického formuláru
    /// </summary>
    /// <param name="id"></param>
    public async void HandleApiClient(int id) {
        currentExperimentId = id;

        GameObject sidemenu = GameObject.FindGameObjectWithTag("SideMenu").transform.Find("RightSidebar").gameObject;
        foreach (Transform child in sidemenu.transform.Find("Content"))
        {
            Destroy(child.gameObject);
        }

        if(id == -1)
        {
            return;
        }

        var experiment = await ApiHelper.GetOneExperiment(id);

        DefaultControls.Resources uiResources = new DefaultControls.Resources();
        foreach (var currentElement in experiment.inputs)
        {
            if(currentElement.group == "hidden")
            {
                if (currentElement.schema_var.StartsWith("V"))
                {
                    var leverList = GameObject.FindGameObjectsWithTag("Lever");
                    foreach(var currentLever in leverList)
                    {
                        if(currentElement.schema_var == currentLever.name)
                        {
                            currentLever.GetComponent<LeverController>().LeverSet((float)Convert.ToDouble(currentElement.inputvals[0].name));
                        }
                    }
                }
                continue;
            }

            GameObject uiInputFieldText = CreateTextLabel(uiResources, currentElement);

            if (currentElement.inputvals.Count > 1)
            {
                GameObject dropdown = CreateDropdown(sidemenu, uiResources);
                dropdown.name = currentElement.schema_var + " Dropdown";
                uiInputFieldText.transform.SetParent(dropdown.transform, false);
                List<string> ValidOptions = new List<string>();
                ValidOptions.Clear();
                dropdown.GetComponent<Dropdown>().ClearOptions();
                foreach (var elementsToAddToDropdown in currentElement.inputvals)
                {
                    ValidOptions.Add(elementsToAddToDropdown.name);
                    dropdownValues.Add(elementsToAddToDropdown.value);
                }
                dropdown.GetComponent<Dropdown>().AddOptions(ValidOptions);
                dropdowns.Add(dropdown.name, new List<double>(dropdownValues));
            }
            else
            {
                GameObject uiInputField = CreateNewInputField(sidemenu, currentElement, uiResources);
                uiInputFieldText.transform.SetParent(uiInputField.transform, false);
            }
        }
    }

    /// <summary>
    /// Vytvorenie JSONU parametrov simulácie a vrátenie simulačných dát
    /// </summary>
    public async void StartSimulation()
    {
        loading.SetActive(true);
        GameObject.FindGameObjectWithTag("CameraCanvas").GetComponent<PlayButton>().PlayButtonMethod();

        string json = "{\"id\":" + currentExperimentId.ToString() + ",";
        foreach (Transform child in GameObject.FindGameObjectWithTag("Content").transform)
        {
            if (child.name.Contains("Dropdown"))
            {
                List<double> result;
                dropdowns.TryGetValue(child.name, out result);
                var indexOnDropdown = child.GetComponent<Dropdown>().value;
                var resultFromDropdown = result[indexOnDropdown];
                json += "\"" + child.name.Split(' ')[0] + "\":\"" + resultFromDropdown + "\",";
                inputfieldsNames.Add(new Tuple<string, string, string>(child.Find("InputLabel").GetComponent<Text>().text, resultFromDropdown.ToString(), child.name.Split(' ')[0]));
            }
            else
            {
                json += "\"" + child.name + "\":\"" + child.GetComponent<InputField>().text + "\",";
                inputfieldsNames.Add(new Tuple<string, string, string>(child.Find("InputLabel").GetComponent<Text>().text, child.GetComponent<InputField>().text, child.name));
            }
        }
        foreach (var currentElement in GameObject.FindGameObjectsWithTag("Lever"))
        {
            
            json += "\"" + currentElement.name + "\":\"" + currentElement.GetComponent<LeverController>().value.ToString() + "\",";
            inputfieldsNames.Add(new Tuple<string, string, string>(currentElement.name, currentElement.GetComponent<LeverController>().value.ToString(), currentElement.name));
        }
        json = json.Remove(json.Length - 1);
        json += "}";

        experimentResponseValues = await ApiHelper.GetSimulationResponse(json);
        SetParametersToH(experimentResponseValues);
    }

    /// <summary>
    /// Posielanie hodnôt do objektov a začatie simulácie + začatie coroutine
    /// </summary>
    /// <param name="values"></param>
    private void SetParametersToH(ExperimentResponseValues values){
        foreach(var currentElement in GameObject.FindGameObjectsWithTag("H"))
        {
            currentElement.GetComponent<AnimateMe>().SetExperimentResponseValues(values);
            currentElement.GetComponent<AnimateMe>().StartCoroutine(currentElement.GetComponent<AnimateMe>().SimulationAnimation());

            loading.SetActive(false);
            GameObject.FindGameObjectWithTag("CameraCanvas").GetComponent<GraphButton>().OpenGraph();
        }
    }
}
