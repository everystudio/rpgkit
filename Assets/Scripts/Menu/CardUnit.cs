using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace rpgkit
{
    public class CardUnit : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_txtName;
        [SerializeField]
        private TextMeshProUGUI m_txtLevel;
        [SerializeField]
        private TextMeshProUGUI m_txtHPCurrent;
        [SerializeField]
        private TextMeshProUGUI m_txtHPMax;
        [SerializeField]
        private TextMeshProUGUI m_txtTPCurrent;
        [SerializeField]
        private TextMeshProUGUI m_txtTPMax;

        [SerializeField]
        private Image m_imgIcon;

        private DataUnitParam m_dataUnit;
        private MasterUnitParam m_masterUnitParam;

        public bool IsUnit( DataUnitParam _unitParam)
        {
            return m_dataUnit == _unitParam;
        }

        public Button m_btn;
        public DataUnitParamEvent m_onDataUnitParam = new DataUnitParamEvent();

        public void SelectCard(bool _bIsSelect)
        {
            GetComponent<Image>().color = _bIsSelect ? new Color(0.5f, 1.0f, 1.0f) : Color.white;
        }

        public void Initialize(DataUnitParam _unit , MasterUnitParam _master = null)
        {
            m_dataUnit = _unit;
            if( _master == null)
            {
                m_masterUnitParam = DataManager.Instance.m_masterUnit.list.Find(p => p.unit_id == _unit.unit_id);
            }
            else
            {
                m_masterUnitParam = _master;
            }
            if ( m_btn == null)
            {
                m_btn = GetComponent<Button>();
            }
            m_btn.onClick.AddListener(() =>
            {
                m_onDataUnitParam.Invoke(m_dataUnit);
            });
            m_txtName.text = m_masterUnitParam.unit_name;
            RefreshParam();
            if (m_imgIcon != null)
            {
                m_imgIcon.sprite = m_masterUnitParam.so_unit_data.unit_left;
            }
        }
        public void RefreshParam()
        {
            m_txtLevel.text = m_dataUnit.level.ToString();
            m_txtHPCurrent.text = m_dataUnit.hp_current.ToString();
            m_txtHPMax.text = m_dataUnit.hp.ToString();
            m_txtTPCurrent.text = m_dataUnit.tp_current.ToString();
            m_txtTPMax.text = m_dataUnit.tp.ToString();
        }

    }
}




