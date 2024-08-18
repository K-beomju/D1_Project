using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UnityEngine의 이벤트 시스템을 사용하여 UI 요소에서 발생하는 클릭, 드래그 등 다양한 입력 이벤트를 처리하기 위한 클래스입니다.
// IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler 인터페이스를 구현하여 해당 이벤트를 감지하고 처리합니다.
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // 각 이벤트에 대해 Action 대리자를 사용하여 외부에서 이벤트 핸들러를 등록할 수 있도록 합니다.
    public event Action<PointerEventData> OnClickHandler = null;
    public event Action<PointerEventData> OnPointerDownHandler = null;
    public event Action<PointerEventData> OnPointerUpHandler = null;
    public event Action<PointerEventData> OnDragHandler = null;


    // 클릭 이벤트가 발생했을 때 등록된 핸들러를 호출합니다.
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke(eventData);
    }

    // 포인터가 눌렸을 때 등록된 핸들러를 호출합니다.
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke(eventData);
    }

    // 포인터에서 손을 뗐을 때 등록된 핸들러를 호출합니다.
    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke(eventData);
    }

    // 드래그가 발생했을 때 등록된 핸들러를 호출합니다.
    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(eventData);
    }

}
