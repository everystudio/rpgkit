using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class EquipList : MonoBehaviour
    {
        public GameObject m_prefBanner;
        public Transform m_tfBannerRoot;

        public DataEquipEvent OnDataEquip = new DataEquipEvent();

        public void Clear()
        {
            m_prefBanner.SetActive(false);
            RPGKitUtil.DeleteObjects<EquipBanner>(m_tfBannerRoot.gameObject);
        }

        public void Show(List<DataEquipParam> _list , string _strEquipType)
        {
            List<DataEquipParam> type_list = _list.FindAll(p => p.equip_type == _strEquipType);

            foreach( DataEquipParam data in type_list)
            {
                EquipBanner banner = Instantiate(m_prefBanner, m_tfBannerRoot).GetComponent<EquipBanner>();
                banner.gameObject.SetActive(true);

                MasterEquipParam master = DataManager.Instance.m_masterEquip.list.Find(p => p.equip_id == data.equip_id);
                banner.Initialize(master, data);
                banner.OnclickDataEquip.AddListener((value) =>
                {
                    OnDataEquip.Invoke(value);
                });

            }
        }



    }
}


