using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace rpgkit
{
	public class BannerSkill : MonoBehaviour
	{
		public TextMeshProUGUI m_txtSkillName;
		public TextMeshProUGUI m_txtSkillDetail;
		public TextMeshProUGUI m_txtSkillArea;
		public TextMeshProUGUI m_txtSkillTP;

		private DataSkillParam m_dataSkillParam;
		
		public void Initialize( DataSkillParam _data)
		{
			m_dataSkillParam = _data;
			m_txtSkillName.text = m_dataSkillParam.skill_name;
			m_txtSkillDetail.text = m_dataSkillParam.skill_detail;
			m_txtSkillArea.text = m_dataSkillParam.area;
			m_txtSkillTP.text = m_dataSkillParam.tp.ToString();
		}
	}
}






