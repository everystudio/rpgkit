using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;

namespace rpgkit
{
    public class DataSkillParam : CsvDataParam
    {
        public int unit_id;
        public string skill_name;
        public string skill_detail;
        public string area;
        public int tp;
    }

    public class DataSkill : CsvData<DataSkillParam>
    {
    }
}





