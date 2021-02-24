using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSetActive : MonoBehaviour
{
	public GameObject m_goTarget;

	public void OnToggle()
	{
		m_goTarget.SetActive(!m_goTarget.activeSelf);

		RectTransform rt = GetComponent<RectTransform>();
		rt.sizeDelta = rt.sizeDelta * 0.000001f;
	}
}
