using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class SkillList : MonoBehaviour
    {
        public FieldMenu m_fieldMenu;
        public GameObject m_prefBannerItem;
        public Transform m_tfRootBanner;
        public int m_iUnitId;
        private List<BannerSkill> m_skillBannerList = new List<BannerSkill>();
        private void OnEnable()
        {
            m_prefBannerItem.SetActive(false);
            if (DataManager.Instance!= null)
            {
                Show(0);
            }
            m_fieldMenu.OnDataUnitParam.AddListener(Show);
        }
        private void OnDisable()
        {
            m_fieldMenu.OnDataUnitParam.RemoveListener(Show);
        }
        private void Show(DataUnitParam _data)
        {
            Show(_data.unit_id);
        }
        public void Show(int _iUnitId)
        {
            m_iUnitId = _iUnitId;
            foreach (BannerSkill banner in m_skillBannerList)
            {
                Destroy(banner.gameObject);
            }
            m_skillBannerList.Clear();

            List<DataSkillParam> skill_list = DataManager.Instance.m_dataSkill.list.
                FindAll(p => p.unit_id == m_iUnitId);

            foreach (DataSkillParam data in skill_list)
            {
                GameObject objSkill = Instantiate(m_prefBannerItem, m_tfRootBanner) as GameObject;
                objSkill.SetActive(true);
                BannerSkill banner = objSkill.GetComponent<BannerSkill>();
                banner.Initialize(data);
                m_skillBannerList.Add(banner);
            }

        }

    }
}


