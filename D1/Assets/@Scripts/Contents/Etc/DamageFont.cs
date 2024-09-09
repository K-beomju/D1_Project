using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class DamageFont : MonoBehaviour
{
	private TextMeshPro _damageText;

	public void SetInfo(Vector2 pos, float damage = 0, Transform parent = null, bool isCritical = false)
	{
		_damageText = GetComponent<TextMeshPro>();
		_damageText.sortingOrder = SortingLayers.PROJECTILE;

		transform.position = pos;

		if (damage < 0)
		{
			_damageText.color = Util.HexToColor("4EEE6F");
		}
		else if (isCritical)
		{
			_damageText.color = Util.HexToColor("EFAD00");
		}
		else
		{
			_damageText.color = Color.white;
		}

		_damageText.text = $"{Mathf.Abs(damage)}";
		_damageText.alpha = 1;

		if (parent != null)
			GetComponent<MeshRenderer>().sortingOrder = SortingLayers.DAMAGE_FONT;

        StartCoroutine(DoAnimation());
	}

	
    private IEnumerator DoAnimation()
    {
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, 0.5f, 0); // 위로 2 단위만큼 이동
        float duration = 1.0f; // 애니메이션 총 시간
        float currentTime = 0f;

        // 텍스트를 위로 이동시키면서 서서히 사라지게 만들기
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            // 텍스트를 위로 이동
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // 텍스트 알파값을 점점 줄여서 서서히 사라지게 함
            _damageText.alpha = Mathf.Lerp(1, 0, t);

            yield return null;
        }

        Managers.Pool.Push(gameObject);

    }
}
