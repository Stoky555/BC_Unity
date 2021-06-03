using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class ApiHelper
{
    /// <summary>
    /// Získanie všetkých regulátorov dostupných pre hydraulickú sústavu
    /// </summary>
    /// <returns></returns>
    public static async Task<SelectOptionsHydraulic> GetExperimentsList()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://wt7.fei.stuba.sk/ovl-api/api/exp-names-cont/3D-model/3");
        request.Headers["Authorization"] = "Bearer LwR1uDvk1Wr9Twv5LmjMulDdD7tsT7v3Vxp0IL7gMnMbnUi58g2IewOMnunK";
        HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        reader.Close();
        response.Close();
        SelectOptionsHydraulic selectList = JsonUtility.FromJson<SelectOptionsHydraulic>("{\"experiments\":" + json + "}");
        return selectList;
    }

    /// <summary>
    /// Získanie detailu pre experiment pomocou http requestu
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static async Task<Experiment> GetOneExperiment(int id)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://wt7.fei.stuba.sk/ovl-api/api/experiment/" + id);
        request.Headers["Authorization"] = "Bearer LwR1uDvk1Wr9Twv5LmjMulDdD7tsT7v3Vxp0IL7gMnMbnUi58g2IewOMnunK";
        HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        reader.Close();
        response.Close();
        Experiment experimentFromJson = JsonUtility.FromJson<Experiment>(json);

        return experimentFromJson;
    }

    /// <summary>
    /// Posielanie parametrov simulácie a vrátenie simulačných dát
    /// </summary>
    /// <param name="jsonPost"></param>
    /// <returns></returns>
    public static async Task<ExperimentResponseValues> GetSimulationResponse(string jsonPost)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://wt7.fei.stuba.sk/ovl-api/api/process");
        request.Headers["Authorization"] = "Bearer LwR1uDvk1Wr9Twv5LmjMulDdD7tsT7v3Vxp0IL7gMnMbnUi58g2IewOMnunK";
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonPost);
        }

        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        reader.Close();
        response.Close();
        ExperimentResponseValues result = JsonUtility.FromJson<ExperimentResponseValues>("{\"response\":" + json + "}");

        return result;
    }
}
