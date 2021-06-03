using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.Android;
using System.Globalization;


public class SaveCSV : MonoBehaviour
{
    public ApiResponseClient apiResponse;
    public LanguageManagerHydro languageManagerScript;

    private InterfaceInfo LanguageData;

    /// <summary>
    /// Vypytanie si povolenia na pristup k  internemu ulozisku zariadenia
    /// </summary>
    private void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif
    }

    /// <summary>
    /// Nastavenie jazyka
    /// </summary>
    private void SetLanguageData()
    {
        LanguageData = languageManagerScript.GetLanguageInterfaceData();
    }

    /// <summary>
    /// Vytvorenie CSV suboru s datami simulacie
    /// </summary>
    private StringBuilder CreateData()
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";

        List<string[]> rowData = new List<string[]>();
        ExperimentResponseValues data = apiResponse.experimentResponseValues;
        List<Tuple<string, string, string>> inputFields = apiResponse.inputfieldsNames;

        string[] rowDataTemp = new string[inputFields.Count];
        int counter = 0;
        foreach(var currentElement in inputFields)
        {
            rowDataTemp[counter] = currentElement.Item1;
            counter++;
        }
        rowData.Add(rowDataTemp);

        rowDataTemp = new string[inputFields.Count];
        counter = 0;
        foreach (var currentElement in inputFields)
        {
            rowDataTemp[counter] = currentElement.Item3;
            counter++;
        }
        rowData.Add(rowDataTemp);

        counter = 0;
        rowDataTemp = new string[inputFields.Count];
        foreach (var currentElement in inputFields)
        {
            rowDataTemp[counter] = currentElement.Item2;
            counter++;
        }
        rowData.Add(rowDataTemp);

        rowData.Add(new string[0]);

        rowDataTemp = new string[4];
        rowDataTemp[0] = "Time";
        rowDataTemp[1] = "H1";
        rowDataTemp[2] = "H3";
        rowDataTemp[3] = "H2";
        rowData.Add(rowDataTemp);

        for (int i = 0; i < data.response.Count; i++)
        {
            rowDataTemp = new string[4];
            rowDataTemp[0] = data.response[i].time.ToString(nfi);
            rowDataTemp[1] = data.response[i].h1.ToString(nfi);
            rowDataTemp[2] = data.response[i].h3.ToString(nfi);
            rowDataTemp[3] = data.response[i].h2.ToString(nfi);
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        return sb;
    }

    /// <summary>
    /// Stiahnutie CSV Suboru do pamate telefonu (Downloads adresar)
    /// </summary>
    public void Save()
    {
        SetLanguageData();

        string filePath = GetPath();
        StringBuilder sb = CreateData();

        try
        {
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
            SSTools.ShowMessage(LanguageData.Download, SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.Log(e);
            SSTools.ShowMessage("Unauthorized Access", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }

    /// <summary>
    /// Zdielanie CSV suboru
    /// </summary>
    public void Share()
    {
        SetLanguageData();

        string filePath = GetPath(true);
        StringBuilder sb = CreateData();

        try
        {
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            new NativeShare().AddFile(filePath).SetSubject("OVLAB Export").SetText("").Share();

        }
        catch (Exception e)
        {
            SSTools.ShowMessage(e.ToString(), SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }

    /// <summary>
    /// Vyhladanie cesty k downloads adresaru
    /// </summary>
    public string GetAndroidExternalStoragePath()
    {
        try
        {
            AndroidJavaClass ajc = new AndroidJavaClass("android.os.Environment");
            return ajc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", ajc.GetStatic<string>("DIRECTORY_DOWNLOADS")).Call<string>("getAbsolutePath");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return String.Empty;
        }
    }

    /// <summary>
    /// Metoda vrati celu cestu kam sa ma subor ulozit v pripade ze sa subor zdiela vrati len jeho nazov
    /// </summary>
    private string GetPath(bool share = false)
    {

        string fileName = LanguageData.FileName + ".csv";

        if (share)
        {
#if UNITY_ANDROID
            return Application.persistentDataPath + fileName;
#endif
        }
        else
        {
#if UNITY_EDITOR
            return Application.dataPath + "/" + fileName;
#elif UNITY_ANDROID
                return GetAndroidExternalStoragePath() + "/" + fileName;
#else
                return Application.dataPath + "/" + fileName;
#endif
        }
    }
}
