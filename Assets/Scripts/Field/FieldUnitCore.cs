using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class FieldUnitCore : MonoBehaviour
    {
        public FieldUnitProperty m_fieldUnitProperty = new FieldUnitProperty();
        public GameObject m_goGameCanvas;

        private void Update()
        {
            /*
            if(Input.GetButtonDown("Jump"))
            {
                bool bShow = !m_goGameCanvas.activeSelf;
                m_goGameCanvas.SetActive(bShow);
                GetComponent<FieldUnitMover>().enabled = !bShow;
            }
            */
        }
    }




}

