using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("퍼즐 데이터")]
    public HanjaData currentHanja;

    [Header("이펙트")]
    public CorrectEffect correctEffect;

    [Header("흔들림")]
    public ShakeEffect shakeTarget;

    //[Header("자물쇠")]
    //public LockAnimator lockAnimator;

    private StrokeValidator validator;
    private StrokeInputHandler inputHandler;
    private StrokeDrawer strokeDrawer;

    private void Awake()
    {
        validator = GetComponent<StrokeValidator>();
        if (validator == null)
            validator = gameObject.AddComponent<StrokeValidator>();

        inputHandler = FindFirstObjectByType<StrokeInputHandler>();
        strokeDrawer = FindFirstObjectByType<StrokeDrawer>();
    }

    private void Start()
    {
        InitPuzzle();
    }

    public void InitPuzzle()
    {
        validator.Init(currentHanja);

        validator.OnCorrectStroke  = OnCorrectStroke;
        validator.OnWrongStroke    = OnWrongStroke;
        validator.OnPuzzleSolved   = OnPuzzleSolved;
        validator.OnStrokeProgress = OnStrokeProgress;

        Debug.Log($"퍼즐 시작: {currentHanja.character} ({currentHanja.meaning})");
    }

    public void OnStrokeInput(StrokeDirection direction)
    {
        validator.ValidateStroke(direction);
    }

    private void OnCorrectStroke()
    {
        strokeDrawer?.SetLastLineCorrect();
        if (correctEffect != null)
            correctEffect.Play(transform.position);
        Debug.Log("획 정답 → 금색 + 파티클");
    }

    private void OnWrongStroke()
    {
        strokeDrawer?.SetLastLineWrong();
        if (shakeTarget != null)
            StartCoroutine(shakeTarget.Shake());
        Debug.Log("획 오답 → 빨강 + 흔들림");
    }

    private void OnPuzzleSolved()
    {
        //if (lockAnimator != null)
            //lockAnimator.PlayUnlock();
        Debug.Log("퍼즐 완료!");
    }

    private void OnStrokeProgress(int index)
    {
        Debug.Log($"진행도: {index}/{currentHanja.correctSequence.Length}");
    }
}