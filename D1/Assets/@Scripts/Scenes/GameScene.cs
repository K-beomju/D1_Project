using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
    private EGameSceneState _gameSceneState = EGameSceneState.None;
    public EGameSceneState GameSceneState
    {
        get { return _gameSceneState; }
        set
        {
            if (_gameSceneState != value)
            {
                _gameSceneState = value;
                SwitchCoroutine();
            }
        }
    }

    [SerializeField] private float _detectionDelay = 0.2f;
    [SerializeField] private float _monsterSpawnDelay = 0.5f;
    [SerializeField] private int _startGameDelay = 1;

    private Coroutine _detectionCoroutine;

    private IEnumerator _currentCoroutine = null;
    private void SwitchCoroutine()
    {
        IEnumerator coroutine = null;

        switch (GameSceneState)
        {
            case EGameSceneState.Play:
                coroutine = CoPlayStage();
                break;
            case EGameSceneState.Pause:
                coroutine = CoPauseStage();
                break;
            case EGameSceneState.Boss:
                coroutine = CoBossStage();
                break;
            case EGameSceneState.Over:
                coroutine = CoStageOver();
                break;
            case EGameSceneState.Clear:
                coroutine = CoStageClear();
                break;
        }

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = coroutine;
        StartCoroutine(_currentCoroutine);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.GameScene;

        //TODO
        Managers.Map.LoadMap();
        Managers.Object.Init();
        Managers.UI.ShowSceneUI<UI_GameScene>();

        GameSceneState = EGameSceneState.Play;


        if (_detectionCoroutine == null)
        {
            _detectionCoroutine = StartCoroutine(CoPerformDetection());
        }

        return true;
    }

    private IEnumerator CoPerformDetection()
    {
        WaitForSeconds wait = new WaitForSeconds(_detectionDelay);
        yield return wait;

        while (true)
        {
            Managers.Object.PerformMatching();
            yield return wait;
        }
    }

    private IEnumerator CoPlayStage()
    {
        WaitForSeconds startWait = new WaitForSeconds(_startGameDelay);
        yield return startWait;

        StartCoroutine(Managers.Game.SpawnMonsterCo(_monsterSpawnDelay, 10, false));
    }

    private IEnumerator CoPauseStage()
    {
        yield return null;
    }

    private IEnumerator CoBossStage()
    {
        // BossBattleTimer = BossBattleTimeLimit;
        // Managers.Game.SpawnMonster(Data, true);
        // while (true)
        // {
        //     BossBattleTimer = Mathf.Clamp(BossBattleTimer - Time.deltaTime, 0.0f, BossBattleTimeLimit);

        //     if (BossBattleTimer <= 0)
        //     {
        //         GameSceneState = EGameSceneState.Over;
        //         yield return null;
        //     }

        yield return null;
        // }
    }

    private IEnumerator CoStageOver()
    {
        foreach (Hero hero in Managers.Object.Heroes)
        {
            hero.CreatureState = ECreatureState.Dead;
        }

        yield return null;
    }

    private IEnumerator CoStageClear()
    {
        foreach (Monster monster in Managers.Object.Monsters)
        {
            monster.CreatureState = ECreatureState.Dead;
        }
        yield return null;
    }

    public override void Clear()
    {

    }

}
