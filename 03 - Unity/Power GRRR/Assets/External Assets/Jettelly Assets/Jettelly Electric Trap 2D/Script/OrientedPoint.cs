using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OrientedPoint
{
    public Vector3 Position;
    public Quaternion Orientation;

    public OrientedPoint(Vector3 position, Quaternion orientation)
    {
        this.Position = position;
        this.Orientation = orientation;
    }

    public OrientedPoint(Vector3 position, Vector3 orientation)
    {
        this.Position = position;
        float angle = Mathf.Atan2(orientation.y, orientation.x) * Mathf.Rad2Deg;
        this.Orientation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public Vector3 LocalToWorldPosition(Vector3 localPosition)
    {
        return Position + Orientation * localPosition;
    }

    public Vector3 LocalToWorldVector(Vector3 localPosition)
    {
        return Orientation * localPosition;
    }
}
