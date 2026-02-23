using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveTime = 0.25f;

    public Facing facing = Facing.Right;

    Vector3 targetPos;

    public Vector2Int cellPos;
    public GridManager grid;

    void Start()
    {
        cellPos = grid.WorldToCell(transform.position);
        transform.position = grid.CellToWorld(cellPos);

        targetPos = transform.position;
        ApplyRotationInstant();
    }

    Vector3 DirVector()
    {
        switch (facing)
        {
            case Facing.Up: return Vector3.up;
            case Facing.Right: return Vector3.right;
            case Facing.Down: return Vector3.down;
            case Facing.Left: return Vector3.left;
        }
        return Vector3.right;
    }

    public IEnumerator MoveForward()
    {
        Vector2Int dir = DirToCellOffset();
        Vector2Int next = cellPos + dir;

        if (!grid.IsWalkable(next))
        {
            Debug.Log("Blocked by wall");
            yield break;
        }

        cellPos = next;
        Vector3 worldTarget = grid.CellToWorld(cellPos);

        yield return MoveTo(worldTarget);

        if (grid.IsGoal(cellPos))
        {
            Debug.Log("GOAL REACHED!");
        }
    }

    public IEnumerator TurnLeft()
    {
        facing = (Facing)(((int)facing + 3) % 4);
        yield return RotateToFacing();
    }

    public IEnumerator TurnRight()
    {
        facing = (Facing)(((int)facing + 1) % 4);
        yield return RotateToFacing();
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(moveTime);
    }

    IEnumerator MoveTo(Vector3 pos)
    {
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, pos, t);
            yield return null;
        }

        transform.position = pos;
    }

    IEnumerator RotateToFacing()
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

    Vector2Int DirToCellOffset()
    {
        switch (facing)
        {
            case Facing.Up: return Vector2Int.up;
            case Facing.Right: return Vector2Int.right;
            case Facing.Down: return Vector2Int.down;
            case Facing.Left: return Vector2Int.left;
        }
        return Vector2Int.right;
    }
}