using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sequence;

namespace rpgkit
{
    [AddComponentMenu("")]
    [SequencePath("rpgkit/Select")]
    public class SQSelect : SequenceBase
    {
        [SerializeField]
        private IntVariable intValue;

        [SerializeField]
        private string Message;

        [SerializeField]
        private List<string> selectList;

        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1)
        {
            // リセット処理
            //Debug.Log(intValue.Value);
            intValue.Value = 0;

            bool bLock = true;
            ChoiceSelect.Instance.Request(Message, selectList, (value) =>
            {
                bLock = false;
                intValue.Value = value;
            });

            while (bLock)
            {
                yield return null;
            }

        }

    }
}







