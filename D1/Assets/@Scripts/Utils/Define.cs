using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Define
{
    public enum EScene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene 
    }

    
    public enum EGameSceneState
    {
        None,
        Play,
        Pause,
        Boss,
        Over,
        Clear,
    }

    public enum EUIEvent 
    {
        Click,
        PointerDown,
        PointerUp,
        Drag
    }

    public enum EObjectType
	{
		None,
		Hero,
		Monster,
		Projectile,
		Effect
	}

    public enum ECreatureState
    {
        None,
        Idle,
        Move,
        Attack,
        Skill,
        Dead,
    }
    
    public enum ECreatureHero 
    {
        None,
        Idle, 
        Move,
        Skill,
    }

    public enum ECreatureMonster 
    {
        None,
        Move,
        Dead
    }

    public enum EAnimType 
    {
        Bool,
        SetTrigger
    }

    #region Projectile
    public enum EProjectileMotionType
    {
        Melee,
        Horizontal,
        Straight,
        Guided,
        Arc,
        ArcRotation,
        ImpactPosition,
        ImpactMoveStraight,
        ImpaactMoveArc,
        Target,
    }

    public enum EProjectileTargetingType
    {
        Single,
        Multi,
    }

    public enum EProjectileRemoveType
    {
        CollisionTarget,
        LifeTime,
        Position,
        PositionCollisionTarget,
    }

    public enum ELayer
    {
        Default = 0,
        Hero = 7,
        Monster = 8,
        Projectile = 9,
    }

    public enum EShootingType
    { 
        None,
        Fixed,
        Random,
    }
    #endregion


    public static readonly Vector2 TRAIL_POS = new Vector2(0, -2.65f);


    public const int HERO_BANDIT_ID = 201001;
    public const int HERO_ARCHER_ID = 201002;

    public const int MONSTER_GOBLIN_BABY_ID = 202001;

    public static class SortingLayers 
    {
        public const int TILE = 100;

        public const int WEAPON = 298;
        public const int HAND = 299;
        public const int CREATURE = 300;

        public const int PROJECTILE = 310;
        public const int SKILL_EFFECT = 315;
        public const int DAMAGE_FONT = 410; 
    }
    public static class AnimName
    {
        public const string ATTACK = "attack";
        public const string IDLE = "idle";
        public const string MOVE = "move";
        public const string DAMAGED = "hit";
        public const string DEAD = "dead";
    }

    public static class TileInfo
    {
        public static readonly Vector3[] HeroPositions = {
            new Vector3(0.1f, -0.05f, 0),
            new Vector3(-0.05f, 0.05f, 0),
            new Vector3(-0.05f, -0.15f, 0)
        };
    }
}
