using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StrokeDrawer : MonoBehaviour
{
    [Header("라인 설정")]
    public float lineWidth = 5f;
    public Color correctColor = new Color(1f, 0.84f, 0f, 1f);
    public Color wrongColor   = new Color(0.8f, 0.1f, 0.1f, 1f);
    public Color drawingColor = new Color(1f, 1f, 1f, 0.8f);

    private Camera mainCam;
    private LineRenderer currentLine;
    private List<LineRenderer> allLines = new List<LineRenderer>();
    private bool isDrawing = false;
    private List<Vector3> currentPoints = new List<Vector3>();

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!isDrawing || currentLine == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(mainCam.transform.position.z);
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        if (currentPoints.Count == 0 ||
            Vector3.Distance(currentPoints[currentPoints.Count - 1], mouseWorld) > 0.05f)
        {
            currentPoints.Add(mouseWorld);
            currentLine.positionCount = currentPoints.Count;
            currentLine.SetPositions(currentPoints.ToArray());
        }
    }

    public void StartDrawing(Vector2 screenPos)
    {
        isDrawing = true;
        currentPoints.Clear();

        GameObject lineObj = new GameObject("StrokeLine");
        currentLine = lineObj.AddComponent<LineRenderer>();

        currentLine.startWidth    = lineWidth * 0.01f;
        currentLine.endWidth      = lineWidth * 0.01f;
        currentLine.material      = new Material(Shader.Find("Sprites/Default"));
        currentLine.startColor    = drawingColor;
        currentLine.endColor      = drawingColor;
        currentLine.useWorldSpace = true;
        currentLine.sortingOrder  = 10;

        allLines.Add(currentLine);
    }

    public void StopDrawing()
    {
        isDrawing = false;
    }

    public void SetLastLineCorrect()
    {
        if (currentLine == null) return;
        StartCoroutine(AnimateLine(currentLine, correctColor, 1.8f, false));
    }

    public void SetLastLineWrong()
    {
        if (currentLine == null) return;
        StartCoroutine(AnimateLine(currentLine, wrongColor, 0.8f, true));
    }

    private IEnumerator AnimateLine(LineRenderer line, Color targetColor, float duration, bool clearAfter)
    {
        if (line == null) yield break;

        float elapsed = 0f;
        Color startColor = line.startColor;

        while (elapsed < 0.2f)
        {
            elapsed += Time.deltaTime;
            Color c = Color.Lerp(startColor, targetColor, elapsed / 0.2f);
            line.startColor = c;
            line.endColor   = c;
            yield return null;
        }

        line.startColor = targetColor;
        line.endColor   = targetColor;

        if (clearAfter)
        {
            yield return new WaitForSeconds(0.3f);
            elapsed = 0f;
            while (elapsed < 0.4f)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / 0.4f);
                Color c = targetColor;
                c.a = alpha;
                line.startColor = c;
                line.endColor   = c;
                yield return null;
            }
            ClearAllLines();
        }
    }

    public void ClearAllLines()
    {
        foreach (var line in allLines)
            if (line != null) Destroy(line.gameObject);
        allLines.Clear();
        currentLine = null;
    }
}