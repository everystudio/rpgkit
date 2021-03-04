using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit
{
    public class PanelEquip : MonoBehaviour
    {
        public EquipUnitView m_equipUnitView;
        public EquipStatusView m_equipStatusView;
        public EquipList m_equipList;

        public EquipInfo m_equipInfoCurrent;
        public EquipInfo m_equipInfoChange;

        private MasterUnitParam m_masterUnitSelected;
        private DataUnitParam m_dataUnitSelected;
        private DataUnitParam m_dummyUnit;

        public List<EquipUnitIcon> m_equipUnitIconList;

        private void Awake()
        {
            foreach( EquipUnitIcon icon in m_equipUnitIconList)
            {
                icon.GetComponent<Button>().onClick.AddListener(() =>
                {
                    m_dataUnitSelected = DataManager.Instance.m_dataUnit.list
                    .Find(p => p.unit_id == icon.m_masterUnitParam.unit_id);

                    m_masterUnitSelected = DataManager.Instance.m_masterUnit.list.Find(p => p.unit_id == m_dataUnitSelected.unit_id);

                    m_dummyUnit = new DataUnitParam();
                    m_dataUnitSelected.RefreshAssist(DataManager.Instance.m_masterEquip.list , DataManager.Instance.m_dataEquip.list);
                    m_dummyUnit.AllCopy(m_dataUnitSelected);

                    m_equipStatusView.Initialize(m_dataUnitSelected, m_dummyUnit);
                    m_equipUnitView.Initialize(
                        m_dataUnitSelected,
                        DataManager.Instance.m_masterEquip.list,
                        DataManager.Instance.m_dataEquip.list
                        );
                });
            }

            m_equipUnitView.OnClickEquipIndex.AddListener((value) =>
            {
                if (0 < value)
                {
                    FieldInfo fieldEquipType = m_masterUnitSelected.GetType().GetField($"equip_type{value}");
                    string equip_type = fieldEquipType.GetValue(m_masterUnitSelected).ToString();

                    FieldInfo fieldEquipSerial = m_dataUnitSelected.GetType().GetField($"equip{value}");
                    int equip_serial = (int)fieldEquipSerial.GetValue(m_dataUnitSelected);
                    DataEquipParam dataEquipParam = DataManager.Instance.m_dataEquip.list.Find(p => p.equip_serial == equip_serial);
                    MasterEquipParam masterEquipParam = null;
                    if( dataEquipParam != null)
                    {
                        masterEquipParam = DataManager.Instance.m_masterEquip.list.Find(p => p.equip_id == dataEquipParam.equip_id);
                    }
                    m_equipInfoCurrent.Initialize(dataEquipParam, masterEquipParam);
                    m_equipInfoChange.Clear();
                    //Debug.Log($"<color=cyan>{equip_type}</color>");

                    m_equipList.Clear();
                    m_equipList.Show(DataManager.Instance.m_dataEquip.list, equip_type);

                }
            });

            m_equipList.OnDataEquip.AddListener((data) =>
            {
                m_equipInfoChange.Clear();
                MasterEquipParam master = null;
                if (data != null)
                {
                    master = DataManager.Instance.m_masterEquip.list.Find(p => p.equip_id == data.equip_id);
                }
                m_equipInfoChange.Initialize(data, master);
            });

        }


        private void OnEnable()
        {
            if(DataManager.Instance == null)
            {
                return;
            }

            m_equipUnitView.Clear();
            m_equipStatusView.Clear();
            m_equipList.Clear();

            m_equipInfoCurrent.Clear();
            m_equipInfoChange.Clear();

            for ( int i = 0; i < m_equipUnitIconList.Count; i++)
            {
                MasterUnitParam master = DataManager.Instance.m_masterUnit.list
                    .Find(p => p.unit_id == DataManager.Instance.m_dataUnit.list[i].unit_id);
                m_equipUnitIconList[i].Initialize(master);
            }

            /*
            m_equipStatusView.Initialize(DataManager.Instance.m_dataUnit.list[0] , m_dummyUnit);
            m_equipUnitView.Initialize(
                DataManager.Instance.m_dataUnit.list[0],
                DataManager.Instance.m_masterEquip.list
                );
                */
        }

    }
}



