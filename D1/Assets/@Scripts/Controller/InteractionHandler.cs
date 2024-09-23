using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionHandler : BaseObject
{
    private bool isDragging = false;
    private bool bFirstOtherTile = false;

    private Camera mainCam;
    private Collider2D selectedTile = null;
    private Collider2D previousTile = null;
    private TileNavigator tileNavigator;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        mainCam = Camera.main;
        return true;
    }

    private void Update()
    {
        HandlePCInput();
    }

    private void HandlePCInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
                return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HandleDown(mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                HandleDrag();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                HandleUp();
            }
        }
    }

    private void HandleDown(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask(Define.TileInfo.TileLayer));
        if (hit.collider != null && !Managers.Map.GetTile(hit.collider).IsEmpty)
        {
            selectedTile = hit.collider;
            isDragging = true;
            Managers.Map.AllActiveTile(true);
        }
    }

    private void HandleDrag()
    {
        if (selectedTile != null)
        {
            RaycastHit2D targetTile = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask(Define.TileInfo.TileLayer));

            if (targetTile.collider != null)
            {
                if (previousTile == null || previousTile != targetTile.collider)
                {
                    if (previousTile != null)
                    {
                        Managers.Map.GetTile(previousTile).HitSelectTile(false);

                        if (!bFirstOtherTile)
                        {
                            bFirstOtherTile = true;
                            if (tileNavigator == null)
                                tileNavigator = Managers.Resource.Instantiate("TileNavigator", transform).GetComponent<TileNavigator>();
                            else
                                tileNavigator.gameObject.SetActive(true);
                        }
                    }

                    previousTile = targetTile.collider;
                    Managers.Map.GetTile(previousTile).HitSelectTile(true);

                    if (bFirstOtherTile)
                    {
                        Vector3 selectedTileWorldPos = selectedTile.transform.position;
                        Vector3 previousTileWorldPos = previousTile.transform.position;
                        tileNavigator.UpdatePosition(selectedTileWorldPos, previousTileWorldPos);
                    }
                }
            }
        }
    }

    private void HandleUp()
    {
        if (selectedTile != null)
        {
            Managers.Map.AllActiveTile(false);

            RaycastHit2D targetTile = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile"));
            if (targetTile.collider != null && targetTile.collider != selectedTile)
            {
                Managers.Map.MovedHero(selectedTile, targetTile.collider);
            }

            selectedTile = null;
            bFirstOtherTile = false;

            if (tileNavigator != null)
                tileNavigator.gameObject.SetActive(false);
        }
    }


}
