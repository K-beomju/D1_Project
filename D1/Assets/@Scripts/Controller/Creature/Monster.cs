using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public override bool Init() 
    {
        if(base.Init() == false)
        return false;

        CreatureType = Define.ECreatureType.Monster;
        return true;
    }

    protected override void SetCreatureInfo(int dataTemplateID)
    {
    }

    #region Anim
    protected override void UpdateAnimation()
    {
        switch (CreatureState)
        {               
            case ECreatureState.Move:
                PlayAnimation(AnimName.MOVE, EAnimType.Bool, true);
                break;
            case ECreatureState.Dead:
                PlayAnimation(AnimName.DEAD, EAnimType.SetTrigger, false);
                break;
        }
    }
    #endregion

    #region AI
    protected override void UpdateIdle()
    {
    }
    protected override void UpdateMove()
    {
    }
    protected override void UpdateDead()
    {
    }
    #endregion


}
