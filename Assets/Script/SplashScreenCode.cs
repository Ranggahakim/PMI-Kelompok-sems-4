using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreenCode : MonoBehaviour
{
    [SerializeField] VideoPlayer telyuVP;
    [SerializeField] VideoPlayer splashScreenVP;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartVideo(telyuVP));
    }

    IEnumerator StartVideo(VideoPlayer video)
    {
        video.EnableAudioTrack(0, true);
        video.Play();

        yield return new WaitForSeconds((float)video.length);

        video.Stop();

        yield return new WaitForSeconds(0.2f);

        if (video != splashScreenVP)
            StartCoroutine(StartVideo(splashScreenVP));
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
