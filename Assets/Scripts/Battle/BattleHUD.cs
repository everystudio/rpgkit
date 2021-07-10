using rpgkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
	public Button m_btnAttack;
	public Button m_btnSkill;
	public Button m_btnItem;
	public Button m_btnBack;

	public GameObject m_goRootPartyCard;
	public GameObject m_prefPartyCard;

	public Transform m_tfSelectChara;
	public Transform m_tfBottomStatus;
	public Transform m_tfSelectCommand;
	public Transform m_tfLogRoot;

	public List<CardUnit> cardUnitList = new List<CardUnit>();

	internal void Setup()
	{
		m_prefPartyCard.SetActive(false);
		cardUnitList.Clear();

		Debug.Log(m_tfSelectCommand.GetComponent<RectTransform>().anchoredPosition);

		m_tfSelectChara.GetComponent<RectTransform>().anchoredPosition = new Vector2(-960, 60);

		m_tfLogRoot.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800 + 30, -6);


	}

	public void ShowParty(List<MasterUnitParam> _masterList, List<DataUnitParam> _list)
	{
		foreach( DataUnitParam param in _list)
		{
			GameObject obj = Instantiate(m_prefPartyCard, m_goRootPartyCard.transform);
			obj.SetActive(true);
			CardUnit card = obj.GetComponent<CardUnit>();

			MasterUnitParam master = _masterList.Find(p => p.unit_id == param.unit_id);

			card.Initialize(param, master);
			card.SetPosition();
			cardUnitList.Add(card);
		}
	}
}
