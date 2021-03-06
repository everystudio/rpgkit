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

		[SerializeField]
		private IntVariable m_iSelectingItemSerial;
		private void Awake()
		{
			m_prefBannerItem.SetActive(false);
		}

		private void OnEnable()
		{
			Show();
		}

		public void Show()
		{
			m_iSelectingItemSerial.Value = 0;
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

				banner.OnBannerDataItem.AddListener((value) =>
				{
					m_iSelectingItemSerial.Value = value.item_serial;
					UIAssistant.Instance.ShowPage("FieldMenuItemCheck");
				});
			}
		}

		private void OnDisable()
		{
		}




	}
}








