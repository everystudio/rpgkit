using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPage : MonoBehaviour
{
    public static int uiAnimation = 0;
    public bool m_bFreez = false;
    bool m_isPlaying = false;
    public bool IsPlaying
    {
        get
        {
            return m_isPlaying;
        }
        set
        {
            if (m_isPlaying != value)
            {
                m_isPlaying = value;
                uiAnimation += m_isPlaying ? 1 : -1;
            }
        }
    }

    public string hide; // Name of showing m_animationation
    public string show; // Name of hiding m_animationation

    private string m_currentClip = "";

    Animation m_animation;
    void Awake()
    {
        m_animation = GetComponent<Animation>();
    }

    void OnEnable()
    {
        m_bFreez = false;
    }

    public void SetVisible(bool visible, bool immediate = false)
    {
        if (gameObject.activeSelf == visible)
            return;
        m_currentClip = "";
        if (!visible)
        {
            if (hide != "")
                m_currentClip = hide;
            else
            {
                gameObject.SetActive(false);
                return;
            }
        }
        if (visible)
        {
            gameObject.SetActive(true);
            if (show != "")
            {
                m_currentClip = show;
            }
            else
            {
                return;
            }
        }
        if (m_currentClip == "")
            return;
        if (immediate)
            m_animation[m_currentClip].time = m_animation[m_currentClip].length;
        else
            Play(m_currentClip);
    }

    void Play(string clip)
    {
        StartCoroutine(PlayClipRoutine(clip));
    }

    public void PlayClip(string clip)
    {
        if (!IsPlaying)
        {
            Play(clip);
        }
    }

    IEnumerator PlayClipRoutine(string clip)
    {
        IsPlaying = true;

        m_animation.Play(clip);
        m_animation[clip].time = 0;

        while (m_animation[clip].time < m_animation[clip].length)
        {

            m_animation[clip].enabled = true;
            m_animation[clip].time += Mathf.Min(Time.unscaledDeltaTime, Time.maximumDeltaTime);
            m_animation.Sample();
            m_animation[clip].enabled = false;

            yield return 0;
        }

        IsPlaying = false;
        if (clip == hide)
        {
            gameObject.SetActive(false);
        }
    }

}
