using System.Collections.Generic;

[System.Serializable]
public class SimulationModel
{
    public List<Experiment> experiments;
}

[System.Serializable]
public class Experiment {
    public int id;
    public string experiment;
    public string controller;
    public string description;
    //public bool public;
    public TdModel tdmodel;
    public Program program;
    public List<Inputs> inputs;
    public List<Outputs> outputs;
    public List<Files> files;
}

[System.Serializable]
public class Program
{
    public int id;
    public string program;
    //public string object;
    public string url;
    public bool enabled;
}

[System.Serializable]
public class TdModel
{
    public int id;
    public string td_model;
    public bool enabled;
}

[System.Serializable]
public class Inputs
{
    public int id;
    public string schema_var;
    public string showed_var;
    public string type;
    public string group;
    public int order;
    public List<InputVals> inputvals;
}

[System.Serializable]
public class InputVals
{
    public int id;
    public string name;
    public double value;
}

[System.Serializable]
public class SelectOptionsHydraulic
{
    public List<SelectOptionExperiment> experiments;
}

[System.Serializable]
public class SelectOptionExperiment 
{
    public int id;
    public string experiment;
    public string controller;
}

[System.Serializable]
public class Outputs
{
    public int id;
    public string output;
    public int order;
}

[System.Serializable]
public class Files
{
    public int id;
    public string filename;
    public string url;
    public int file_type_id;
    public FileType filetype;
}

[System.Serializable]
public class FileType
{
    public int id;
    public string file_type;
}