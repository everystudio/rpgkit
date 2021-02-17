using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class DataManager : Singleton<DataManager>
    {
        public TextAsset m_taDataUnit;
        public TextAsset m_taDataItem;
        public TextAsset m_taDataSkill;

        public DataUnit m_dataUnit = new DataUnit();
        public DataItem m_dataItem = new DataItem();
        public DataSkill m_dataSkill = new DataSkill();

        public override void Initialize()
        {
            base.Initialize();

            m_dataUnit.Load(m_taDataUnit);
            m_dataItem.Load(m_taDataItem);
            m_dataSkill.Load(m_taDataSkill);

            foreach( DataSkillParam skill in m_dataSkill.list)
            {
                DataUnitParam user = m_dataUnit.list.Find(p => p.unit_id == skill.unit_id);

                Debug.Log(string.Format("使用者({0}):スキル名「{1}」",
                    user.unit_name,
                    skill.skill_name));
            }



        }
    }
}




