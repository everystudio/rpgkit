using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace rpgkit
{
	public class TalkManager : Singleton<TalkManager>
	{
		public TextMeshProUGUI m_txtMessage;
		public Animator m_animator;
		public Button m_btnWindow;

		public IEnumerator Talk(string _strMessage , Action _onFinish)
		{
			m_txtMessage.text = "";

			m_animator.SetBool("show", true);
			yield return new WaitForSeconds(0.3f);

			m_txtMessage.text = _strMessage;

			bool bSkip = false;
			m_btnWindow.onClick.AddListener(() =>
			{
				bSkip = true;
			});

			while (bSkip == false)
			{
				yield return null;
			}

			m_animator.SetBool("show", false);
			yield return new WaitForSeconds(0.3f);
			_onFinish.Invoke();




		}







	}
}




