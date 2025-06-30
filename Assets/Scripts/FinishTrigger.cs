using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinishTrigger : MonoBehaviour
{
    public GameObject popupPanel; // Panel untuk pop up gagal

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (QuestionManager.Instance.score >= 80)
        {
            Debug.Log("✅ Finish terbuka, skor mencukupi!");
            // Lanjut ke level berikutnya
            // Contoh: SceneManager.LoadScene("NextLevel");
        }
        else
        {
            Debug.Log("❌ Skor kurang dari 80");
            if (popupPanel != null)
                popupPanel.SetActive(true);
        }
    }
}


