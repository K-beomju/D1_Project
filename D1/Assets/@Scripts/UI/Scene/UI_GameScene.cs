using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_GameScene : UI_Scene
{
    enum Buttons
    {
        SpawnHeroButton
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        GetButton((int)Buttons.SpawnHeroButton).onClick.AddListener(OnSpawnHeroButtonClick);
        return true;
    }

    private void OnSpawnHeroButtonClick()
    {
      int RandNormalHeroId = HERO_BANDIT_ID + Random.Range(0, 2);

        if (Managers.Map.IsFull(RandNormalHeroId))
        {
            Tile lastTile = Managers.Map.GetLastTile();
            if(!lastTile.IsFull && lastTile.heroes[0].DataTemplateID != RandNormalHeroId)
            {
                Managers.Object.Spawn<Hero>(Vector2.zero, lastTile.heroes[0].DataTemplateID);
            }
            return;
        }

        Managers.Object.Spawn<Hero>(Vector2.zero, RandNormalHeroId);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Managers.Object.Spawn<Monster>(Vector2.zero, 0);
        }
    }
}
