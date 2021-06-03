using System.Collections.Generic;

[System.Serializable]
public class ExperimentResponseValues
{
    public List<ResponseValues> response;
}

[System.Serializable]
public class ResponseValues
{
    public double time;
    public double h1;
    public double h2;
    public double h3;
}