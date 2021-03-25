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
        public void Write( int _iFlagId , bool _bIsCompleted)
        {
            DataFlagParam flag = list.Find(p => p.flag_id == _iFlagId);
            if( flag == null)
            {
                flag = new DataFlagParam();
                flag.flag_id = _iFlagId;
                flag.is_completed = _bIsCompleted;
                list.Add(flag);
            }
            else
            {
                flag.is_completed = _bIsCompleted;
            }
        }
    }

}


