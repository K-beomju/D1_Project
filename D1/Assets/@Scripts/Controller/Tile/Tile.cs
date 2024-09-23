using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Tile : BaseObject
{
    public List<Hero> heroes { get; set; } = new List<Hero>();
    private Color originColor;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Sprite.enabled = false;
        originColor = Sprite.color;
        Sprite.sortingOrder = SortingLayers.TILE;
        return true;
    }

    public void AddHero(Hero hero)
    {
        if (IsFull)
        {
            Debug.LogWarning("타일에 이미 최대 영웅 수가 배치되었습니다.");
            return;
        }
        heroes.Add(hero);
        RefreshHeroPositions();
    }

    public void RefreshHeroPositions(bool animate = false)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
                Vector3 newPosition = GetHeroPosition(i);
            if (!animate)
                heroes[i].transform.localPosition = newPosition;
            else
                StartCoroutine(MoveHeroCoroutine(heroes[i], newPosition, TileInfo.HeroMoveSpeed));
        }
    }






    public void SetShowTile(bool show)
    {
        Sprite.enabled = show;
    }

    public void HitSelectTile(bool press)
    {
        if (press)
            Sprite.color = Util.HexToColor("#75F2FF94");
        else
            Sprite.color = originColor;
    }

    public Hero GetHero()
    {
        if (heroes.Count == 0)
            throw new InvalidOperationException("The hero list is empty");

        return heroes[0];
    }



    private Vector3 GetHeroPosition(int index)
    {
        return transform.position + TileInfo.HeroPositions[index];
    }

    public bool IsFull => heroes.Count >= 3;

    public bool IsEmpty => heroes.Count == 0;

      // 코루틴을 사용한 MoveHero 메소드
    private IEnumerator MoveHeroCoroutine(Hero hero, Vector3 targetPosition, float speed)
    {
        hero.CreatureState = ECreatureState.Move;
        hero.LookLeft = hero.transform.position.x > targetPosition.x;

        // 영웅이 목표 위치에 도달할 때까지 매 프레임 이동
        while (Vector3.Distance(hero.transform.position, targetPosition) > 0.1f)
        {
            hero.transform.position = Vector3.MoveTowards(hero.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }

        // 이동이 끝난 후 상태 업데이트
        hero.transform.position = targetPosition; // 정확한 목표 위치로 설정
        hero.CreatureState = ECreatureState.Idle;
    }
}
