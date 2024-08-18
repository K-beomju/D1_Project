using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;


// UI 요소를 다루기 위한 기본 클래스입니다. Unity의 다양한 UI 컴포넌트(Button, Text 등)를 쉽게 바인딩하고 가져올 수 있도록 도와줍니다.
public class UI_Base : InitBase
{
    // UI 요소들을 저장할 딕셔너리입니다. 키는 타입, 값은 해당 타입의 오브젝트 배열입니다.
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // 초기화 시에 호출되는 메서드입니다.
    private void Awake()
    {
        Init();
    }
    
    #region Bind
    // UI 요소들을 바인딩하는 메서드입니다. 예를 들어, 버튼이나 텍스트를 타입에 따라 찾아서 딕셔너리에 저장합니다.

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); // enum 타입의 이름들을 가져옵니다.
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects); // 타입을 키로 하여 딕셔너리에 저장합니다.

        for (int i = 0; i < names.Length; i++)
        { 
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({name[i]})");
        }
    }

    // 오브젝트, 이미지, 텍스트, 버튼 등을 쉽게 바인딩하기 위한 헬퍼 메서드들입니다.
    protected void BindObjects(Type type) { Bind<GameObject>(type); }
    protected void BindImages(Type type) { Bind<Image>(type); }
    protected void BindTexts(Type type) { Bind<TMP_Text>(type); }
    protected void BindButtons(Type type) { Bind<Button>(type); }
    #endregion

    #region Get
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    // GameObject, Image, Text, Button 등을 쉽게 가져오기 위한 헬퍼 메서드들입니다.
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    #endregion

    // UI 이벤트를 바인딩하기 위한 메서드입니다. 이벤트 타입에 따라 적절한 핸들러에 Action을 등록합니다.
    public static void BindEvent(GameObject go, Action<PointerEventData> action = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.EUIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.EUIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.EUIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case Define.EUIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }

}
