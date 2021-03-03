using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit
{
    public class EquipUnitView : MonoBehaviour
    {
        public EquipBanner equip1;
        public EquipBanner equip2;
        public EquipBanner equip3;
        public EquipBanner equip4;
        public EquipBanner equip5;
        public EquipBanner equip6;

        public void Awake()
        {
            equip1.GetComponent<Button>().onClick.AddListener(() =>
            {

            });

        }


        public void Initialize(DataUnitParam _dataUnit , List<MasterEquipParam> _masterList)
        {
            equip1.Initialize(_masterList.Find(p => p.equip_id == _dataUnit.equip1));
            equip2.Initialize(_masterList.Find(p => p.equip_id == _dataUnit.equip2));
            equip3.Initialize(_masterList.Find(p => p.equip_id == _dataUnit.equip3));
            equip4.Initialize(_masterList.Find(p => p.equip_id == _dataUnit.equip4));
            equip5.Initialize(_masterList.Find(p => p.equip_id == _dataUnit.equip5));
            equip6.Initialize(_masterList.Find(p => p.equip_id == _dataUnit.equip6));
        }


    }
}



