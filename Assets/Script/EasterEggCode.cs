using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class EasterEggCode : MonoBehaviour
{
    public string pass;
    public string answer = "421836";

    public TextMeshProUGUI eastereggtxt;

    public GameObject merah;

    public VideoPlayer jumpScare;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InsertNumber(int number)
    {
        if (pass.Length < 6)
            pass += number;

        eastereggtxt.text = pass.PadRight(6, 'X');

    }

    public void ClearInput()
    {
        pass = "";

        eastereggtxt.text = pass.PadRight(6, 'X');
    }

    public void PressApply()
    {
        if (pass == answer)
        {
            StartCoroutine(BENER());
        }
        else
        {
            StartCoroutine(SALAH());
        }
    }

    IEnumerator BENER()
    {
        jumpScare.gameObject.SetActive(true);
        jumpScare.Play();

        yield return new WaitForSeconds((float)jumpScare.length);

        jumpScare.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator SALAH()
    {
        merah.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        merah.SetActive(false);
        gameObject.SetActive(false);
    }
}
