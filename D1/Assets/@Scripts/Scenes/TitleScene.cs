using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    // 체계를 잡으면 생각하지 않아도 이런 공식처럼, 자신만의 프레임워크를 구축 
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.TitleScene;
        StartLoadAssets();
        return true;
    }

    private void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.UI.ShowSceneUI<UI_TitleScene>();
                (Managers.UI.SceneUI as UI_TitleScene).LoadComplateUI();
                // GetObject((int)GameObjects.StartImage).gameObject.SetActive(true);
                // GetText((int)Texts.DisplayText).text = $"Touch to Start";
                Debug.Log("Addressable All Load Complete");
            }
        });
    }
 

    public override void Clear()
    {
        
    }
}
