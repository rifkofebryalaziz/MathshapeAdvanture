using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Ganti scene dan pastikan Time.timeScale kembali ke normal
    public void ChangeScene(string name)
    {
        Time.timeScale = 1; // Sangat penting: pastikan game tidak dalam kondisi pause
        SceneManager.LoadScene(name);
    }

    // Pause game (berhenti semua pergerakan berbasis waktu)
    public void pause()
    {
        Time.timeScale = 0;
    }

    // Resume game (lanjutkan pergerakan)
    public void resume()
    {
        Time.timeScale = 1;
    }

    // Exit game (berfungsi saat build, tidak saat di Editor)
    public void ExitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }

    // Jika kamu punya tombol exit dari gameplay ke main menu
    public void ExitToMainMenu()
    {
        Time.timeScale = 1; // Reset waktu agar gameplay tidak tetap pause
        SceneManager.LoadScene("MainMenu"); // Ganti sesuai nama scene main menu kamu
    }

    // Fungsi untuk pindah ke level berikutnya (misalnya "Level2")
    public void GoToLevel2()
    {
        SceneManager.LoadScene("level2");
    }

    public void GoToLevel3()
    {
        SceneManager.LoadScene("level3");
    }


}
