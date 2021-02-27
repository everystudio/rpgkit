using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class ChestManager : Singleton<ChestManager>
    {
        public List<int> m_strOpenedChestId = new List<int>();

        private ChestBase[] m_currentSceneChestArr;

        // 変なことしなくてもよくなったかも
        public void OnChangeScene(string _strSceneName)
        {
            //Debug.Log(_strSceneName);
            m_currentSceneChestArr = FindObjectsOfType<ChestBase>();
            /*
            foreach( ChestBase chest in m_currentSceneChestArr)
            {
                if (m_strOpenedChestId.Contains(chest.GetInstanceID()))
                {
                    chest.Open();
                }
                else
                {
                    chest.Close();
                }
            }
            */
        }

        public void OpenChest(ChestBase _chest)
        {
            int chest_instance_id = _chest.GetInstanceID();
            if(!m_strOpenedChestId.Contains(chest_instance_id))
            {
                m_strOpenedChestId.Add(chest_instance_id);
                _chest.Open();
            }
        }

    }
}



