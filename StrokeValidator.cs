using UnityEngine;

public class StrokeValidator : MonoBehaviour
{
    private HanjaData targetHanja;
    private int currentIndex = 0;
    private bool isSolved = false;

    public System.Action OnCorrectStroke;   // 획 하나 맞을 때
    public System.Action OnWrongStroke;     // 획 틀렸을 때
    public System.Action OnPuzzleSolved;    // 전체 정답
    public System.Action<int> OnStrokeProgress; // 현재 몇 번째 획인지

    public void Init(HanjaData hanja)
    {
        targetHanja = hanja;
        currentIndex = 0;
        isSolved = false;
        Debug.Log($"Validator 초기화: {hanja.character}, 총 {hanja.correctSequence.Length}획");
    }

    public void ValidateStroke(StrokeDirection input)
    {
        if (isSolved || targetHanja == null) return;

        StrokeDirection correct = targetHanja.correctSequence[currentIndex];

        if (input == correct)
        {
            currentIndex++;
            Debug.Log($"✓ {currentIndex}/{targetHanja.correctSequence.Length}획 정답");
            OnCorrectStroke?.Invoke();
            OnStrokeProgress?.Invoke(currentIndex);

            if (currentIndex >= targetHanja.correctSequence.Length)
            {
                isSolved = true;
                OnPuzzleSolved?.Invoke();
                Debug.Log("🎉 퍼즐 해결!");
            }
        }
        else
        {
            Debug.Log($"✗ 오답. 입력: {input}, 정답: {correct} → 리셋");
            currentIndex = 0;
            OnWrongStroke?.Invoke();
            OnStrokeProgress?.Invoke(0);
        }
    }

    public int GetCurrentIndex() => currentIndex;
    public bool IsSolved() => isSolved;
    public float GetProgress() => targetHanja == null ? 0 :
        (float)currentIndex / targetHanja.correctSequence.Length;
}