using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    #region Stat
    public float Hp { get; private set; }
    public float MaxHp { get; private set; }
    public float MoveSpeed { get; private set; }
    #endregion

    private Transform[] WayPoints;
    private Transform destination;
    private int wavepointIndex = 0;
    private bool isSpawn = true;

    protected UI_HPBar hpBar;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        return true;
    }

    protected override void SetCreatureInfo(int dataTemplateID)
    {
        MaxHp = 200;
        Hp = MaxHp;
        MoveSpeed = 1;

        WayPoints = Managers.Game.WayPoints;
        wavepointIndex = 0;
        destination = WayPoints[wavepointIndex];

        GameObject obj = Managers.Resource.Instantiate("UI_HPBar", (Managers.UI.SceneUI as UI_GameScene).transform, pooling: false);
        obj.gameObject.SetActive(false);
        hpBar = obj.GetComponent<UI_HPBar>();
        hpBar.SetInfo(this, MaxHp);
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

    void LateUpdate()
    {
        // HP 바의 위치를 업데이트
        if (hpBar != null)
        {
            hpBar.UpdatePosition();
        }
    }

    #region AI

    protected override void UpdateIdle()
    {
        if (isSpawn)
        {
            StartWait(0.5f);
            CreatureState = ECreatureState.Idle;
            isSpawn = false;
            return;
        }

        if (_coWait == null)
        {
            CreatureState = ECreatureState.Move;
            return;
        }
    }
    protected override void UpdateMove()
    {
        Vector2 direction = (destination.position - transform.position).normalized;
        transform.Translate(direction * MoveSpeed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, destination.position) <= 0.05f)
        {
            GoNextPoint();
        }

    }
    protected override void UpdateDead()
    {
    }
    #endregion	
    
	#region Battle
	public override void OnDamaged(BaseObject attacker)
	{
		base.OnDamaged(attacker);

        if (attacker.IsValid() == false)
			return;

		Hero hero = attacker as Hero;
		if (hero == null)
			return;

		float finalDamage = hero.Atk; // TODO
		Hp = Mathf.Clamp(Hp - finalDamage, 0, MaxHp);
          if (hpBar != null)
        hpBar.Refresh(Hp);
        Managers.Object.ShowDamageFont(CenterPosition, finalDamage, transform, false);


		if (Hp <= 0)
		{
			OnDead(attacker);
			CreatureState = ECreatureState.Dead;
		}
	}

	public override void OnDead(BaseObject attacker)
	{
        base.OnDead(attacker);

		// TODO : Drop Item
        Managers.Resource.Destroy(hpBar.gameObject);
		Managers.Object.Despawn(this);
	}
	#endregion

    void GoNextPoint()
    {
        wavepointIndex = (wavepointIndex + 1) % WayPoints.Length;
        destination = WayPoints[wavepointIndex];
        Sprite.flipX = wavepointIndex == 0 || wavepointIndex == 3;
    }


}
