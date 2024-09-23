using System;
using System.Collections;
using UnityEngine;

public abstract class ProjectileMotionBase : BaseObject
{
    protected float _speed;
    protected bool IsRotation = true;

    protected Vector3 _endPos;
    protected Vector3 _startPos;
    protected Creature _target;
    protected Action EndCallback;

    protected abstract IEnumerator LaunchProjectile();

    public virtual void SetInfo(Vector2 startPos, Vector3 endPos, Creature target, Data.ProjectileData 
    projData, Action endCallback = null)
    {
        _startPos = startPos;
        _endPos = endPos;
        _target = target;
        _speed = projData.ProjSpeed;
        
        EndCallback = endCallback;
        StartCoroutine(LaunchProjectile());
    }

    protected Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90);
    }
}
