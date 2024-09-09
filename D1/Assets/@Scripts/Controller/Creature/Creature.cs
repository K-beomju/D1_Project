using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    public Animator Anim { get; protected set; }

    protected ECreatureState _creatureState = ECreatureState.None;
    public virtual ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                _creatureState = value;
                UpdateAnimation();
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Anim = GetComponent<Animator>();
        Sprite.sortingOrder = SortingLayers.CREATURE;
        return true;
    }

    public override void SetInfo(int dataTemplateID)
    {
        base.SetInfo(dataTemplateID);

        // InfoSetting
        SetCreatureInfo(dataTemplateID);

        // State
        CreatureState = ECreatureState.Idle;

        #region AI
        StartCoroutine(CoUpdateAI());
        #endregion
    }



    protected virtual void SetCreatureInfo(int dataTemplateID)
    {
    }

    #region Anim
    protected virtual void UpdateAnimation()
    {
    }

    public void PlayAnimation(string AnimName, EAnimType AnimType, bool loop = false)
    {
        if (Anim == null)
        {
            return;
        }

        switch (AnimType)
        {
            case EAnimType.Bool:
                Anim.SetBool(AnimName, loop);
                break;
            case EAnimType.SetTrigger:
                Anim.SetTrigger(AnimName);
                break;
        }
    }
    #endregion


    #region AI
    protected virtual IEnumerator CoUpdateAI()
    {
        while (true)
        {
            UpdateCreatureState();
            yield return null;
        }
    }

    private void UpdateCreatureState()
    {
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                UpdateIdle();
                break;
            case ECreatureState.Move:
                UpdateMove();
                break;
            case ECreatureState.Attack:
                UpdateAttack();
                break;
            case ECreatureState.Skill:
                UpdateSkill();
                break;
            case ECreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle() {}
    protected virtual void UpdateMove() {}
    protected virtual void UpdateAttack() {}
    protected virtual void UpdateSkill() {}
    protected virtual void UpdateDead() {}

    #endregion

    #region Wait

    protected Coroutine _coWait;

    protected void StartWait(float seconds)
    {
        CancelWait();
        _coWait = StartCoroutine(CoWait(seconds));
    }

    IEnumerator CoWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _coWait = null;
    }

    protected void CancelWait()
    {
        if (_coWait != null)
            StopCoroutine(_coWait);
        _coWait = null;
    }

    #endregion



}
