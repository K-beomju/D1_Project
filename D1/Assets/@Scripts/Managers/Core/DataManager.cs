using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Data;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, HeroInfoData> HeroInfoDic { get; private set; } = new Dictionary<int, HeroInfoData>();
    public Dictionary<int, ProjectileData> ProjectileDic { get; private set; } = new Dictionary<int, ProjectileData>();

    public void Init()
    {
        HeroInfoDic = LoadJson<HeroInfoDataLoader, int, HeroInfoData>("HeroInfoData").MakeDict();
        ProjectileDic = LoadJson<ProjectileDataLoader, int, ProjectileData>("ProjectileData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

}
