using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace rpgkit
{
    public class EquipStatusView : MonoBehaviour
    {
        public struct ParamTextSet
        {
            public TextMeshProUGUI m_txtHP;
            public TextMeshProUGUI m_txtTP;
            public TextMeshProUGUI m_txtAttack;
            public TextMeshProUGUI m_txtDefense;
            public TextMeshProUGUI m_txtSpeed;
            public TextMeshProUGUI m_txtMind;
            public TextMeshProUGUI m_txtWisdom;

            public void SetComponent(Transform _root , string _strName )
            {
                m_txtHP = _root.Find($"statusParamHP/{_strName}").GetComponent<TextMeshProUGUI>();
                m_txtTP = _root.Find($"statusParamTP/{_strName}").GetComponent<TextMeshProUGUI>();
                m_txtAttack = _root.Find($"statusParamAttack/{_strName}").GetComponent<TextMeshProUGUI>();
                m_txtDefense = _root.Find($"statusParamDefense/{_strName}").GetComponent<TextMeshProUGUI>();
                m_txtSpeed = _root.Find($"statusParamSpeed/{_strName}").GetComponent<TextMeshProUGUI>();
                m_txtMind = _root.Find($"statusParamMind/{_strName}").GetComponent<TextMeshProUGUI>();
                m_txtWisdom = _root.Find($"statusParamWisdom/{_strName}").GetComponent<TextMeshProUGUI>();
            }
        }
        private ParamTextSet m_txtsetPrev;
        private ParamTextSet m_txtsetNext;

        public void Initialize(DataUnitParam _unit , DataUnitParam _change)
        {
            _unit.RefreshAssist(DataManager.Instance.m_masterEquip.list);
            _change.RefreshAssist(DataManager.Instance.m_masterEquip.list);

            m_txtsetPrev.SetComponent(transform.Find("paramRoot"), "params/txtPrev");
            m_txtsetNext.SetComponent(transform.Find("paramRoot"), "params/txtNext");

            m_txtsetPrev.m_txtHP.text = _unit.GetStatus("hp").ToString();
            m_txtsetPrev.m_txtTP.text = _unit.GetStatus("tp").ToString();
            m_txtsetPrev.m_txtAttack.text = _unit.GetStatus("attack").ToString();
            m_txtsetPrev.m_txtDefense.text = _unit.GetStatus("defense").ToString();
            m_txtsetPrev.m_txtSpeed.text = _unit.GetStatus("speed").ToString();
            m_txtsetPrev.m_txtMind.text = _unit.GetStatus("mind").ToString();
            m_txtsetPrev.m_txtWisdom.text = _unit.GetStatus("wisdom").ToString();

            Diff(_unit, _change);
        }

        public void Diff(DataUnitParam _unit, DataUnitParam _change)
        {
            int iHP = _change.GetStatus("hp") - _unit.GetStatus("hp");
            int iTP = _change.GetStatus("tp") - _unit.GetStatus("tp");
            int iAttack = _change.GetStatus("attack") - _unit.GetStatus("attack");
            int iDefense = _change.GetStatus("defense") - _unit.GetStatus("defense");
            int iSpeed = _change.GetStatus("speed") - _unit.GetStatus("speed");
            int iMind = _change.GetStatus("mind") - _unit.GetStatus("mind");
            int iWisdom = _change.GetStatus("wisdom") - _unit.GetStatus("wisdom");

            int[] param_arr = new int[]
            {
                iHP,
                iTP,
                iAttack,
                iDefense,
                iSpeed,
                iMind,
                iWisdom
            };
            TextMeshProUGUI[] tmp_arr = new TextMeshProUGUI[]
            {
                m_txtsetNext.m_txtHP,
                m_txtsetNext.m_txtTP,
                m_txtsetNext.m_txtAttack,
                m_txtsetNext.m_txtDefense,
                m_txtsetNext.m_txtSpeed,
                m_txtsetNext.m_txtMind,
                m_txtsetNext.m_txtWisdom,
            };
            for( int i = 0; i < param_arr.Length; i++)
            {
                if( 0 < param_arr[i])
                {
                    tmp_arr[i].text = param_arr[i].ToString();
                    tmp_arr[i].color = Color.blue;
                }
                else if( param_arr[i] < 0)
                {
                    tmp_arr[i].text = param_arr[i].ToString();
                    tmp_arr[i].color = Color.red;
                }
                else
                {
                    tmp_arr[i].text = "";
                }
            }

        }
    }
}



