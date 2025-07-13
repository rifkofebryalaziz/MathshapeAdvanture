using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("🟢 MainMenuManager: Start() dipanggil");

        if (AudioManager.instance != null)
        {
            Debug.Log("🎧 MainMenu: Memanggil PlayBackgroundMusic()");
            AudioManager.instance.PlayBackgroundMusic();
        }
        else
        {
            Debug.LogWarning("⚠️ AudioManager.instance = null. Pastikan AudioManager sudah ada di scene dan tidak terhapus.");
        }
    }
}
