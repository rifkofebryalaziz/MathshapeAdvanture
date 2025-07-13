using UnityEngine;

public class FinishHandler : MonoBehaviour
{
    public GameObject popupFail;
    public GameObject popupSuccess;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int currentScore = QuestionManager.Instance.score;

            if (currentScore < 80)
            {
                popupFail.SetActive(true);
            }
            else
            {
                popupSuccess.SetActive(true);
            }
        }
    }

    // 🔽 Tambahkan fungsi ini
    public void CloseFailPopup()
    {
        popupFail.SetActive(false);
    }
}
