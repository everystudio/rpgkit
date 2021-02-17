using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit
{
	public class FieldMenuButton : MonoBehaviour
	{
		public string m_name;
		private void Awake()
		{
			gameObject.GetComponent<Button>().onClick.AddListener(() =>
			{
				if(m_name!= "")
				{
					UIAssistant.Instance.ShowPage(m_name);
				}
			});
		}
	}
}




