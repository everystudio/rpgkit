using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using anogamelib;
using UnityEngine.InputSystem;
using anogamelib;

namespace rpgkit {
    public class FieldManager : Singleton<FieldManager>
    {
        public string TargetTeleporterName;

        public FieldUnitCore m_unitCore;
        public FieldMenu m_menu;

        public InputAction m_inputAction;
        public InputAction m_inputMenu;
        public InputAction m_inputCancel;

        public Button m_btnAction;
        public Button m_btnMenu;

        public CinemachineVirtualCamera m_vcamMain;
        public Room m_roomCurrent;

        private void OnEnable()
        {
            m_inputAction.Enable();
            m_inputMenu.Enable();
            m_inputCancel.Enable();
        }

        private void OnDisable()
        {
            m_inputAction.Disable();
            m_inputMenu.Disable();
            m_inputCancel.Disable();
        }

        public override void Initialize()
        {
            base.Initialize();

            m_inputAction.performed += M_inputAction_performed;
            m_inputMenu.performed += M_inputMenu_performed;

            m_btnAction.onClick.AddListener(() =>
            {
            });

            m_btnMenu.onClick.AddListener(() =>
            {
            });
        }

        private void M_inputAction_performed(InputAction.CallbackContext obj)
        {
            FieldUnitSearcher fus = m_unitCore.GetComponent<FieldUnitSearcher>();
            TalkBase tb = fus.m_talkTarget;
            ChestBase cb = fus.m_chestTarget;
            SequencePlayer sp = fus.m_sequencePlayer;

            //Debug.Log(tb);
            if (sp != null)
            {
                UnitFreeze(true);
                StartCoroutine(sp.PlaySequences(() =>
                {
                    UnitFreeze(false);
                }));
            }
            else if (tb != null)
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
            else if (cb != null)
            {
                ChestManager.Instance.OpenChest(cb);
            }
        }
        private void M_inputMenu_performed(InputAction.CallbackContext obj)
        {
            if (UIAssistant.Instance.GetCurrentPage() == "FieldIdle")
            {
                UnitFreeze(true);
                UIAssistant.Instance.ShowPage("FieldMenuTop");
                m_menu.OnClose.AddListener(() =>
                {
                    UnitFreeze(false);
                    m_menu.OnClose.RemoveAllListeners();
                });
            }
        }

        private void Update()
        {
            /*
            if(Input.GetKeyDown(KeyCode.Space))
            {
            }
            */
            if(m_inputCancel.phase == InputActionPhase.Performed)
            {
                if (UIAssistant.Instance.GetCurrentPage() == "FieldMenuTop")
                {
                    m_unitCore.GetComponent<FieldUnitMover>().enabled = true;
                    UIAssistant.Instance.ShowPage("none");
                }
            }
            /*
            else if(Input.GetKeyDown(KeyCode.Z))
            {
            }
            */
        }

        public void OnChangeScene(string _strSceneName)
        {
            //Debug.Log(_strSceneName);

            CinemachineVirtualCamera vcam = null;

            Room[] rooms = GameObject.FindObjectsOfType<Room>();
            //Debug.Log(rooms.Length);
            if( rooms.Length == 0)
            {
                vcam = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
            }
            else
            {
                foreach(Room r in rooms)
                {
                    r.m_vcam.Priority = 0;
                    if( r.GetComponent<Collider>().bounds.Contains(m_unitCore.transform.position))
                    {
                        vcam = r.m_vcam;
                    }
                }
                if( vcam == null)
                {
                    Debug.LogError("だめ");
                }
            }
            vcam.Priority = 10;
            vcam.Follow = m_unitCore.transform;
            m_vcamMain = vcam;
        }
        public void UnitFreeze(bool _bIsFreeze)
        {
            Debug.Log($"UnitFreeze({_bIsFreeze})");
            m_btnAction.gameObject.SetActive(!_bIsFreeze);
            m_btnMenu.gameObject.SetActive(!_bIsFreeze);
            if(_bIsFreeze)
            {
                m_unitCore.SetState(new FieldUnitCore.Freeze(m_unitCore));
            }
            else
            {
                m_unitCore.SetState(new FieldUnitCore.Idle(m_unitCore));
            }
        }
    }



}

