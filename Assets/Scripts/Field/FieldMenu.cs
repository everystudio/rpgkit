using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace rpgkit
{
	public class FieldMenu : MonoBehaviour
	{
		public Button m_btnItem;

		public GameObject m_goItemList;

		public GameObject m_prefUnitCard;
		public Transform m_tfRootUnitCard;

		public List<CardUnit> m_cardUnitList = new List<CardUnit>();
		public DataUnitParamEvent OnDataUnitParam = new DataUnitParamEvent();

		public UnityEvent OnClose = new UnityEvent();

		private void Awake()
		{
			m_prefUnitCard.SetActive(false);
		}

		public void SelectCardUnit( DataUnitParam _dataUnitParam)
		{
			foreach (CardUnit card in m_cardUnitList)
			{
				card.SelectCard(card.IsUnit(_dataUnitParam));
			}

		}

		private void OnEnable()
		{
			foreach (CardUnit card in m_cardUnitList)
			{
				Destroy(card.gameObject);
			}
			m_cardUnitList.Clear();

			foreach ( DataUnitParam unit_param in DataManager.Instance.m_dataUnit.list)
			{
				GameObject objCard = Instantiate(m_prefUnitCard, m_tfRootUnitCard) as GameObject;
				objCard.SetActive(true);
				CardUnit card = objCard.GetComponent<CardUnit>();
				card.Initialize(unit_param);
				card.SelectCard(false);
				m_cardUnitList.Add(card);

				card.m_onDataUnitParam.AddListener((value) =>
				{
					OnDataUnitParam.Invoke(value);
				});
			}

		}
		private void OnDisable()
		{
			OnClose.Invoke();
		}

	}
}




