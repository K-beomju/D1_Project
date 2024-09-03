using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Data;

public class Hero : Creature
{
    #region Stat
    public float Atk { get; private set; }
    public float AtkSpeed { get; private set; }
    public float AtkRange { get; private set; }
    public float AtkDelay { get; private set; }
    #endregion

    #region Config
    public HeroInfoData HeroInfo { get; private set; }
    public Creature Target { get; set; }
    public Sprite heroSprite;
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        CreatureType = ECreatureType.Hero;
        return true;
    }

    protected override void SetCreatureInfo(int dataTemplateID)
    {
        HeroInfo = Managers.Data.HeroInfoDic[dataTemplateID];

        #region Stat
        Atk = HeroInfo.Atk;
        AtkSpeed = HeroInfo.AtkSpeed;
        AtkRange = HeroInfo.AtkRange;
        AtkDelay = HeroInfo.AtkDelay;
        #endregion

        Anim.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>(HeroInfo.AnimName);
    }

    #region Anim
    protected override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                PlayAnimation(AnimName.IDLE, EAnimType.Bool, true);
                break;
            case ECreatureState.Move:
                PlayAnimation(AnimName.MOVE,EAnimType.Bool, true);
                break;
            case ECreatureState.Attack:
                PlayAnimation(AnimName.ATTACK, EAnimType.SetTrigger, true);
                break;
        }
    }
    #endregion

    #region AI

    protected override void UpdateIdle()
    {
        if (Target.IsValid())
        {
            if (Target.CreatureType == ECreatureType.Monster)
            {
                CreatureState = ECreatureState.Attack;
                return;
            }
        }
        else
        {
            Target = null;
        }
    }

    protected override void UpdateMove()
    {
    }

    protected override void UpdateAttack()
    {
        // 이동 중 일때 공격 x
        if (CreatureState == ECreatureState.Move)
            return;

        // 공격 쿨타임, 타겟 없을 때
        if (_coWait != null || Target.IsValid() == false)
        {
            // 기다릴 땐 IDLE 실행
            CreatureState = ECreatureState.Idle;
            return;
        }

        Vector3 dir = (Target.CenterPosition - CenterPosition);
        if (dir.sqrMagnitude > AtkRange * AtkRange)
        {
            CreatureState = ECreatureState.Idle;
            Target = null;
            return;
        }
        GenerateProjectile(transform.position);
        Debug.Log("제대로 되는지 확인");
        LookAtTarget(Target);
        StartWait(AtkDelay);
    }
    #endregion

    
    protected void GenerateProjectile(Vector3 spawnPos)
    {
        Projectile projectile = Managers.Object.Spawn<Projectile>(spawnPos, 0);

        LayerMask excludeMask = 0;
        // excludeMask.AddLayer(ELayer.Default);
        // excludeMask.AddLayer(ELayer.Projectile);

        // switch (CreatureType)
        // {
        //     case ECreatureType.Tank:
        //     case ECreatureType.Hero:
        //     case ECreatureType.TankAddOn:
        //     case ECreatureType.Summon:
        //         excludeMask.AddLayer(ELayer.Tank);
        //         excludeMask.AddLayer(ELayer.Hero);
        //         break;
        //     case ECreatureType.Monster:
        //     case ECreatureType.BossMonster:
        //         excludeMask.AddLayer(ELayer.Monster);
        //         break;
        // }

        projectile.SetSpawnInfo(this, excludeMask);
    }

    
	public override void OnAnimEventHandler()
	{
		// Skill
		if (Target.IsValid() == false)
			return;

		//Target.OnDamaged(this);
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CenterPosition, AtkRange);
    }



}
