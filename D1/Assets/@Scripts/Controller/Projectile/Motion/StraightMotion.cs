using System.Collections;
using UnityEngine;

public class StraightMotion : ProjectileMotionBase
{
    protected override IEnumerator LaunchProjectile()
    {
        while (true)
        {     
            Vector3 direction = (_endPos -  (Vector3)CenterPosition).normalized;
            transform.rotation = LookAt2D(direction);
            transform.position += direction *(_speed * Time.deltaTime);

            if (Vector2.Distance(CenterPosition, _endPos) < 0.1f)
            {
                EndCallback?.Invoke();
                yield break; 
            }
            yield return null; 
        }
    }
}