using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

namespace rpgkit
{
    public class EquipInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_txtEquipName;
        [SerializeField] private GameObject m_prefParamHolder;
        [SerializeField] private GameObject m_goHolderRoot;

        private List<GameObject> m_goParamHolderList = new List<GameObject>();

        public void Clear()
        {
            m_txtEquipName.text = "----";
            m_prefParamHolder.SetActive(false);
            foreach(GameObject obj in m_goParamHolderList)
            {
                Destroy(obj);
            }
            m_goParamHolderList.Clear();
        }

        public void Initialize( DataEquipParam _data , MasterEquipParam _master)
        {
            Clear();
            m_txtEquipName.text = _master != null ? _master.equip_name : "なし";
            if(_master== null)
            {
                return;
            }

            FieldInfo[] infoArr = new StatusParam().GetType().GetFields();
            foreach( FieldInfo info in infoArr)
            {
                FieldInfo master_info = _master.GetType().GetField(info.Name);
                if(master_info != null)
                {
                    int iParam = (int)master_info.GetValue(_master);
                    if( 0 != iParam)
                    {
                        string strMessage = "";
                        if (0 < iParam)
                        {
                            strMessage = $"{info.Name}<color=blue>+{iParam}</color>";
                        }
                        else if (iParam < 0)
                        {
                            strMessage = $"{info.Name}<color=red>-{iParam}</color>";
                        }
                        GameObject go = Instantiate(m_prefParamHolder, m_goHolderRoot.transform) as GameObject;
                        go.GetComponent<TextMeshProUGUI>().text = strMessage;
                        go.SetActive(true);
                        m_goParamHolderList.Add(go);
                    }
                }
            }
        }
    }
}








