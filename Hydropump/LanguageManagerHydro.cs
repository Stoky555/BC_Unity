using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManagerHydro : MonoBehaviour
{
    public Text LanguageButton;
    public Text AddButton;
    public Text RemoveButton;
    public Text SimulateButton;
    public Text SideMenuTitle;
    public Text SelectLabel;
    public Text MoreHelpButton;
    public Text MoreHelpTitle;
    public Text MoreHelpText;
    public Text GotItButton;
    public Text GraphButton;
    public Text GraphTitle;

    public GameObject DataManager;

    public GameObject SideMenuContent;

    private DataManager DataManagerScript;

    private InterfaceInfo LanguageInterfaceData;

    private Language ActiveLanguage = Language.English;

    public void Start()
    {
        DataManagerScript = DataManager.GetComponent<DataManager>();

        LanguageInterfaceData = JsonUtility.FromJson<InterfaceInfo>(DataManagerScript.GetJsonData("InterfaceInfoHydraulic", ActiveLanguage));

        SetInterfaceLanguage();
    }

    /// <summary>
    /// Zíkasnie dát jazyka z jsonu
    /// </summary>
    /// <returns></returns>
    public InterfaceInfo GetLanguageInterfaceData()
    {
        return LanguageInterfaceData;
    }
    /// <summary>
    /// Kontrola aktuálneho jazyka
    /// </summary>
    /// <returns></returns>
    public Language GetActiveLanguage()
    {
        return ActiveLanguage;
    }

    /// <summary>
    /// Nastavenie Slovenského jazyka
    /// </summary>
    public void SetSlovakLanguage()
    {
        ActiveLanguage = Language.Slovak;
        ChangeLanguage();
    }

    /// <summary>
    /// Nastavenie Anglického jazyka
    /// </summary>
    public void SetEnglishLanguage()
    {
        ActiveLanguage = Language.English;
        ChangeLanguage();
    }

    /// <summary>
    /// Zmena jazyka
    /// </summary>
    private void ChangeLanguage()
    {
        LanguageInterfaceData = JsonUtility.FromJson<InterfaceInfo>(DataManagerScript.GetJsonData("InterfaceInfoHydraulic", ActiveLanguage));

        SetInterfaceLanguage();
    }

    /// <summary>
    /// Nastavenie jednotlivých textových polí podľa nastaveného jazyka
    /// </summary>
    public void SetInterfaceLanguage()
    {
        AddButton.text = LanguageInterfaceData.Add;
        LanguageButton.text = LanguageInterfaceData.Language;
        RemoveButton.text = LanguageInterfaceData.Remove;
        SimulateButton.text = LanguageInterfaceData.Simulate;
        SideMenuTitle.text = LanguageInterfaceData.Title;
        SelectLabel.text = LanguageInterfaceData.SelectLabel;
        GotItButton.text = LanguageInterfaceData.GotIt;
        MoreHelpButton.text = LanguageInterfaceData.Open;
        MoreHelpTitle.text = LanguageInterfaceData.MoreHelpTitle;
        MoreHelpText.text = LanguageInterfaceData.MoreHelpText;
        GraphButton.text = LanguageInterfaceData.Graph;
        GraphTitle.text = LanguageInterfaceData.GraphTitle;

        foreach(Transform currentElement in SideMenuContent.transform)
        {
            if(currentElement.Find("Placeholder") != null) {
                currentElement.Find("Placeholder").GetComponent<Text>().text = LanguageInterfaceData.Placeholder;
            }
        }
    }
}
