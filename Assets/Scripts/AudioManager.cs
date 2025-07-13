using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    public AudioClip walk, jump, died, correct, wrong, next, knockback, background;

    [Header("Audio Sources")]
    public AudioSource sfxSource;         // Untuk SFX umum
    public AudioSource walkSource;        // Khusus walk
    public AudioSource backgroundSource;  // Musik latar

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Dipanggil otomatis saat scene berubah
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("🎬 Pindah scene ke: " + scene.name);

        if (scene.name == "mainmenu")
        {
            StopBackgroundMusic(); // Berhentiin musik di MainMenu
        }
        else if (scene.name == "gameplay" || scene.name == "level2" || scene.name == "level3")
        {
            PlayBackgroundMusic(); // Mulai musik di level
        }
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "walk":
                if (!walkSource.isPlaying && walk != null)
                {
                    walkSource.clip = walk;
                    walkSource.loop = true;
                    walkSource.Play();
                }
                break;
            case "jump":
                if (jump != null) sfxSource.PlayOneShot(jump);
                break;
            case "died":
                if (died != null) sfxSource.PlayOneShot(died);
                break;
            case "correct":
                if (correct != null) sfxSource.PlayOneShot(correct);
                break;
            case "wrong":
                if (wrong != null) sfxSource.PlayOneShot(wrong);
                break;
            case "next":
                if (next != null) sfxSource.PlayOneShot(next);
                break;
            case "knockback":
                if (knockback != null) sfxSource.PlayOneShot(knockback);
                break;
        }
    }

    public void StopWalkSound()
    {
        if (walkSource.isPlaying)
            walkSource.Stop();
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundSource.isPlaying && background != null)
        {
            backgroundSource.clip = background;
            backgroundSource.loop = true;
            backgroundSource.Play();
            Debug.Log("▶️ Background music dimulai");
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundSource.isPlaying)
        {
            backgroundSource.Stop();
            Debug.Log("⏹️ Background music dihentikan");
        }
    }
}
