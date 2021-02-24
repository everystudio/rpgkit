using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace rpgkit
{
    public class BannerItem : MonoBehaviour
    {
        public TextMeshProUGUI m_txtItemName;
        public void Initialize(DataItemParam _param)
        {
            MasterItemParam master = DataManager.Instance.m_masterItem.list
                .Find((p => p.id == _param.id));

            m_txtItemName.text = master.name;
        }
    }
}

