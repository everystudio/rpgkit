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
        public TextAsset m_taMasterEvent;

        public MasterEvent m_masterEvent = new MasterEvent();

        public DataUnit m_dataUnit = new DataUnit();
        public DataItem m_dataItem = new DataItem();
        public DataSkill m_dataSkill = new DataSkill();
        public DataEvent m_dataEvent = new DataEvent();



        public override void Initialize()
        {
            base.Initialize();

            m_dataUnit.Load(m_taDataUnit);
            m_dataItem.Load(m_taDataItem);
            m_dataSkill.Load(m_taDataSkill);

            m_masterEvent.Load(m_taMasterEvent);

        }
    }
}




