using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}