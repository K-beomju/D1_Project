using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcMotion : BaseProjectileMotion
{
    public float HeightArc { get; protected set; } = 2.0f;

    public new void SetInfo(int dataTemplateID, Vector3 startPosition, Vector3 targetPosition, Creature target, Action endCallback)
    {
        base.SetInfo(dataTemplateID, startPosition, targetPosition, target, endCallback);
        //HeightArc = ProjectileInfoData.ArcHeight;
        HeightArc = 2;
    }

    protected override IEnumerator CoLaunchProjectile()
    {
        float journeyLength = Vector2.Distance(StartPosition, TargetPosition);
        float totalTime = journeyLength / 3; //ProjectileInfoData.MoveSpeed;
        float elapsedTime = 0;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / totalTime;

            float x = Mathf.Lerp(StartPosition.x, TargetPosition.x, normalizedTime);
            float baseY = Mathf.Lerp(StartPosition.y, TargetPosition.y, normalizedTime);
            float arc = HeightArc * Mathf.Sin(normalizedTime * Mathf.PI);

            float y = baseY + arc;

            var nextPos = new Vector3(x, y);
            transform.position = nextPos;

            yield return null;
        }

        EndCallback?.Invoke();
        yield return null;
    }
}
