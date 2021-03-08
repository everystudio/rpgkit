using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	public class ItemList : MonoBehaviour
	{
		public GameObject m_prefBannerItem;
		public Transform m_tfRootBanner;

		public GameObject m_prefBannerEquip;

		private List<BannerItem> m_itemBannerList = new List<BannerItem>();

		[SerializeField]
		private IntVariable m_iSelectingItemSerial;
		private void Awake()
		{
			m_prefBannerItem.SetActive(false);
			m_prefBannerEquip.SetActive(false);
		}

		private void OnEnable()
		{
			ShowItem();
		}

		public void ShowItem()
		{
			m_iSelectingItemSerial.Value = 0;

			RPGKitUtil.DeleteObjects<EquipBanner>(m_tfRootBanner.gameObject);
			RPGKitUtil.DeleteObjects<BannerItem>(m_tfRootBanner.gameObject);
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
		public void ShowEquip()
		{
			RPGKitUtil.DeleteObjects<EquipBanner>(m_tfRootBanner.gameObject);
			RPGKitUtil.DeleteObjects<BannerItem>(m_tfRootBanner.gameObject);
			m_itemBannerList.Clear();

			List<DataEquipParam> type_list = DataManager.Instance.m_dataEquip.list;

			foreach (DataEquipParam data in type_list)
			{
				EquipBanner banner = Instantiate(m_prefBannerEquip, m_tfRootBanner).GetComponent<EquipBanner>();
				banner.gameObject.SetActive(true);

				MasterEquipParam master = DataManager.Instance.m_masterEquip.list.Find(p => p.equip_id == data.equip_id);
				banner.Initialize(master, data);
			}
		}




		private void OnDisable()
		{
		}




	}
}








