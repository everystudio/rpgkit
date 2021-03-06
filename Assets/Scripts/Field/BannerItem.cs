using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace rpgkit
{
    public class BannerItem : MonoBehaviour
    {
        public Image m_imgIcon;
        public TextMeshProUGUI m_txtItemName;

        private MasterItemParam m_masterItemParam;
        private DataItemParam m_dataItemParam;

        public class OnBannerDataItemEvent : UnityEvent<DataItemParam>
        {
        }
        public class OnBannerMasterItemEvent : UnityEvent<MasterItemParam>
        {
        }
        public OnBannerDataItemEvent OnBannerDataItem = new OnBannerDataItemEvent();
        public OnBannerMasterItemEvent OnBannerMasterItem = new OnBannerMasterItemEvent();

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                OnBannerDataItem.Invoke(m_dataItemParam);
                OnBannerMasterItem.Invoke(m_masterItemParam);
            });
        }

        public void Initialize(MasterItemParam _master)
        {
            m_masterItemParam = _master;
            m_txtItemName.text = _master.item_name;
            if( _master.so_item != null)
            {
                m_imgIcon.sprite = _master.so_item.icon;
            }
        }

        public void Initialize(DataItemParam _param)
        {
            m_dataItemParam = _param;
            MasterItemParam master = DataManager.Instance.m_masterItem.list
                .Find((p => p.item_id == _param.item_id));
            Initialize(master);
        }
    }
}

