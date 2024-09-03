using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

// Creature, Projectile, Etc 
public class BaseObject : InitBase
{
    public EObjectType ObjectType { get; protected set; } = EObjectType.None;
    public CircleCollider2D Collider { get; private set; }
    public SpriteRenderer Sprite { get; private set; }

    public float ColliderRadius { get { return Collider != null ? Collider.radius : 0.0f; } }
    public Vector2 CenterPosition { get { return (Vector2)transform.position + Vector2.up * ColliderRadius; } }


    public int DataTemplateID { get; set; }

    private bool lookLeft = true;
    public bool LookLeft
    {
        get { return lookLeft; }
        set
        {
            lookLeft = value;
            Flip(value);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = gameObject.GetComponent<CircleCollider2D>();
        Sprite = GetComponent<SpriteRenderer>();
        return true;
    }

    public virtual void SetInfo(int dataTemplateID)
    {
        DataTemplateID = dataTemplateID;
    }

    public void LookAtTarget(BaseObject target)
    {
        if (target == null)
            return;
        Vector2 dir = target.transform.position - transform.position;
        if (dir.x < 0)
            LookLeft = true;
        else if (dir.x > 0)
            LookLeft = false;
    }

    public void Flip(bool flag)
    {
        Vector3 scale = transform.localScale;
        scale.x = flag ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public virtual void OnAnimEventHandler()
    {
        Debug.Log("OnAnimEventHandler");
    }
}
