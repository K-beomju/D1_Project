using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static Define;

public abstract class BaseScene : InitBase
{
    // 씬 타입은 기본적으로 여기서 관리 이걸 자식들인 너희들이 정해달라
    public EScene SceneType { get; protected set; } = EScene.Unknown;
     
    public override bool Init()
    {
        if(base.Init() == false)
        return false;   

        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);

        Object obj = GameObject.FindAnyObjectByType(typeof(EventSystem));
        if(obj == null)
        {
            GameObject go = new GameObject { name = "@EventSystem" };
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
        return true;
    }


    //추상함수, 이에대한 결정은 뒤에서 내리는 
    public abstract void Clear();
}
