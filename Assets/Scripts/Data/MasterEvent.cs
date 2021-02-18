using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

public class MasterEventParam : CsvDataParam
{
    public int event_serial;
    public string event_name;
    public bool completed;
}
public class MasterEvent : CsvData<MasterEventParam>
{
}

