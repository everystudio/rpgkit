using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class UIAssistant : MonoBehaviour {
    public static UIAssistant Instance;

    public List<Transform> UImodules = new List<Transform>();

    public delegate void Action();
    public static Action onScreenResize = delegate {};
    public static Action<string> onShowPage = delegate {};
    Vector2 screenSize;

    public List<PanelPage> panels = new List<PanelPage>(); // Dictionary panels. It is formed automatically from the child objects
    public List<Page> pages = new List<Page>(); // Dictionary pages. It is based on an array of "pages"

    private string currentPage; // Current page name
    private string previousPage; // Previous page name

    void Start() {
        ArraysConvertation(); // filling dictionaries
        Page defaultPage = GetDefaultPage();
        if (defaultPage != null)
            ShowPage(defaultPage, true); // Showing of starting page
    }

    void Awake() {
        if (Instance == null)
        {
            System.Type type = typeof(UIAssistant);
            Instance = GameObject.FindObjectOfType(type) as UIAssistant;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

        screenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update() {
        if (screenSize != new Vector2(Screen.width, Screen.height)) {
            screenSize = new Vector2(Screen.width, Screen.height);
            onScreenResize.Invoke();
        }
    }

    // filling dictionaries
    public void ArraysConvertation() {
        // filling panels dictionary of the child objects of type "PanelPage"
        panels = new List<PanelPage>();
        panels.AddRange(GetComponentsInChildren<PanelPage>(true));
        foreach (Transform module in UImodules)
        {
            panels.AddRange(module.GetComponentsInChildren<PanelPage>(true));
        }
        if (Application.isEditor)
        {
            panels.Sort((PanelPage a, PanelPage b) =>
            {
                return string.Compare(a.name, b.name);
            });
        }
    }

    public void ShowPage(Page page, bool immediate = false) {
        if (PanelPage.uiAnimation > 0)
        {
            return;
        }

        if (currentPage == page.name)
        {
            return;
        }

        if (pages == null)
        {
            return;
        }
        previousPage = currentPage;
        currentPage = page.name;


        foreach (PanelPage panel in panels) {
            if (page.panels.Contains(panel))
            {
                panel.SetVisible(true, immediate);
            }
            else
            {
                if (!page.ignoring_panels.Contains(panel) && !panel.m_bFreez)
                {
                    panel.SetVisible(false, immediate);
                }
            }
        }
        
        onShowPage.Invoke(page.name);

        if (page.soundtrack != "-") {
        }

        if (page.setTimeScale)
        {
            Time.timeScale = page.timeScale;
        }
    }

    public void ShowPage(string page_name) {
        ShowPage(page_name, false);
    }

    public void ShowPage(string page_name, bool immediate) {
        Page page = pages.Find(x => x.name == page_name);
        if (page != null)
        {
            ShowPage(page, immediate);
        }
    }

    public void FreezPanel(string panel_name, bool value = true) {
        PanelPage panel = panels.Find(x => x.name == panel_name);
        if (panel != null)
            panel.m_bFreez = value;
    }

    public void SetPanelVisible(string panel_name, bool visible, bool immediate = false) {
        PanelPage panel = panels.Find(x => x.name == panel_name);
        if (panel) {
            if (immediate)
                panel.SetVisible(visible, true);
            else
                panel.SetVisible(visible);
        }
    }

    // hide all panels
    public void HideAll() {
        foreach (PanelPage panel in panels)
            panel.SetVisible(false);
    }

    // show previous page
    public void ShowPreviousPage() {
        ShowPage(previousPage);
    }
    public void ShowParentPage()
    {
        Page page = pages.Find(x => x.name == currentPage);
        if( page != null && page.parent_page != "")
        {
            ShowPage(page.parent_page);
        }

    }

    public string GetCurrentPage() {
        return currentPage;
    }

    public Page GetDefaultPage() {
        return pages.Find(x => x.default_page);
    }

    // Class information about the page
    [System.Serializable]
    public class Page {
        public string name; // page name
        public List<PanelPage> panels = new List<PanelPage>(); // a list of names of panels in this page
        public List<PanelPage> ignoring_panels = new List<PanelPage>(); // a list of names of panels in this page
        public string soundtrack = "-";
        public bool default_page = false;
        public bool setTimeScale = true;
        public float timeScale = 1;
        public string parent_page = "";
    }
}