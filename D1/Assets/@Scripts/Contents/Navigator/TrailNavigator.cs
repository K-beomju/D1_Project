using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailNavigator : MonoBehaviour
{
    private float _heightArc = 1f;
    float heightAdjustmentFactor = 1.5f; // 원하는 높이 조정 비율 (0.5f는 높이를 절반으로 줄임)

    public void SetInfo(Vector2 startPos, Vector2 endPos, float _speed, Action EndCallback)
    {
        StartCoroutine(LaunchTrail(startPos, endPos, _speed, EndCallback));
    }

    public IEnumerator LaunchTrail(Vector2 startPos, Vector2 endPos, float _speed, Action EndCallback)
    {
        float startTime = Time.time;
        float journeyLength = Vector2.Distance(startPos, endPos);
        float totalTime = journeyLength / _speed;

        while (Time.time - startTime < totalTime)
        {
            float normalizedTime = (Time.time - startTime) / totalTime;

            // 포물선 모양으로 이동
            float x = Mathf.Lerp(startPos.x, endPos.x, normalizedTime);
            float baseY = Mathf.Lerp(startPos.y, endPos.y, normalizedTime);
            float arc = _heightArc * heightAdjustmentFactor * Mathf.Sin(normalizedTime * Mathf.PI);

            float y = baseY - arc;

            var nextPos = new Vector3(x, y);
            transform.position = nextPos;

            yield return null;
        }

        EndCallback?.Invoke();
    }
}
