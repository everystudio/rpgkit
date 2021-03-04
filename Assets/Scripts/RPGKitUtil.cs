using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
	public class RPGKitUtil : MonoBehaviour
	{
		public static int DeleteObjects<T>(GameObject _goRoot) where T : Component
		{
			int iRet = 0;
			T[] arr = _goRoot.GetComponentsInChildren<T>();
			iRet = arr.Length;
			if (0 < iRet)
			{
				foreach (T t in arr)
				{
					if (_goRoot != t.gameObject)
					{
						Destroy(t.gameObject);
					}
				}
			}
			return iRet;
		}


	}
}
