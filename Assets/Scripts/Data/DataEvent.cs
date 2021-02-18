using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
    public class DataEventParam : CsvDataParam
    {
        public int event_serial;
        public bool is_completed;
    }
    public class DataEvent : CsvData<DataEventParam>
    {

    }

}


