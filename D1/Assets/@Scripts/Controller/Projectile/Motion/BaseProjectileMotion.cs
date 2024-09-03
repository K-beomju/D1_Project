using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectileMotion : InitBase
{
    //public ProjectileInfoData ProjectileInfoData { get; private set; }
    public Vector3 StartPosition { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public Creature Target { get; private set; }
    protected Action EndCallback { get; private set; }

    private Coroutine _coLaunchProjectile;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }


    public void OnDisable()
    {
        StopCoroutine(_coLaunchProjectile);
        _coLaunchProjectile = null;
    }

    protected void SetInfo(int dataTemplateID, Vector3 startPosition, Vector3 targetPosition, Creature target = null, Action endCallback = null)
    {
        //ProjectileInfoData = Managers.Data.ProjectileInfoDataDic[dataTemplateID];
        StartPosition = startPosition;
        TargetPosition = targetPosition;
        Target = target;
        EndCallback = endCallback;

        if (_coLaunchProjectile != null)
        {
            StopCoroutine(_coLaunchProjectile);
        }

        _coLaunchProjectile = StartCoroutine(CoLaunchProjectile());
    }

    protected void LookAt2D(Vector2 forward)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    protected abstract IEnumerator CoLaunchProjectile();
}
