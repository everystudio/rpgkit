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

        public string equip_type1;
        public string equip_type2;
        public string equip_type3;
        public string equip_type4;
        public string equip_type5;
        public string equip_type6;

        public SOUnit so_unit_data;
    }

    public class MasterUnit : CsvData<MasterUnitParam>
    {
    }
}




