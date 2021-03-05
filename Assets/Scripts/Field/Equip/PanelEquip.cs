using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit
{
    public class PanelEquip : MonoBehaviour
    {
        public List<EquipUnitIcon> m_equipUnitIconList;
        public EquipUnitView m_equipUnitView;
        public EquipStatusView m_equipStatusView;
        public EquipList m_equipList;

        public EquipInfo m_equipInfoCurrent;
        public EquipInfo m_equipInfoChange;

        private MasterUnitParam m_masterUnitSelected;
        private DataUnitParam m_dataUnitSelected;
        private DataUnitParam m_dummyUnit;

        public int m_iSelectingEquipIndex;
        private DataEquipParam m_dataEquipCurrent;
        private DataEquipParam m_dataEquipChange;

        public Button m_btnRemove;
        public Button m_btnSet;
        public Button m_btnReturn;

        private void Awake()
        {
            m_btnRemove.onClick.AddListener(() =>
            {
                FieldInfo fieldEquipSerial = m_dataUnitSelected.GetType().GetField($"equip{m_iSelectingEquipIndex}");

                int equip_serial = (int)fieldEquipSerial.GetValue(m_dataUnitSelected);
                if( 0 < equip_serial)
                {
                    DataEquipParam data_equip = DataManager.Instance.m_dataEquip.list.Find(p => p.equip_serial == equip_serial);
                    data_equip.equip_unit = 0;

                    foreach( EquipBanner banner in m_equipList.m_equipBannerList)
                    {
                        if( banner.dataEquipParam.equip_serial == equip_serial)
                        {
                            // 少し楽してます
                            banner.m_goEquipUnitRoot.SetActive(false);
                        }
                    }
                }

                fieldEquipSerial.SetValue(m_dataUnitSelected, 0);

                m_equipInfoCurrent.Clear();
                ResetStatusView();

            });

            // 装備するボタン
            m_btnSet.onClick.AddListener(() =>
            {
                m_btnSet.interactable = false;

                FieldInfo fieldEquipSerial = m_dataUnitSelected.GetType().GetField($"equip{m_iSelectingEquipIndex}");

                int tempEquipSerial = (int)fieldEquipSerial.GetValue(m_dataUnitSelected);
                if( 0 < tempEquipSerial && m_dataEquipCurrent != null)
                {
                    foreach (EquipBanner banner in m_equipList.m_equipBannerList)
                    {
                        if (banner.dataEquipParam.equip_serial == m_dataEquipCurrent.equip_serial)
                        {
                            banner.HideIcon();
                        }
                    }
                    m_dataEquipCurrent.equip_unit = 0;
                }
                fieldEquipSerial.SetValue(m_dataUnitSelected, m_dataEquipChange.equip_serial);
                m_dataEquipCurrent = m_dataEquipChange;
                m_dataEquipChange = null;

                m_dataEquipCurrent.equip_unit = m_dataUnitSelected.unit_id;
                foreach (EquipBanner banner in m_equipList.m_equipBannerList)
                {
                    if (banner.dataEquipParam.equip_serial == m_dataEquipCurrent.equip_serial)
                    {
                        // 少し楽してます
                        banner.ShowIcon(m_dataUnitSelected);
                    }
                }

                MasterEquipParam master_equip = DataManager.Instance.m_masterEquip.list.Find(p => p.equip_id == m_dataEquipCurrent.equip_id);
                m_equipInfoCurrent.Initialize(m_dataEquipChange, master_equip);
                m_equipInfoChange.Clear();
                ResetStatusView();

            });
            m_btnReturn.onClick.AddListener(() => UIAssistant.Instance.ShowParentPage());

			#region Event Unit Icon
			foreach ( EquipUnitIcon icon in m_equipUnitIconList)
            {
                icon.GetComponent<Button>().onClick.AddListener(() =>
                {
                    m_btnRemove.interactable = false;
                    m_btnSet.interactable = false;
                    m_dataUnitSelected = DataManager.Instance.m_dataUnit.list
                    .Find(p => p.unit_id == icon.m_masterUnitParam.unit_id);

                    m_masterUnitSelected = DataManager.Instance.m_masterUnit.list.Find(p => p.unit_id == m_dataUnitSelected.unit_id);

                    ResetStatusView();
                });
            }
            #endregion

            #region 装備中のアイコンとか
            m_equipUnitView.OnClickEquipIndex.AddListener((value) =>
            {
                m_iSelectingEquipIndex = value;
                m_btnSet.interactable = false;

                if (0 < value)
                {
                    FieldInfo fieldEquipType = m_masterUnitSelected.GetType().GetField($"equip_type{value}");
                    string equip_type = fieldEquipType.GetValue(m_masterUnitSelected).ToString();

                    FieldInfo fieldEquipSerial = m_dataUnitSelected.GetType().GetField($"equip{value}");
                    int equip_serial = (int)fieldEquipSerial.GetValue(m_dataUnitSelected);
                    m_dataEquipCurrent = DataManager.Instance.m_dataEquip.list.Find(p => p.equip_serial == equip_serial);
                    MasterEquipParam masterEquipParam = null;
                    if(m_dataEquipCurrent != null)
                    {
                        masterEquipParam = DataManager.Instance.m_masterEquip.list.Find(p => p.equip_id == m_dataEquipCurrent.equip_id);
                    }
                    m_equipInfoCurrent.Initialize(m_dataEquipCurrent, masterEquipParam);
                    m_equipInfoChange.Clear();
                    //Debug.Log($"<color=cyan>{equip_type}</color>");

                    m_btnRemove.interactable = m_dataEquipCurrent!=null;

                    m_equipList.Clear();
                    m_equipList.Show(DataManager.Instance.m_dataEquip.list, equip_type);
                }
            });
            #endregion

            // 装備するためのリスト
            m_equipList.OnDataEquip.AddListener((data) =>
            {
                m_equipInfoChange.Clear();
                m_dataEquipChange = data;
                MasterEquipParam master = null;
                if (m_dataEquipChange != null)
                {
                    master = DataManager.Instance.m_masterEquip.list.Find(p => p.equip_id == m_dataEquipChange.equip_id);
                }

                if(data.equip_unit != 0 && data.equip_unit != m_dataUnitSelected.unit_id)
                {
                    m_btnSet.interactable = false;
                }
                else
                {
                    m_btnSet.interactable = true;
                }

                m_equipInfoChange.Initialize(m_dataEquipChange, master);
            });

        }

        public void ResetStatusView()
        {
            m_dummyUnit = new DataUnitParam();
            m_dataUnitSelected.RefreshAssist(DataManager.Instance.m_masterEquip.list, DataManager.Instance.m_dataEquip.list);
            m_dummyUnit.AllCopy(m_dataUnitSelected);

            m_equipStatusView.Initialize(m_dataUnitSelected, m_dummyUnit);
            m_equipUnitView.Initialize(
                m_dataUnitSelected,
                DataManager.Instance.m_masterEquip.list,
                DataManager.Instance.m_dataEquip.list
                );
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

            List<DataUnitParam> party_unit_list = DataManager.Instance.m_dataUnit.list.FindAll(p => 0 < p.position);

            for ( int i = 0; i < m_equipUnitIconList.Count; i++)
            {
                if (i < party_unit_list.Count)
                {
                    MasterUnitParam master = DataManager.Instance.m_masterUnit.list
                        .Find(p => p.unit_id == party_unit_list[i].unit_id);
                    m_equipUnitIconList[i].gameObject.SetActive(true);
                    m_equipUnitIconList[i].Initialize(master);
                }
                else
                {
                    m_equipUnitIconList[i].gameObject.SetActive(false);
                }
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



