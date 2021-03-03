using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit
{
    public class EquipUnitIcon : MonoBehaviour
    {
        public Button m_btn;
        public Image m_imgIcon;

        public void Initialize( MasterUnitParam _master)
        {
            m_imgIcon.sprite = _master.so_unit_data.face_icon;
        }
    }
}



