using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_Scene
{
    enum GameObjects
    {
        StartImage
    }

    enum Texts
    {
        DisplayText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));

        GetObject((int)GameObjects.StartImage).BindEvent((evt) =>
        {
            Debug.Log("ChangeScene");
            Managers.Scene.LoadScene(Define.EScene.GameScene);
        });

        GetObject((int)GameObjects.StartImage).gameObject.SetActive(false);
        GetText((int)Texts.DisplayText).text = $"";

        return true;
    }

    public void LoadComplateUI()
    {
        GetObject((int)GameObjects.StartImage).gameObject.SetActive(true);
        GetText((int)Texts.DisplayText).text = $"Touch to Start";
    }
}
