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
            m_txtItemName.text = _param.item_name;
        }
    }
}

