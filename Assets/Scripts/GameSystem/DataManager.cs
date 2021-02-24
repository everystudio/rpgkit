using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class DataManager : Singleton<DataManager>
    {
        public TextAsset m_taMasterItem;

        public TextAsset m_taDataUnit;
        public TextAsset m_taDataItem;
        public TextAsset m_taDataSkill;
        public TextAsset m_taMasterFlag;

        public MasterItem m_masterItem = new MasterItem();
        public MasterFlag m_masterFlag = new MasterFlag();

        public DataUnit m_dataUnit = new DataUnit();
        public DataItem m_dataItem = new DataItem();
        public DataSkill m_dataSkill = new DataSkill();
        public DataFlag m_dataFlag = new DataFlag();

        public override void Initialize()
        {
            base.Initialize();
            m_masterItem.Load(m_taMasterItem);
            m_masterFlag.Load(m_taMasterFlag);

            m_dataUnit.Load(m_taDataUnit);
            m_dataItem.Load(m_taDataItem);
            m_dataSkill.Load(m_taDataSkill);


        }
    }
}




