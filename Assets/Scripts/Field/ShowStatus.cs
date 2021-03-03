using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace rpgkit
{
	public class ShowStatus : MonoBehaviour
	{
		public FieldMenu m_fieldMenu;
		public TextMeshProUGUI m_txtLevel;
		public TextMeshProUGUI m_txtHP;
		public TextMeshProUGUI m_txtTP;
		public TextMeshProUGUI m_txtAttack;
		public TextMeshProUGUI m_txtDefence;
		public TextMeshProUGUI m_txtSpeed;
		public TextMeshProUGUI m_txtMind;
		public TextMeshProUGUI m_txtWisdom;

		public GameObject m_goRoot;

		private void OnEnable()
		{
			m_goRoot.SetActive(false);
			m_fieldMenu.OnDataUnitParam.AddListener(Show);
		}

		private void OnDisable()
		{
			m_fieldMenu.OnDataUnitParam.RemoveListener(Show);
		}

		private void Show(DataUnitParam _unit)
		{
			m_goRoot.SetActive(true);

			m_txtLevel.text = $"Level:{_unit.level}";
			m_txtHP.text = $"HP:{_unit.hp_current}/{_unit.hp}";
			m_txtTP.text = $"TP:{_unit.tp_current}/{_unit.tp}";
			m_txtAttack.text = $"{_unit.attack}";
			m_txtDefence.text = $"{_unit.defense}";
			m_txtSpeed.text = $"{_unit.speed}";
			m_txtMind.text = $"{_unit.mind}";
			m_txtWisdom.text = $"{_unit.wisdom}";
	}

	}
}







