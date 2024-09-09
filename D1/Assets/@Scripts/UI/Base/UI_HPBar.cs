using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    private Creature _owner;
    private Slider _slider;
    private float offsetY = 0.15f;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _slider = GetComponent<Slider>();
        return true;
    }

    public void SetInfo(Creature owner, float MaxHp)
    {
        _owner = owner;
        _slider.maxValue = MaxHp;
        _slider.value = MaxHp;
    }

    public void UpdatePosition()
    {
        if (_owner != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(_owner.transform.position + new Vector3(0, offsetY)); // 위치 조정
            transform.position = screenPosition;
        }
    }

    public void Refresh(float hp)
    {
        gameObject.SetActive(true);
        _slider.value = hp;
    }


}
