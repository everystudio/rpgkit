using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using anogamelib;

namespace rpgkit
{
    public class ItemUseCheck : MonoBehaviour
    {
        [SerializeField]
        private FieldMenu m_fieldMenu;
        [SerializeField]
        private BannerItem m_headerBanner;
        [SerializeField]
        private TextMeshProUGUI m_txtDescription;

        [SerializeField]
        private IntVariable m_iSelectingItemSerial;

        private DataItemParam m_dataItemParam;
        private MasterItemParam m_masterItemParam;
        private DataUnitParam m_selectingUnitParam;

        [SerializeField]
        private ItemList m_itemList;

        [SerializeField]
        private Button m_btnUse;
        [SerializeField]
        private Button m_btnCancel;

        private void Awake()
        {
            m_btnUse.onClick.AddListener(() =>
            {
                UseItem();
            });
        }

        private void OnEnable()
        {
            if( m_iSelectingItemSerial.Value == 0) {
                return;
            }
            m_dataItemParam = DataManager.Instance.m_dataItem.list.Find(p=>p.item_serial == m_iSelectingItemSerial.Value);
            m_masterItemParam = DataManager.Instance.m_masterItem.list.Find(p => p.item_id == m_dataItemParam.item_id);

            m_headerBanner.Initialize(m_dataItemParam);

            m_btnUse.gameObject.SetActive(m_masterItemParam.field);

            m_txtDescription.text = m_masterItemParam.description;

            // 味方全体の場合はいきなり使うボタンが押せる
            if ( m_masterItemParam.item_target == "friendall")
            {
                m_btnUse.interactable = true;
            }
            else if(m_masterItemParam.item_target == "friendone")
            {
                m_btnUse.interactable = false;
                m_fieldMenu.OnDataUnitParam.AddListener(SelectCardUnit);
            }
            else
            {
                Debug.LogError( $"okashi_item_target:{m_masterItemParam.item_target}");
            }
        }
        private void OnDisable()
        {
            if(m_masterItemParam == null)
            {
                return;
            }
            // 味方全体の場合はいきなり使うボタンが押せる
            if (m_masterItemParam.item_target == "friendall")
            {
            }
            else if (m_masterItemParam.item_target == "friendone")
            {
                m_fieldMenu.OnDataUnitParam.RemoveListener(SelectCardUnit);
            }
            else
            {
                Debug.LogError($"okashi_item_target:{m_masterItemParam.item_target}");
            }
            m_fieldMenu.SelectCardUnit(null);

        }
        private void SelectCardUnit(DataUnitParam _selectUnit)
        {
            m_selectingUnitParam = _selectUnit;
            m_fieldMenu.SelectCardUnit(m_selectingUnitParam);
            m_btnUse.interactable = true;
        }

        public void UseItem()
        {
            List<DataUnitParam> target_unit_list = new List<DataUnitParam>();
            if (m_masterItemParam.item_target == "friendone")
            {
                target_unit_list.Add(m_selectingUnitParam);
            }
            else
            {
                target_unit_list = DataManager.Instance.m_dataUnit.list.FindAll(p => 0 < p.position);
            }

            foreach( DataUnitParam unit in target_unit_list)
            {
                if( m_masterItemParam.item_type == "hpheal")
                {
                    unit.hp_current = Mathf.Min(unit.GetStatus("hp"), unit.hp_current + m_masterItemParam.item_param);
                }
                else if(m_masterItemParam.item_type == "tpheal")
                {
                    unit.tp_current = Mathf.Min(unit.GetStatus("tp"), unit.tp_current + m_masterItemParam.item_param);
                }
            }
            DataManager.Instance.m_dataItem.list.Remove(m_dataItemParam);
            m_itemList.ShowItem();
            m_btnCancel.onClick.Invoke();

            foreach( CardUnit card in m_fieldMenu.m_cardUnitList)
            {
                card.RefreshParam();
            }
        }
    }
}





