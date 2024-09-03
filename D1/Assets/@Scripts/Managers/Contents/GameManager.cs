using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private Transform[] _wayPoints = null;
    public Transform[] WayPoints
    {
        get
        {
            if (_wayPoints == null)
            {
                // WayPoints가 null이면 초기화
                Transform rootTransform = Managers.Object.GetRootTransform("@WayPoints");
                _wayPoints = new Transform[rootTransform.childCount];
                for (int i = 0; i < rootTransform.childCount; i++)
                {
                    _wayPoints[i] = rootTransform.GetChild(i);
                }
            }
            return _wayPoints;
        }
        set
        {
            _wayPoints = value;
        }
    }



    #region MonsterSpawn
    public void SpawnMonster(bool isBoss = false)
    {
        if (WayPoints == null)
            return;
            
        Managers.Object.Spawn<Monster>(WayPoints[0].transform.position, 0);
    }

    #endregion
}
