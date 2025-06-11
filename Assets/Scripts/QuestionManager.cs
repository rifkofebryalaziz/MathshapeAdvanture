using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    [Header("Question Database")]
    public QuestionData[] allQuestions;

    [Header("UI Elements")]
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public Button optionA_Button;
    public Button optionB_Button;
    public Button optionC_Button;
    public Button optionD_Button;
    public TextMeshProUGUI optionA_Text;
    public TextMeshProUGUI optionB_Text;
    public TextMeshProUGUI optionC_Text;
    public TextMeshProUGUI optionD_Text;
    public TextMeshProUGUI correctAnswerText;
    public GameObject correctAnswerPanel;
    public Button nextButton;

    [Header("Game Settings")]
    public float showAnswerDelay = 2f;

    private QuestionData currentQuestion;
    private List<QuestionData> availableQuestions;
    private bool hasAnswered = false;

    public static QuestionManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeQuestions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Setup button listeners
        optionA_Button.onClick.AddListener(() => OnAnswerSelected(0));
        optionB_Button.onClick.AddListener(() => OnAnswerSelected(1));
        optionC_Button.onClick.AddListener(() => OnAnswerSelected(2));
        optionD_Button.onClick.AddListener(() => OnAnswerSelected(3));
        nextButton.onClick.AddListener(CloseQuestion);

        // Hide question panel at start
        questionPanel.SetActive(false);
    }

    void InitializeQuestions()
    {
        availableQuestions = new List<QuestionData>(allQuestions);
        ShuffleQuestions();
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < availableQuestions.Count; i++)
        {
            QuestionData temp = availableQuestions[i];
            int randomIndex = Random.Range(i, availableQuestions.Count);
            availableQuestions[i] = availableQuestions[randomIndex];
            availableQuestions[randomIndex] = temp;
        }
    }

    public void ShowRandomQuestion()
    {
        if (availableQuestions.Count == 0)
        {
            // Jika semua soal sudah habis, shuffle ulang
            InitializeQuestions();
        }

        // Ambil soal acak dari list
        int randomIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[randomIndex];

        // Setup UI
        SetupQuestionUI();

        // Show panel
        questionPanel.SetActive(true);
        hasAnswered = false;

        // Enable all buttons
        SetButtonsInteractable(true);
        correctAnswerPanel.SetActive(false);
    }

    void SetupQuestionUI()
    {
        questionText.text = currentQuestion.question;
        optionA_Text.text = "A. " + currentQuestion.optionA;
        optionB_Text.text = "B. " + currentQuestion.optionB;
        optionC_Text.text = "C. " + currentQuestion.optionC;
        optionD_Text.text = "D. " + currentQuestion.optionD;

        // Reset button colors
        ResetButtonColors();
    }

    void OnAnswerSelected(int selectedAnswer)
    {
        if (hasAnswered) return;

        hasAnswered = true;

        // Disable all buttons
        SetButtonsInteractable(false);

        // Show correct answer
        bool isCorrect = selectedAnswer == currentQuestion.correctAnswerIndex;

        // Highlight selected answer
        HighlightButton(selectedAnswer, isCorrect);

        // Show correct answer if wrong
        if (!isCorrect)
        {
            HighlightButton(currentQuestion.correctAnswerIndex, true);
        }

        // Show correct answer text
        Invoke(nameof(ShowCorrectAnswer), showAnswerDelay);
    }

    void ShowCorrectAnswer()
    {
        correctAnswerText.text = $"Jawaban Benar: {currentQuestion.GetCorrectAnswerLetter()}. {currentQuestion.GetCorrectAnswerText()}";

        if (!string.IsNullOrEmpty(currentQuestion.explanation))
        {
            correctAnswerText.text += $"\n\nPenjelasan: {currentQuestion.explanation}";
        }

        correctAnswerPanel.SetActive(true);
    }

    void HighlightButton(int buttonIndex, bool isCorrect)
    {
        Button targetButton = buttonIndex switch
        {
            0 => optionA_Button,
            1 => optionB_Button,
            2 => optionC_Button,
            3 => optionD_Button,
            _ => optionA_Button
        };

        ColorBlock colors = targetButton.colors;
        colors.normalColor = isCorrect ? Color.green : Color.red;
        colors.selectedColor = isCorrect ? Color.green : Color.red;
        targetButton.colors = colors;
    }

    void ResetButtonColors()
    {
        Button[] buttons = { optionA_Button, optionB_Button, optionC_Button, optionD_Button };

        foreach (Button button in buttons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.selectedColor = Color.white;
            button.colors = colors;
        }
    }

    void SetButtonsInteractable(bool interactable)
    {
        optionA_Button.interactable = interactable;
        optionB_Button.interactable = interactable;
        optionC_Button.interactable = interactable;
        optionD_Button.interactable = interactable;
    }

    public void CloseQuestion()
    {
        questionPanel.SetActive(false);

        // Optional: Remove answered question from available list to avoid repetition
        // availableQuestions.Remove(currentQuestion);
    }
}