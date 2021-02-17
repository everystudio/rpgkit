using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class FieldUnitMover : MonoBehaviour
    {
        private void Start()
        {
            m_fieldUnitProperty = GetComponent<FieldUnitCore>().m_fieldUnitProperty;
        }
        private FieldUnitProperty m_fieldUnitProperty;

        public Animator m_animator;
        public float m_fSpeed = 1.0f;
        void Update()
        {
            float fInputHorizontal = Input.GetAxis("Horizontal");
            float fInputVertical = Input.GetAxis("Vertical");
            transform.Translate(
                fInputHorizontal * m_fSpeed,
                fInputVertical * m_fSpeed,
                0.0f
                );

            if(fInputHorizontal != 0.0f || fInputVertical != 0.0f)
            {
                m_animator.SetFloat("dirX", fInputHorizontal);
                m_animator.SetFloat("dirY", fInputVertical);
                m_fieldUnitProperty.direction = new Vector2(fInputHorizontal, fInputVertical).normalized;
            }
        }
    }
}