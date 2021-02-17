using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit {
    public class FieldManager : Singleton<FieldManager>
    {
        public string TargetTeleporterName;

        public FieldUnitCore m_unitCore;
        public FieldMenu m_menu;

        public Button m_btnAction;
        public Button m_btnMenu;

        private void Awake()
        {
            m_btnAction.onClick.AddListener(() =>
            {
                TalkBase tb = m_unitCore.GetComponent<FieldUnitSearcher>().m_talkTarget;
                Debug.Log(tb);
                if (tb != null)
                {
                    m_btnAction.gameObject.SetActive(false);
                    m_btnMenu.gameObject.SetActive(false);
                    m_unitCore.GetComponent<FieldUnitMover>().enabled = false;
                    StartCoroutine(TalkManager.Instance.Talk(tb.message, () =>
                    {
                        m_unitCore.GetComponent<FieldUnitMover>().enabled = true;
                        m_btnAction.gameObject.SetActive(true);
                        m_btnMenu.gameObject.SetActive(true);
                    }));
                }
            });

            m_btnMenu.onClick.AddListener(() =>
            {
                if (UIAssistant.Instance.GetCurrentPage() == "FieldIdle")
                {
                    m_unitCore.GetComponent<FieldUnitMover>().enabled = false;
                    UIAssistant.Instance.ShowPage("FieldMenuTop");
                    m_menu.OnClose.AddListener(() =>
                    {
                        m_unitCore.GetComponent<FieldUnitMover>().enabled = true;
                        m_menu.OnClose.RemoveAllListeners();
                    });
                }
            });


        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (UIAssistant.Instance.GetCurrentPage() == "FieldMenuTop")
                {
                    m_unitCore.GetComponent<FieldUnitMover>().enabled = true;
                    UIAssistant.Instance.ShowPage("none");
                }
            }
            else if(Input.GetKeyDown(KeyCode.Z))
            {
            }

        }


    }
}

