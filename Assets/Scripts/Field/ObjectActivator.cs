using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class ObjectActivator : MonoBehaviour
    {
        public int flag_id;
        public bool is_active;
        void Start()
        {
            if (DataManager.Instance.m_dataFlag.Check(flag_id))
            {
                gameObject.SetActive(!is_active);
            }
            else
            {
                gameObject.SetActive(is_active);
            }
        }

    }
}