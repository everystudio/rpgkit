using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csvutility;

namespace rpgkit
{
    public class MasterUnitParam : CsvDataParam
    {
        public int unit_id;
        public string unit_name;

        public SOUnit so_unit_data;
    }

    public class MasterUnit : CsvData<MasterUnitParam>
    {
    }
}




