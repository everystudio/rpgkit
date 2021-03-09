using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
    public class DataFlagParam : CsvDataParam
    {
        public int flag_id;
        public bool is_completed;
    }
    public class DataFlag : CsvData<DataFlagParam>
    {
        public bool Check( int _iFlagId)
        {
            DataFlagParam flag = list.Find(p => p.flag_id == _iFlagId);
            return flag != null ? flag.is_completed : false;
        }
    }

}


