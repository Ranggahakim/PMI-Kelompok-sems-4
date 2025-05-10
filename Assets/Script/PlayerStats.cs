using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI collectedBodyText;

    public GameObject gateGameObject;

    public GameObject LosePanel;
    public GameObject WinPanel;
    public TextMeshProUGUI timerCountInWinPanel;

    public TextMeshProUGUI timerText;

    public int collectedBody;
    public int totalCollectedBody;

    public Transform spawnLocation;
    public float countdownTime = 60f;



    void Start()
    {
        collectedBodyText.text = $"{collectedBody}/{totalCollectedBody}";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            timerText.text = $"{countdownTime}s";
        }
        else
        {
            LosePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ObtainCollectedBody()
    {
        collectedBody++;
        collectedBodyText.text = $"{collectedBody}/{totalCollectedBody}";

        if (collectedBody >= totalCollectedBody)
        {
            gateGameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Exit")
        {
            WinPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            timerCountInWinPanel.text = $"{countdownTime} detik";
        }

        if (other.gameObject.tag == "Duri")
        {
            gameObject.transform.position = spawnLocation.position;
            Physics.SyncTransforms();
            Debug.Log("Teleport");
        }
    }
}
