using System;
using System.Collections;
using UnityEngine;
using Data;


public class Projectile : BaseObject
{
    protected Hero Owner;
    protected ProjectileData ProjectileInfoData;

    protected Vector3 endPos;
    protected Vector3 startPos;
    
    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = Define.EObjectType.Projectile;
        return true;
    }

    public void SetInfo(int dataTemplateID, Hero owner)
    {
        int projDataId = dataTemplateID;
        ProjectileInfoData = Managers.Data.ProjectileDic[projDataId];

        Owner = owner;

        ProjectileMotionBase motion;
        switch (ProjectileInfoData.ProjectileMotion)
        {
            case Define.EProjetionMotion.Straight:
                motion = gameObject.GetOrAddComponent<StraightMotion>();
                break;
            case Define.EProjetionMotion.Parabola:
                motion = gameObject.GetOrAddComponent<ParabolaMotion>();
                break;
            default:
                motion = gameObject.GetOrAddComponent<StraightMotion>();
                break;
        }
        
        if (motion != null)
        {
            startPos = transform.position;
            endPos = owner.Target.CenterPosition;
            motion.SetInfo(startPos, endPos, Owner.Target , ProjectileInfoData, DestroyProjectile);
        }

        if (this.IsValid())
            StartCoroutine(CoReserveDestroy());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Creature target = collision.GetComponent<Creature>();
        if (!target.IsValid())
        { 
            return;
        }
        target.OnDamaged(Owner);
        DestroyProjectile();
    }

    private IEnumerator CoReserveDestroy()
    {
        yield return new WaitForSeconds(5f);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Managers.Object.Despawn(this);
    }

}
