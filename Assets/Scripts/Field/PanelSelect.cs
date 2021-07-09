using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using anogamelib;

namespace rpgkit
{
    public class PanelSelect : Singleton<PanelSelect>
    {
        public GameObject m_goRoot;
        public TextMeshProUGUI m_txtMessage;

        public Button m_btn0;
        public Button m_btn1;
        public Button m_btn2;

        private Action<int> OnSelectButton;

        private void Awake()
        {
            Debug.Log(TalkManager.Instance);
            m_goRoot.SetActive(false);
            m_btn0.onClick.AddListener(() =>
            {
                SelectButtonHandler(0);
            });
            m_btn1.onClick.AddListener(() =>
            {
                SelectButtonHandler(1);
            });
            m_btn2.onClick.AddListener(() =>
            {
                SelectButtonHandler(2);
            });
        }

        public void Request(string _strMessage , List<string> _strSelectList , Action<int> _onSelected)
        {
            m_btn0.gameObject.SetActive(false);
            m_btn1.gameObject.SetActive(false);
            m_btn2.gameObject.SetActive(false);

            GameObject[] btnArr = new GameObject[]
            {
                m_btn0.gameObject,
                m_btn1.gameObject,
                m_btn2.gameObject,
            };

            m_txtMessage.text = _strMessage;
            for( int i = 0; i < _strSelectList.Count; i++)
            {
                btnArr[i].transform.Find("txtButton").GetComponent<Text>().text = _strSelectList[i];
            }
            OnSelectButton = _onSelected;
            m_goRoot.SetActive(true);
        }

        public void SelectButtonHandler(int _iIndex)
        {
            if(OnSelectButton != null)
            {
                OnSelectButton.Invoke(_iIndex);
            }
            m_goRoot.SetActive(false);

        }
    }
}




