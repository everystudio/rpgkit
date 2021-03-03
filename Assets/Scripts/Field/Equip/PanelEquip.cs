using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class PanelEquip : MonoBehaviour
    {
        public EquipUnitView m_equipUnitView;
        public EquipStatusView m_equipStatusView;

        public EquipInfo m_equipInfoCurrent;
        public EquipInfo m_equipInfoChange;

        private DataUnitParam m_dummyUnit;

        private void OnEnable()
        {
            m_dummyUnit = new DataUnitParam();

            DataManager.Instance.m_dataUnit.list[0].RefreshAssist(DataManager.Instance.m_masterEquip.list);
            m_dummyUnit.AllCopy(DataManager.Instance.m_dataUnit.list[0]);

            //　無理やり装備を外す
            m_dummyUnit.equip1 = 0;

            m_equipStatusView.Initialize(DataManager.Instance.m_dataUnit.list[0] , m_dummyUnit);
            m_equipUnitView.Initialize(
                DataManager.Instance.m_dataUnit.list[0],
                DataManager.Instance.m_masterEquip.list
                );
        }

    }
}



