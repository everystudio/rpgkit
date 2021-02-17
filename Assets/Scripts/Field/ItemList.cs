using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	public class ItemList : MonoBehaviour
	{
		public GameObject m_prefBannerItem;
		public Transform m_tfRootBanner;

		private List<BannerItem> m_itemBannerList = new List<BannerItem>();

		private void Awake()
		{
			m_prefBannerItem.SetActive(false);
		}

		private void OnEnable()
		{
			foreach (BannerItem banner in m_itemBannerList)
			{
				Destroy(banner.gameObject);
			}
			m_itemBannerList.Clear();

			foreach (DataItemParam data in DataManager.Instance.m_dataItem.list)
			{
				GameObject objItem = Instantiate(m_prefBannerItem, m_tfRootBanner) as GameObject;
				objItem.SetActive(true);
				BannerItem banner = objItem.GetComponent<BannerItem>();
				banner.Initialize(data);
				m_itemBannerList.Add(banner);
			}

		}

		private void OnDisable()
		{
		}




	}
}








