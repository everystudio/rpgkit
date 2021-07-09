using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;
using UnityEngine.Events;

namespace rpgkit
{
    public class MasterEquipEvent : UnityEvent<MasterEquipParam>
    {
    }


    public class MasterEquipParam : StatusParam
    {
        public int equip_id;
        public string equip_name;
        public string equip_type;
        public int price;

        public string special1;
        public string special2;

        public SOEquip so_equip;
    }

    public class MasterEquip : CsvData<MasterEquipParam>
    {
    }
}




