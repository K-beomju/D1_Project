using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Projectile : BaseObject
{
    public Hero Owner { get; private set; }
    public BaseProjectileMotion ProjectileMotion { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Projectile;

        return true;
    }

    public override void SetInfo(int dataTemplateID)
    {
        base.SetInfo(dataTemplateID);

        //Managers.Resource.Instantiate(ProjectileInfoData.PrefabKey, gameObject.transform);
    }

     public void SetSpawnInfo(Hero owner, LayerMask layer)
    {
        Owner = owner;

        if (Owner.Target.IsValid() == false)
        {
            Managers.Object.Despawn(this);
            return;
        }

        Collider.excludeLayers = layer;
        ProjectileMotion = gameObject.AddComponent<ArcMotion>();
        ArcMotion arcMotion = ProjectileMotion as ArcMotion;
        arcMotion.SetInfo(0, owner.CenterPosition, Util.GetRandomPositionWithinRadius(owner.Target.CenterPosition, owner.Target.Collider.radius), owner.Target, () => { Managers.Object.Despawn(this); });


        // switch (default)
        // {
        //     case EProjectileMotionType.Horizontal:
        //         ProjectileMotion = gameObject.AddComponent<HorizontalMotion>();
        //         HorizontalMotion horizontalMotion = ProjectileMotion as HorizontalMotion;
        //         horizontalMotion.SetInfo(ProjectileInfoData.DataID, owner.CenterPosition, Util.GetRandomPositionWithinRadius(owner.Target.CenterPosition, owner.Target.Collider.radius), owner.Target, () => { Managers.Object.Despawn(this); });
        //         break;
        //     case EProjectileMotionType.Straight:
        //         ProjectileMotion = gameObject.AddComponent<StraightMotion>();
        //         StraightMotion straightMotion = ProjectileMotion as StraightMotion;
        //         straightMotion.SetInfo(ProjectileInfoData.DataID,  owner.CenterPosition, Util.GetRandomPositionWithinRadius(owner.Target.CenterPosition, owner.Target.Collider.radius), owner.Target, () => { Managers.Object.Despawn(this); });
        //         break;
        //     case EProjectileMotionType.Guided:
        //         ProjectileMotion = gameObject.AddComponent<GuidedMotion>();
        //         GuidedMotion guidedMotion = ProjectileMotion as GuidedMotion;
        //         guidedMotion.SetInfo(ProjectileInfoData.DataID,  owner.CenterPosition, Util.GetRandomPositionWithinRadius(owner.Target.CenterPosition, owner.Target.Collider.radius), owner.Target, () => { Managers.Object.Despawn(this); });
        //         break;
        //     case EProjectileMotionType.Arc:
        //         ProjectileMotion = gameObject.AddComponent<ArcMotion>();
        //         ArcMotion arcMotion = ProjectileMotion as ArcMotion;
        //         arcMotion.SetInfo(ProjectileInfoData.DataID,  owner.CenterPosition, Util.GetRandomPositionWithinRadius(owner.Target.CenterPosition, owner.Target.Collider.radius), owner.Target, () => { Managers.Object.Despawn(this); });
        //         break;
        //     case EProjectileMotionType.ArcRotation:
        //         ProjectileMotion = gameObject.AddComponent<ArcRotationMotion>();
        //         ArcRotationMotion arcRotationMotion = ProjectileMotion as ArcRotationMotion;
        //         arcRotationMotion.SetInfo(ProjectileInfoData.DataID, owner.CenterPosition, Util.GetRandomPositionWithinRadius(owner.Target.CenterPosition, owner.Target.Collider.radius), owner.Target, () => { Managers.Object.Despawn(this); });
        //         break;
        // }

        StartCoroutine(CoReserveDestroy(3));
    }

    private IEnumerator CoReserveDestroy(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Managers.Object.Despawn(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Creature target = collision.GetComponent<Creature>();
        if (!target.IsValid())
        { 
            return;
        }

        // if (ProjectileInfoData.ProjectileTargetingType == EProjectileTargetingType.Single && target != Owner.Target)
        // {
        //     return;
        // } 

        // target.OnDamaged(Owner, OwnerSkill);
        // HandleProjectileRemoval();
        // Managers.Object.Spawn<Effect>(transform.position, ProjectileInfoData.HitEffectDataID);
    }

}
