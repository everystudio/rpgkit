using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using anogamelib;

namespace rpgkit
{
    public class DataManager : Singleton<DataManager>
    {
        [SerializeField] private TextAsset m_taMasterUnit;
        [SerializeField] private TextAsset m_taMasterItem;
        [SerializeField] private TextAsset m_taMasterEquip;
        [SerializeField] public TextAsset m_taMasterFlag;

        [SerializeField] private TextAsset m_taDataUnit;
        [SerializeField] private TextAsset m_taDataItem;
        [SerializeField] private TextAsset m_taDataSkill;
        [SerializeField] private TextAsset m_taDataEquip;
        [SerializeField] private TextAsset m_taDataFlag;

        public MasterUnit m_masterUnit = new MasterUnit();
        public MasterItem m_masterItem = new MasterItem();
        public MasterEquip m_masterEquip = new MasterEquip();
        public MasterFlag m_masterFlag = new MasterFlag();

        public List<SOUnit> so_unit_list = new List<SOUnit>();
        public List<SOEquip> so_equip_list = new List<SOEquip>();
        public List<SOItem> so_item_list = new List<SOItem>();

        public DataUnit m_dataUnit = new DataUnit();
        public DataItem m_dataItem = new DataItem();
        public DataSkill m_dataSkill = new DataSkill();
        public DataFlag m_dataFlag = new DataFlag();
        public DataEquip m_dataEquip = new DataEquip();

        public override void Initialize()
        {
            base.Initialize();
            m_masterUnit.Load(m_taMasterUnit);
            m_masterItem.Load(m_taMasterItem);
            m_masterEquip.Load(m_taMasterEquip);
            m_masterFlag.Load(m_taMasterFlag);

            m_dataUnit.Load(m_taDataUnit);
            m_dataItem.Load(m_taDataItem);
            m_dataSkill.Load(m_taDataSkill);
            m_dataEquip.Load(m_taDataEquip);
            m_dataFlag.Load(m_taDataFlag);

            foreach (MasterUnitParam unit in m_masterUnit.list)
            {
                unit.so_unit_data = so_unit_list.Find(p => p.unit_id == unit.unit_id);
            }
            foreach ( MasterEquipParam equip in m_masterEquip.list)
            {
                equip.so_equip = so_equip_list.Find(p => p.equip_id == equip.equip_id);
            }
            foreach( MasterItemParam item in m_masterItem.list)
            {
                item.so_item = so_item_list.Find(p => p.item_id == item.item_id);
            }

            foreach( DataUnitParam unit in m_dataUnit.list) {
                unit.RefreshAssist(m_masterEquip.list, m_dataEquip.list);
            }

            /*
            FieldInfo[] infoArr = new StatusParam().GetType().GetFields();
            foreach( MasterEquipParam eq in m_masterEquip.list)
            {
                foreach (FieldInfo info in infoArr)
                {
                    //Debug.Log(info.Name);
                    FieldInfo master_info = eq.GetType().GetField(info.Name);
                    if (master_info != null)
                    {
                        int param = (int)master_info.GetValue(eq);
                        //Debug.Log($"{info.Name}={param}");
                    }
                }
            }
            */


        }
    }
}




