using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    [CreateAssetMenu(fileName ="SOUnit" ,menuName = "ScriptableObject/Unit")]
    public class SOUnit : ScriptableObject
    {
        public int unit_id;
        public Texture texture;
        public Sprite face_icon;
    }
}



