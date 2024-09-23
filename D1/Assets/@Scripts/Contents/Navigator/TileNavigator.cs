using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNavigator : BaseObject
{
    private LineRenderer line;
    private Transform startCirle;
    private Transform endSquare;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        line = GetComponent<LineRenderer>();
        startCirle = Util.FindChild(gameObject, "StartPos").transform;
        endSquare = Util.FindChild(gameObject, "EndPos").transform;
        line.startWidth = .05f;
        line.endWidth = .05f;
        return true;
    }

    public void UpdatePosition(Vector3 startPos, Vector3 endPos)
    {
        startCirle.transform.localPosition = startPos;
        endSquare.transform.localPosition = endPos;

        line.SetPosition(0, startCirle.position);
        line.SetPosition(1, endSquare.position);

    }
}
