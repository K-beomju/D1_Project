using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    // 타일맵의 가로와 세로 타일 개수 및 타일 간 간격
    private readonly int tilesX = 6;
    private readonly int tilesY = 3;
    private readonly float tileGap = 0.05f; // 타일 간의 간격

    // 타일맵의 월드 좌표상 최소 및 최대 위치
    private Vector3 worldMinPos;
    private Vector3 worldMaxPos;

    // 타일맵 직사각형 영역의 크기 
    private float totalWidth;
    private float totalHeight;

    // 각 타일과 그 위에 배치된 영웅을 저장하는 딕셔너리
    private Dictionary<Tile, Hero> heroMapDic = new Dictionary<Tile, Hero>();

    public GameObject map { get; private set; }

    // 맵을 로드하는 메서드
    public void LoadMap()
    {
        // "BaseMap"이라는 이름의 맵을 인스턴스화하고 초기 위치 설정
        map = Managers.Resource.Instantiate("BaseMap");
        map.transform.position = new Vector3(0, 0.5f, 0);
        map.name = "@BaseMap";

        // "Terrain_playerbox"라는 이름의 타일맵을 찾고, 타일맵의 경계를 최소화
        Tilemap tm = Util.FindChild<Tilemap>(map, "Terrain_playerbox", true);
        tm.CompressBounds();

        // 타일맵의 월드 좌표 경계를 계산
        (worldMinPos, worldMaxPos) = GetTileMapBounds(tm);

        // 계산된 경계를 바탕으로 타일맵 위에 타일 생성
        CreateTileInTileMap(worldMinPos, worldMaxPos);
    }

    // 타일맵의 월드 좌표 경계를 계산하는 메서드
    private (Vector3 worldMinPos, Vector3 worldMaxPos) GetTileMapBounds(Tilemap tm)
    {
        // 타일맵의 셀 좌표 경계를 월드 좌표로 변환
        worldMinPos = tm.CellToWorld(new Vector3Int(tm.cellBounds.xMin, tm.cellBounds.yMin, 0));
        worldMaxPos = tm.CellToWorld(new Vector3Int(tm.cellBounds.xMax, tm.cellBounds.yMax, 0));

        // 경계값을 반환
        return (worldMinPos, worldMaxPos);
    }

    // 타일맵 위에 타일을 생성하는 메서드
    private void CreateTileInTileMap(Vector3 worldMinPos, Vector3 worldMaxPos)
    {
        // 타일맵 직사각형 영역의 크기 계산
        totalWidth = worldMaxPos.x - worldMinPos.x;
        totalHeight = worldMaxPos.y - worldMinPos.y;
        Debug.Log(totalWidth);
        // 각각의 타일 크기 계산 (간격 제외)
        float tileWidth = (totalWidth - (tileGap * (tilesX - 1))) / tilesX;
        float tileHeight = (totalHeight - (tileGap * (tilesY - 1))) / tilesY;


        // 타일 생성 및 배치
        for (int x = 0; x < tilesX; x++)
        {
            for (int y = tilesY - 1; y >= 0; y--)
            {
                // 각 타일의 중앙 위치 계산 (간격 포함)
                float posX = worldMinPos.x + (x * (tileWidth + tileGap)) + (tileWidth / 2);
                float posY = worldMinPos.y + (y * (tileHeight + tileGap)) + (tileHeight / 2);
                Vector3 rectCenter = new Vector3(posX, posY, 0);

                // 타일 게임 오브젝트 생성 및 초기화
                GameObject go = Managers.Object.SpawnTile(rectCenter, "Tile"); go.transform.localScale = new Vector3(tileWidth, tileHeight, 1); // z축 크기는 1로 설정
                go.name = $"Tile_{x + 1}_{tilesY - y}";  // 타일 이름 설정

                // 타일 컴포넌트 추가 및 정보 설정
                Tile tile = go.GetOrAddComponent<Tile>();

                // 생성된 타일을 딕셔너리에 추가 (영웅은 아직 없음)
                heroMapDic.Add(tile, null);
            }
        }
    }

    #region Helpers 
    // 영웅을 배치할 수 있는 타일을 찾는 메서드
    public void PlaceHeroOnTile(Hero hero, int templateId)
    {
        Tile emptyTile = null;

        foreach (var map in heroMapDic)
        {
            // Find Same Hero -> Check : { Null, DataId, IsFull } 
            if (map.Value != null && map.Value.DataTemplateID == templateId && !map.Key.IsFull)
            {
                map.Key.AddHero(hero); // 타일에 영웅을 추가
                heroMapDic[map.Key] = hero; // 딕셔너리에 영웅을 매핑
                return;
            }

            // Empty Tile -> Check : { tile(Have Hero), null }  
            if (map.Value == null && emptyTile == null)
            {
                emptyTile = map.Key;
            }
        }

        // 빈 타일이 있으면 해당 타일에 영웅을 배치하고 추가한다.
        if (emptyTile != null)
        {
            emptyTile.AddHero(hero); // 타일에 영웅을 추가
            heroMapDic[emptyTile] = hero; // 딕셔너리에 영웅을 매핑
        }

    }



    public bool IsFull(int templateId)
    {
        foreach (var map in heroMapDic)
        {
            if (!map.Key.IsFull)
            {
                if (map.Value == null)
                    return false;
                else if (map.Value.DataTemplateID == templateId)
                    return false;
            }
        }

        Debug.LogWarning("모든 타일의 영웅이 가득 참");
        return true;
    }

    public Tile GetLastTile()
    {
        return heroMapDic.Last().Key;
    }
    #endregion

}
