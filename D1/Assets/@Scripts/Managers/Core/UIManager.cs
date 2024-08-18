using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// UI의 전반적인 관리를 담당하는 클래스입니다. UI를 생성하고, 팝업을 관리하고, UI 계층을 설정하는 등의 작업을 수행합니다.
public class UIManager
{
    private int _order = 10; // UI 정렬 순서를 관리하기 위한 변수입니다.

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();// 팝업 UI들을 관리하기 위한 스택입니다.

    private UI_Scene _sceneUI = null;
    public UI_Scene SceneUI
    {
        get { return _sceneUI; }
        set { _sceneUI = value; }
    }

    // UI의 최상위 루트를 반환합니다. 없으면 새로 생성합니다.
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    // Canvas를 설정하고, 필요한 경우 정렬 순서를 설정합니다.
    public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        if (canvas == null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = sort;
        }

        CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
        if (cs != null)
        {
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1080, 1920);
        }

        go.GetOrAddComponent<GraphicRaycaster>();

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = sortOrder;
        }
    }

    // 현재 씬에 연결된 UI를 특정 타입으로 반환합니다.
    public T GetSceneUI<T>() where T : UI_Base
    {
        return _sceneUI as T;
    }

    // 부모가 될 Transform과 이름을 받아 서브 아이템 UI를 생성하고 반환합니다.
    public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name, parent, pooling);
        go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    // UI를 기본적으로 생성하고, 루트에 추가하여 반환합니다.
    public T ShowBaseUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);
        T baseUI = Util.GetOrAddComponent<T>(go);

        go.transform.SetParent(Root.transform);

        return baseUI;
    }

    // 씬 UI를 생성하고, 루트에 추가하여 반환합니다.
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    // 팝업 UI를 생성하고, 스택에 추가하여 반환합니다.
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(name);
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    // 스택에서 팝업 UI를 제거하고 닫습니다.
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    // 가장 최근에 열린 팝업 UI를 닫습니다.
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        _order--;
    }

    // 스택에 있는 모든 팝업 UI를 닫습니다.
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    // 현재 열린 팝업 UI의 수를 반환합니다.
    public int GetPopupCount()
    {
        return _popupStack.Count;
    }

    // 팝업 UI를 전부 닫고, 씬 UI를 초기화합니다.
    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }

}
