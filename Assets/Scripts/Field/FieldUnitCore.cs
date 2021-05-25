using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace rpgkit
{
    public class FieldUnitCore : StateMachineBase<FieldUnitCore>
    {
        public FieldUnitProperty m_fieldUnitProperty = new FieldUnitProperty();
        public GameObject m_goGameCanvas;

        [SerializeField] private InputAction m_inputMover;
        public Vector2 m_movementValue;
        public float m_fMoveSpeed = 1.0f;

        private void Awake()
        {
            SetState(new Idle(this));
        }
        protected override void OnUpdatePrev()
        {
            base.OnUpdatePrev();
            m_movementValue = m_inputMover.ReadValue<Vector2>();
            //Debug.Log(m_movementValue);
        }
        private void OnEnable()
        {
            m_inputMover.Enable();
        }
        private void OnDisable()
        {
            m_inputMover.Disable();
        }

        public class Idle : StateBase<FieldUnitCore>
        {
            public Idle(FieldUnitCore _machine) : base(_machine)
            {
            }
            public override void OnUpdateState()
            {
                base.OnUpdateState();
                if (0 < machine.m_movementValue.sqrMagnitude)
                {
                    machine.SetState(new Walk(machine));
                }
            }

            private class Walk : StateBase<FieldUnitCore>
            {
                public Walk(FieldUnitCore _machine) : base(_machine)
                {
                }
                public override void OnUpdateState()
                {
                    base.OnUpdateState();
                    if( 0 < machine.m_movementValue.sqrMagnitude)
                    {
                        float fInputHorizontal = machine.m_movementValue.x;
                        float fInputVertical = machine.m_movementValue.y;

                        machine.transform.Translate(
                            fInputHorizontal * machine.m_fMoveSpeed,
                            fInputVertical * machine.m_fMoveSpeed,
                            0.0f
                            );
                        machine.GetComponent<Animator>().SetFloat("dirX", fInputHorizontal);
                        machine.GetComponent<Animator>().SetFloat("dirY", fInputVertical);
                        machine.m_fieldUnitProperty.direction = new Vector2(fInputHorizontal, fInputVertical).normalized;
                    }
                    else
                    {
                        machine.SetState(new Idle(machine));
                    }
                }
            }
        }

        public class Freeze : StateBase<FieldUnitCore>
        {
            public Freeze(FieldUnitCore _machine) : base(_machine)
            {
            }
        }

    }




}

