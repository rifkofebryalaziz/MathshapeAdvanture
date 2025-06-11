using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/Question Data")]
public class QuestionData : ScriptableObject
{
    [Header("Soal")]
    [TextArea(3, 5)]
    public string question;

    [Header("Pilihan Jawaban")]
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;

    [Header("Jawaban Benar")]
    [Range(0, 3)]
    public int correctAnswerIndex; // 0=A, 1=B, 2=C, 3=D

    [Header("Penjelasan (Opsional)")]
    [TextArea(2, 4)]
    public string explanation;

    // Method untuk mendapatkan jawaban benar dalam bentuk huruf
    public string GetCorrectAnswerLetter()
    {
        return correctAnswerIndex switch
        {
            0 => "A",
            1 => "B",
            2 => "C",
            3 => "D",
            _ => "A"
        };
    }

    // Method untuk mendapatkan text jawaban benar
    public string GetCorrectAnswerText()
    {
        return correctAnswerIndex switch
        {
            0 => optionA,
            1 => optionB,
            2 => optionC,
            3 => optionD,
            _ => optionA
        };
    }
}
