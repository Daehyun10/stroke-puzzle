using UnityEngine;
using UnityEngine.EventSystems;

public class StrokeInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float minStrokeDistance = 30f;

    private Vector2 startPos;
    private bool isDragging = false;

    private PuzzleManager puzzleManager;
    private StrokeDrawer strokeDrawer;

    private void Awake()
    {
        puzzleManager = FindFirstObjectByType<PuzzleManager>();
        strokeDrawer  = FindFirstObjectByType<StrokeDrawer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        startPos   = eventData.position;
        strokeDrawer?.StartDrawing(startPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;
        strokeDrawer?.StopDrawing();

        Vector2 endPos   = eventData.position;
        float   distance = Vector2.Distance(startPos, endPos);

        if (distance < minStrokeDistance)
        {
            strokeDrawer?.ClearAllLines();
            return;
        }

        StrokeDirection direction = GetDirection(startPos, endPos);
        puzzleManager.OnStrokeInput(direction);
    }

    private StrokeDirection GetDirection(Vector2 from, Vector2 to)
    {
        Vector2 delta = to - from;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        return AngleToDirection(angle);
    }

    private StrokeDirection AngleToDirection(float angle)
    {
        if (angle < 0) angle += 360f;
        if (angle >= 337.5f || angle < 22.5f)  return StrokeDirection.Right;
        if (angle >= 22.5f  && angle < 67.5f)  return StrokeDirection.UpRight;
        if (angle >= 67.5f  && angle < 112.5f) return StrokeDirection.Up;
        if (angle >= 112.5f && angle < 157.5f) return StrokeDirection.UpLeft;
        if (angle >= 157.5f && angle < 202.5f) return StrokeDirection.Left;
        if (angle >= 202.5f && angle < 247.5f) return StrokeDirection.DownLeft;
        if (angle >= 247.5f && angle < 292.5f) return StrokeDirection.Down;
        if (angle >= 292.5f && angle < 337.5f) return StrokeDirection.DownRight;
        return StrokeDirection.Right;
    }
}