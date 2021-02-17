using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit
{
    public class FieldMenuButtonManager : MonoBehaviour
    {
        public List<FieldMenuButton> buttn_list;

        private void Awake()
        {
            foreach( FieldMenuButton btn in buttn_list)
            {
                btn.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log(btn.m_name);
                });
            }
        }
    }
}



