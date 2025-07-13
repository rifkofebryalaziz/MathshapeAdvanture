using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player jatuh ke FallZone!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Sama seperti kena trap
        }
    }
}
