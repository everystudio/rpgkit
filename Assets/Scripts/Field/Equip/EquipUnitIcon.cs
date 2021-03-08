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
        public MasterUnitParam m_masterUnitParam;

        public void Initialize( MasterUnitParam _master)
        {
            m_masterUnitParam = _master;
            m_imgIcon.sprite = _master.so_unit_data.face_icon;
        }

        public void Select(bool _bSelect)
        {
            GetComponent<Image>().color = _bSelect ? new Color(0.5f, 1.0f, 1.0f) : Color.white;
        }

        public void Select(MasterUnitParam _master)
        {
            Select(m_masterUnitParam == _master);
        }
    }
}



