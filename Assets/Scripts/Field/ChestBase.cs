using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	public class ChestBase : MonoBehaviour,ISaveable
	{
		[System.Serializable]
		public struct SaveData
		{
			public string guid;
			public bool is_open;
		}

		public int item_id;
		public string Guid
		{
			get
			{
				if (string.IsNullOrEmpty(m_guid)){
					m_guid = System.Guid.NewGuid().ToString();
				}
				return m_guid;
			}
		}
		public string m_guid;
		public bool m_bIsOpen;
		
		[Header("Binding")]
		public GameObject m_Open;
		public GameObject m_Close;

		private void open(bool _bIsOpen)
		{
			//Debug.Log(_bIsOpen);
			m_Open.SetActive(_bIsOpen);
			m_Close.SetActive(!_bIsOpen);
		}

		public void Open()
		{
			open(true);
			m_bIsOpen = true;
		}
		public void Close()
		{
			open(false);
			m_bIsOpen = false;
		}

		public MasterItemParam GetItem()
		{
			MasterItemParam master = DataManager.Instance.m_masterItem.list
				.Find(p => p.item_id == item_id);

			if (master != null)
			{
				DataManager.Instance.m_dataItem.AddItem(master);
			}
			return master;
		}

		public string OnSave()
		{
			//Debug.Log("ChestBase.OnSave");
			return JsonUtility.ToJson(new SaveData
			{
				guid = Guid,
				is_open = m_bIsOpen
			}) ;
		}

		public void OnLoad(string data)
		{
			//Debug.Log("ChestBase.OnLoad");
			SaveData saveData = JsonUtility.FromJson<SaveData>(data);
			m_guid = saveData.guid;
			m_bIsOpen = saveData.is_open;
			open(m_bIsOpen);
			//Debug.Log(m_guid);
			//Debug.Log(m_bIsOpen);
		}

		public bool OnSaveCondition()
		{
			return true;
		}
	}
}



