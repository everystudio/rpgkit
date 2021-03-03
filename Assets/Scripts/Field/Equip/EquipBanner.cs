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

        public void Initialize(MasterEquipParam _equip)
        {
            if( m_imgIcon != null)
            {
                m_imgIcon.sprite = _equip != null ? _equip.so_equip.icon : null;
            }
            if( m_txtName != null)
            {
                m_txtName.text = _equip != null ? _equip.equip_name : "";
            }
            if( m_txtDesc != null)
            {
                m_txtDesc.text = "";
            }
        }
    }
}



