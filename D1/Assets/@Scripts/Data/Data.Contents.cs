using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Data
{
    #region HeroData
    [Serializable]
    public class HeroInfoData 
    {
        public int DataId;
        public string Name;
        public string AnimName;
        public float Atk;
        public float AtkSpeed;
        public float AtkRange;
        public float AtkDelay;
    }

    [Serializable]
	public class HeroInfoDataLoader : ILoader<int, HeroInfoData>
	{
		public List<HeroInfoData> heroes = new List<HeroInfoData>();
		public Dictionary<int, HeroInfoData> MakeDict()
		{
			Dictionary<int, HeroInfoData> dict = new Dictionary<int, HeroInfoData>();
			foreach (HeroInfoData hero in heroes)
				dict.Add(hero.DataId, hero);
			return dict;
		}
	}
    #endregion

    #region ProjectileData
    [Serializable]
    public class ProjectileData 
    {
        public int DataId;
        public string Name;
        public EProjetionMotion ProjectileMotion;
        public string PrefabName;
        public int ProjSpeed;
    }

    [Serializable]
	public class ProjectileDataLoader : ILoader<int, ProjectileData>
	{
		public List<ProjectileData> projectiles = new List<ProjectileData>();
		public Dictionary<int, ProjectileData> MakeDict()
		{
			Dictionary<int, ProjectileData> dict = new Dictionary<int, ProjectileData>();
			foreach (ProjectileData proj in projectiles)
				dict.Add(proj.DataId, proj);
			return dict;
		}
	}
    #endregion


}