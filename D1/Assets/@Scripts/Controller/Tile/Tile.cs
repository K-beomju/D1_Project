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
            heroes[i].transform.position = newPosition;
        }
    }

    private Vector3 GetHeroPosition(int index)
    {
        return transform.position + TileInfo.HeroPositions[index];
    }

    public bool IsFull => heroes.Count >= 3;


}
