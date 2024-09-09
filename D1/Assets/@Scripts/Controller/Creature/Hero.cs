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
    public SpriteRenderer Hand { get; set; }
    public SpriteRenderer Weapon { get; set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Hero;
        Hand = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Hand.sortingOrder = SortingLayers.HAND;
        Weapon = Hand.transform.GetChild(0).GetComponent<SpriteRenderer>();
        Weapon.sortingOrder = SortingLayers.WEAPON;
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
        Debug.Log(AtkRange);
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
                PlayAnimation(AnimName.MOVE, EAnimType.Bool, true);
                break;
        }
    }
    #endregion

    #region AI

    protected override void UpdateIdle()
    {
        if (Target.IsValid())
        {
            if (Target.ObjectType == EObjectType.Monster)
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
        if(_coWait != null)
         return;

        // 이동 중 일때 공격 x
        if (CreatureState == ECreatureState.Move)
            return;

        if (Target.IsValid() == false)
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


        PlayAnimation(AnimName.ATTACK, EAnimType.SetTrigger, true);
        LookAtTarget(Target);
        StartWait(AtkDelay);
    }
    #endregion


    public override void OnAnimEventHandler()
    {
        if (Target.IsValid() == false)
            return;

        Target.OnDamaged(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CenterPosition, AtkRange);
    }



}
