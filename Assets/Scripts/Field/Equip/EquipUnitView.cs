using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rpgkit
{
    public class EquipUnitView : MonoBehaviour
    {

        public UnityEventInt OnClickEquipIndex = new UnityEventInt();

        public Image m_imgUnitFront;

        public EquipBanner equip1;
        public EquipBanner equip2;
        public EquipBanner equip3;
        public EquipBanner equip4;
        public EquipBanner equip5;
        public EquipBanner equip6;

        public void Awake()
        {
            EquipBanner[] banner_arr = new EquipBanner[]
            {
                equip1,
                equip2,
                equip3,
                equip4,
                equip5,
                equip6,
            };
            for( int i = 0; i < banner_arr.Length; i++)
            {
                banner_arr[i].OnClickIndex.AddListener((value) =>
                {
                    OnClickEquipIndex.Invoke(value);

                });
            }
            m_imgUnitFront.sprite = null;
            m_imgUnitFront.enabled = false;

        }
        public void Clear()
        {
            equip1.Initialize(null, null, 0);
            equip2.Initialize(null, null, 0);
            equip3.Initialize(null, null, 0);
            equip4.Initialize(null, null, 0);
            equip5.Initialize(null, null, 0);
            equip6.Initialize(null, null, 0);
        }

        public void Initialize(DataUnitParam _dataUnit , List<MasterEquipParam> _masterList , List<DataEquipParam> _dataList)
        {
            MasterUnitParam masterUnit = DataManager.Instance.m_masterUnit.list.Find(p => p.unit_id == _dataUnit.unit_id);
            m_imgUnitFront.enabled = true;
            m_imgUnitFront.sprite = masterUnit.so_unit_data.unit_front;

            DataEquipParam dataEquip1 = _dataList.Find(p => p.equip_serial == _dataUnit.equip1);
            DataEquipParam dataEquip2 = _dataList.Find(p => p.equip_serial == _dataUnit.equip2);
            DataEquipParam dataEquip3 = _dataList.Find(p => p.equip_serial == _dataUnit.equip3);
            DataEquipParam dataEquip4 = _dataList.Find(p => p.equip_serial == _dataUnit.equip4);
            DataEquipParam dataEquip5 = _dataList.Find(p => p.equip_serial == _dataUnit.equip5);
            DataEquipParam dataEquip6 = _dataList.Find(p => p.equip_serial == _dataUnit.equip6);

            MasterEquipParam masterEquip1 = dataEquip1 == null ? null : _masterList.Find(p => p.equip_id == dataEquip1.equip_id);
            MasterEquipParam masterEquip2 = dataEquip2 == null ? null : _masterList.Find(p => p.equip_id == dataEquip2.equip_id);
            MasterEquipParam masterEquip3 = dataEquip3 == null ? null : _masterList.Find(p => p.equip_id == dataEquip3.equip_id);
            MasterEquipParam masterEquip4 = dataEquip4 == null ? null : _masterList.Find(p => p.equip_id == dataEquip4.equip_id);
            MasterEquipParam masterEquip5 = dataEquip5 == null ? null : _masterList.Find(p => p.equip_id == dataEquip5.equip_id);
            MasterEquipParam masterEquip6 = dataEquip6 == null ? null : _masterList.Find(p => p.equip_id == dataEquip6.equip_id);

            equip1.Initialize(masterEquip1, dataEquip1,1);
            equip2.Initialize(masterEquip2, dataEquip2,2);
            equip3.Initialize(masterEquip3, dataEquip3,3);
            equip4.Initialize(masterEquip4, dataEquip4,4);
            equip5.Initialize(masterEquip5, dataEquip5,5);
            equip6.Initialize(masterEquip6, dataEquip6,6);
        }
        public void Select( int _iIndex)
        {
            EquipBanner[] bannerArr = new EquipBanner[]
            {
                equip1,
                equip2,
                equip3,
                equip4,
                equip5,
                equip6,
            };
            foreach( EquipBanner banner in bannerArr)
            {
                banner.Select(banner.Index == _iIndex);
            }
        }


    }
}



