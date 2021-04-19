using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace rpgkit
{
    public class FadeScreen : Singleton<FadeScreen>
    {
        [SerializeField]
        private Image m_imgScreen;

        public void Fadeout(float _fDuration, Action _onFinished)
        {
            m_imgScreen.DOFade(1.0f, _fDuration)
                .OnComplete(() => {
                    Debug.Log("aaa");
                    _onFinished.Invoke();
                });
        }
        public void Fadein(float _fDuration, Action _onFinished)
        {
            m_imgScreen.DOFade(0.0f, _fDuration)
                .OnComplete(() => { _onFinished.Invoke(); });
        }


    }
}
