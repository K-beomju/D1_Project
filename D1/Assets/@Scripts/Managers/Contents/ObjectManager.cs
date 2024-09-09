using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

// 스폰과 디스폰 기능, 컨테이너를 리스트로 할 것이냐, 
public class ObjectManager
{
    //리스트에 비해서 바로 찾는 게 편리하기 때문에 
    public HashSet<Hero> Heroes = new HashSet<Hero>();
    public HashSet<Monster> Monsters = new HashSet<Monster>();

    #region Root
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public Transform HeroRoot { get { return GetRootTransform("@Heroes"); } }
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform TileRoot { get { return GetRootTransform("@Tiles"); } }

    #endregion

    public T Spawn<T>(Vector2 position, int templateId) where T : BaseObject
    {
        string prefabName = typeof(T).Name;
        if(typeof(T).Name.Contains("Hero"))
            prefabName = Managers.Data.HeroInfoDic[templateId].Name;


        GameObject go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        BaseObject obj = go.GetComponent<BaseObject>();

        if (obj.ObjectType == EObjectType.Hero)
        {
            obj.transform.parent = HeroRoot;
            Hero hero = go.GetComponent<Hero>();
            Heroes.Add(hero);
            hero.SetInfo(templateId);

            Managers.Map.PlaceHeroOnTile(hero, templateId);
        }
        else if (obj.ObjectType == EObjectType.Monster)
        {
            obj.transform.parent = HeroRoot;
            Monster monster = go.GetComponent<Monster>();
            Monsters.Add(monster);
            monster.SetInfo(templateId);
        }
        else if (obj.ObjectType == EObjectType.Projectile)
        {

        }
        // 왜 갑자기 오브젝트를 T로 
        // 오브젝트는 베이스 오브젝트 T는 베이스 오브젝트를 상속받은 그 무언가 이기 때문에 
        return obj as T;
    }

    public void Despawn<T>(T obj) where T : BaseObject
    {
        EObjectType objectType = obj.ObjectType;

		if (obj.ObjectType == EObjectType.Hero)
		{
			Hero hero = obj.GetComponent<Hero>();
			Heroes.Remove(hero);
		}
		else if (obj.ObjectType == EObjectType.Monster)
		{
			Monster monster = obj.GetComponent<Monster>();
			Monsters.Remove(monster);
		}

        Managers.Resource.Destroy(obj.gameObject);
    }


    public GameObject SpawnTile(Vector3 position, string prefabName)
    {
        GameObject go = Managers.Resource.Instantiate(prefabName, pooling: false);
        go.transform.position = position;
        go.transform.parent = TileRoot;
        return go;
    }

    public void PerformMatching()
    {
        List<Monster> MonsterList = Monsters.ToList();

        // 3-1. 영웅 -> 몬스터
        foreach (Hero hero in Heroes)
        {
            Creature target = FindClosestTarget(hero, MonsterList);
            if (target.IsValid())
            {
                hero.Target = target;
            }
        }
    }

    private T FindClosestTarget<T>(Hero hero, List<T> targets) where T : Creature
    {
        float shortestDistance = Mathf.Infinity;
        T closestTarget = null;
        T realTarget = null;

        foreach (T target in targets)
        {
            float distance = (hero.transform.position - target.transform.position).sqrMagnitude;

            if (target.IsValid() == false)
            {
                continue;
            }

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestTarget = target;
            }
        }

        if (closestTarget != null & shortestDistance <= hero.AtkRange)
        {
            realTarget = closestTarget;
        }
        else
        {
            realTarget = null;
        }

        return realTarget;
    }



    public void Clear()
    {
        Heroes.Clear();
        Monsters.Clear();
    }
}
