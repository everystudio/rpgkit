using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	public class Portal : MonoBehaviour
	{
		public string exitscene;
		public string target_position_name;

		public Portal targetPortal;

		public Room setroom;
		public GameObject exitpoint;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			PlayerPrefs.SetString("target_position_name", target_position_name);
			string result = PlayerPrefs.GetString("target_position_name");

			if (collision.tag == "Player")
			{
				collision.gameObject.transform.position =
					targetPortal.exitpoint.transform.position;

				setroom.Exit();
				targetPortal.setroom.Enter();
				targetPortal.setroom.m_vcam.LookAt = collision.gameObject.transform;
			}
		}
	}

}


