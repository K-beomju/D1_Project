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
        
        
        return true;
    }


    public override void Clear()
    {
        
    }
}
