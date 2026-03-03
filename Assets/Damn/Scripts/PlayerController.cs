using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Timing")]
    public float moveTime = 0.25f;

    [Header("State")]
    public Facing facing = Facing.Right;
    public Vector2Int cellPos;

    [Header("Refs")]
    public GridManager grid;

    Vector2Int startCell;
    Facing startFacing;
    public event Action<Vector2Int, Vector2Int> OnMoved;

    void Start()
    {
        cellPos = grid.WorldToCell(transform.position);
        transform.position = grid.CellToWorld(cellPos);

        startCell = cellPos;
        startFacing = facing;

        ApplyRotationInstant();
    }

    // =====================================================
    // MOVEMENT API
    // =====================================================

    public bool TryGetForwardCell(out Vector2Int next)
{
    next = cellPos + FacingToVector(facing);
    return grid.IsWalkable(next) || grid.IsHole(next);
}

    public Vector2Int GetRelativeCell(RelativeDirection dir)
    {
        return cellPos + GetRelativeOffset(dir);
    }

    public bool IsGoalCell(Vector2Int cell)
    {
        return grid.IsGoal(cell);
    }

    public void CommitMove(Vector2Int newCell)
    {
        Vector2Int oldCell = cellPos;
        cellPos = newCell;
        OnMoved?.Invoke(oldCell, newCell);
    }

    public void CommitTurn(bool right)
    {
        if (right)
            facing = (Facing)(((int)facing + 1) % 4);
        else
            facing = (Facing)(((int)facing + 3) % 4);
    }

    public IEnumerator AnimateWait(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    // =====================================================
    // ANIMATION
    // =====================================================

    public IEnumerator AnimateMove(Vector2Int targetCell)
    {
        Vector3 start = transform.position;
        Vector3 end = grid.CellToWorld(targetCell);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }

    public IEnumerator AnimateRotate()
    {
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.Euler(0, 0, FacingToAngle());

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.rotation = Quaternion.Lerp(start, end, t);
            yield return null;
        }

        transform.rotation = end;
    }

    // =====================================================
    // RESET
    // =====================================================

    public void ResetPlayer()
    {
        cellPos = startCell;
        facing = startFacing;

        transform.position = grid.CellToWorld(cellPos);
        ApplyRotationInstant();
    }

    // =====================================================
    // INTERNAL LOGIC
    // =====================================================

    void ApplyRotationInstant()
    {
        transform.rotation = Quaternion.Euler(0, 0, FacingToAngle());
    }
    
    float FacingToAngle()
    {
        switch (facing)
        {
            case Facing.Up: return 90;
            case Facing.Right: return 0;
            case Facing.Down: return -90;
            case Facing.Left: return 180;
        }
        return 0;
    }

    Vector2Int FacingToVector(Facing f)
    {
        switch (f)
        {
            case Facing.Up: return Vector2Int.up;
            case Facing.Right: return Vector2Int.right;
            case Facing.Down: return Vector2Int.down;
            case Facing.Left: return Vector2Int.left;
        }
        return Vector2Int.up;
    }

    // =====================================================
    // RELATIVE DIRECTION SYSTEM (FOR IF COMMAND)
    // =====================================================

    Vector2Int GetRelativeOffset(RelativeDirection dir)
    {
        // Rotation mapping:
        // Ahead  = +0
        // Right  = +1
        // Behind = +2
        // Left   = +3

        int rotationOffset = 0;

        switch (dir)
        {
            case RelativeDirection.Ahead: rotationOffset = 0; break;
            case RelativeDirection.Right: rotationOffset = 1; break;
            case RelativeDirection.Behind: rotationOffset = 2; break;
            case RelativeDirection.Left: rotationOffset = 3; break;
        }

        int finalFacing =
            ((int)facing + rotationOffset) % 4;

        return FacingToVector((Facing)finalFacing);
    }
    public bool IsWall(Vector2Int cell)
    {
        return !grid.IsWalkable(cell);
    }

    public bool IsGround(Vector2Int cell)
    {
        return grid.IsWalkable(cell) && !grid.IsGoal(cell);
    }

    public bool IsGoal(Vector2Int cell)
    {
        return grid.IsGoal(cell);
    }
    public Vector2Int GetFacingVector()
{
    return FacingToVector(facing);
}
}