using System.Collections;
using UnityEngine;

public class ParabolaMotion : ProjectileMotionBase
{
    private float _heightArc = 1;
    float heightAdjustmentFactor = 0.15f; // 원하는 높이 조정 비율

    protected override IEnumerator LaunchProjectile()
    {
        float startTime = Time.time;

        while (true)
        {
            // 목표가 계속 움직이므로, 목표의 현재 위치를 매번 업데이트
            Vector3 currentEndPos = _target != null ? _target.transform.position : _endPos;

            float journeyLength = Vector2.Distance(_startPos, currentEndPos);
            float totalTime = journeyLength / _speed;

            float normalizedTime = (Time.time - startTime) / totalTime;
            Debug.Log(normalizedTime);
            // 선형 보간 
            float x = Mathf.Lerp(_startPos.x, currentEndPos.x, normalizedTime);
            float baseY = Mathf.Lerp(_startPos.y, currentEndPos.y, normalizedTime);

            // 포물선 모양으로 이동 (x, y 좌표 계산)
            float arc = _heightArc * heightAdjustmentFactor * Mathf.Sin(normalizedTime * Mathf.PI);
            float y = baseY + arc;

            Vector3 nextPos = new Vector3(x, y);

            // 목표 방향을 바라보도록 회전
            if (IsRotation)
                transform.rotation = LookAt2D(nextPos - (Vector3)transform.position);

            transform.position = nextPos;

            // 목표에 도달하면 루프 종료
            if (normalizedTime >= 1f)
                break;

            yield return null;
        }

        EndCallback?.Invoke();
    }
}
