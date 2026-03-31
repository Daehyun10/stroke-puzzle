using UnityEngine;

[CreateAssetMenu(fileName = "NewHanja", menuName = "Puzzle/HanjaData")]
public class HanjaData : ScriptableObject
{
    [Header("한자 정보")]
    public string character;
    public string meaning;

    [Header("획순 데이터")]
    [Tooltip("정답 획 방향을 순서대로 입력")]
    public StrokeDirection[] correctSequence;

    [Header("힌트")]
    [TextArea] public string hint1;
    [TextArea] public string hint2;
    [TextArea] public string hint3;
}