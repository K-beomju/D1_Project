using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instacnce;
    private static Managers Instance { get { Init(); return s_instacnce; } }

    #region Core
    private DataManager _data = new DataManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private UIManager _ui = new UIManager();
    private SceneManagerEx _scene = new SceneManagerEx();

    public static DataManager Data { get { return Instance?._data; } }
    public static PoolManager Pool { get { return Instance?._pool; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static UIManager UI { get { return Instance?._ui; } }
    public static SceneManagerEx Scene { get { return Instance?._scene;} } 
    #endregion
    
    public static void Init()
    {
        if (s_instacnce == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            //초기화
            s_instacnce = go.GetComponent<Managers>();
        }
    }

}
