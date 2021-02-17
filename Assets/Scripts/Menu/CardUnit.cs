﻿using System.Collections;
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

        private DataUnitParam m_dataUnit;

        public Button m_btn;
        public DataUnitParamEvent m_onDataUnitParam = new DataUnitParamEvent();

        public void Initialize(DataUnitParam _unit)
        {
            m_dataUnit = _unit;
            if( m_btn == null)
            {
                m_btn = GetComponent<Button>();
                m_btn.onClick.AddListener(() =>
                {
                    m_onDataUnitParam.Invoke(m_dataUnit);
                });
            }
            m_txtName.text = _unit.unit_name;
            m_txtLevel.text = _unit.level.ToString();
            m_txtHPCurrent.text = _unit.hp_current.ToString();
            m_txtHPMax.text = _unit.hp_max.ToString();
            m_txtTPCurrent.text = _unit.tp_current.ToString();
            m_txtTPMax.text = _unit.tp_max.ToString();
        }

    }
}




