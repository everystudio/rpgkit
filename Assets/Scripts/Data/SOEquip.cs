using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    [CreateAssetMenu(fileName = "SOEquip", menuName = "ScriptableObject/SOEquip")]
    public class SOEquip : ScriptableObject
    {
        public int equip_id;
        public Sprite icon;
    }
}
