using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace rpgkit
{
    public class OnEquipBanner : UnityEvent<EquipBanner>
    {
    }

    public class EquipBanner : MonoBehaviour
    {
        public Image m_imgIcon;
        public TextMeshProUGUI m_txtName;
        public TextMeshProUGUI m_txtDesc;
        public GameObject m_goEquipUnitRoot;
        public Image m_imgEquipUnit;

        private int m_iEquipIndex;
        public int Index => m_iEquipIndex;
        private MasterEquipParam m_masterEquipParam;
        private DataEquipParam m_dataEquipParam;
        public DataEquipParam dataEquipParam => m_dataEquipParam;

        public UnityEventInt OnClickIndex = new UnityEventInt();
        public MasterEquipEvent OnClickEquip = new MasterEquipEvent();
        public DataEquipEvent OnclickDataEquip = new DataEquipEvent();
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => {
                OnClickIndex.Invoke(m_iEquipIndex);
                OnClickEquip.Invoke(m_masterEquipParam);
                OnclickDataEquip.Invoke(m_dataEquipParam);
            });
        }
        public void HideIcon()
        {
            if (m_goEquipUnitRoot != null)
            {
                m_goEquipUnitRoot.SetActive(false);
            }
        }
        public void ShowIcon(DataUnitParam _unit)
        {
            if (m_goEquipUnitRoot != null)
            {
                m_goEquipUnitRoot.SetActive(true);
                MasterUnitParam master_unit = DataManager.Instance.m_masterUnit.list.Find(p => p.unit_id == _unit.unit_id);
                m_imgEquipUnit.sprite = master_unit.so_unit_data.face_icon;
            }
        }

        public void Initialize(MasterEquipParam _equip , DataEquipParam _dataEquip, int _iIndex)
        {
            m_masterEquipParam = _equip;
            m_dataEquipParam = _dataEquip;

            if( m_goEquipUnitRoot!= null)
            {
                if (0 < m_dataEquipParam.equip_unit)
                {
                    m_goEquipUnitRoot.SetActive(true);

                    MasterUnitParam master_unit = DataManager.Instance.m_masterUnit.list.Find(p => p.unit_id == m_dataEquipParam.equip_unit);
                    m_imgEquipUnit.sprite = master_unit.so_unit_data.face_icon;
                }
                else
                {
                    m_goEquipUnitRoot.SetActive(false);
                }
            }



            m_iEquipIndex = _iIndex;
            if ( m_imgIcon != null)
            {
                m_imgIcon.sprite = _equip != null ? _equip.so_equip.icon : null;
            }
            if( m_txtName != null)
            {
                m_txtName.text = _equip != null ? _equip.equip_name : "なし";
            }
            if( m_txtDesc != null)
            {
                m_txtDesc.text = "";
            }
        }
        public void Initialize(MasterEquipParam _masterEquip , DataEquipParam _dataEquip)
        {
            Initialize(_masterEquip, _dataEquip, 0);
        }
    }
}



