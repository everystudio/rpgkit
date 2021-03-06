using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    [CreateAssetMenu(fileName = "SOItem", menuName = "ScriptableObject/SOItem")]
    public class SOItem : ScriptableObject
    {
        public int item_id;
        public Sprite icon;
    }
}


