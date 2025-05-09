using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonScript : MonoBehaviour
{
    public string firstLevelSceneName = "Level1"; // Nama scene level pertama Anda

    public void PlayGame()
    {
        Debug.Log("Tombol Play ditekan!");
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Tombol Quit Application ditekan!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Untuk keluar dari Play Mode di Editor
#else
        Application.Quit(); // Untuk keluar dari aplikasi yang sudah di-build
#endif
    }

    // Anda bisa menambahkan fungsi-fungsi lain untuk tombol di Main Menu di sini
    // Misalnya, untuk membuka panel Credits, memilih bahasa, dll.
}