using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;

namespace rpgkit
{
    public class MasterFlagParam : CsvDataParam
    {
        public int id;
        public string type;
        public string name;
    }
    public class MasterFlag : CsvData<MasterFlagParam>
    {
    }
}



