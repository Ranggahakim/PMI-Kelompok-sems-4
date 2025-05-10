using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] DialogueIndex[] dialogues;

    public GameObject char1;
    public GameObject char2;

    public TextMeshProUGUI dialogueText;
    public Image char1Img;
    public Image char2Img;

    public int dialogueIndex;

    public int nextLevelIndex { get; set; }
    public string nextLevelName { get; set; }

    private AsyncOperationHandle<string> localizationOperationHandle;

    void Start()
    {
        ApplyText();
    }

    public void PressNext()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogues[nextLevelIndex].dialogueCodes.Length)
        {
            ApplyText();
        }
        else
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }

    void ApplyText()
    {
        DialogueCode dialogue = dialogues[nextLevelIndex].dialogueCodes[dialogueIndex];

        if (dialogue.charIndex == 0)
        {
            char1.SetActive(true);
            char2.SetActive(false);

            if (dialogue.charImg != null)
                char1Img.sprite = dialogue.charImg;
        }
        else
        {
            char2.SetActive(true);
            char1.SetActive(false);

            if (dialogue.charImg != null)
                char2Img.sprite = dialogue.charImg;
        }

        // Ambil teks yang dilokalisasi berdasarkan dialogueKey
        if (!string.IsNullOrEmpty(dialogue.dialogueKey) && dialogueText != null)
        {
            if (localizationOperationHandle.IsValid())
            {
                localizationOperationHandle.Release();
            }

            localizationOperationHandle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("DialogueTable", dialogue.dialogueKey); // Ganti dengan nama String Table Anda
            localizationOperationHandle.Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    dialogueText.text = handle.Result;
                }
                else
                {
                    Debug.LogError($"Gagal melokalisasi teks dengan key '{dialogue.dialogueKey}': {handle.OperationException}");
                    // Tampilkan key sebagai fallback jika lokalisasi gagal
                    dialogueText.text = dialogue.dialogueKey;
                }
            };
        }
        else if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }

    private void OnDestroy()
    {
        if (localizationOperationHandle.IsValid())
        {
            localizationOperationHandle.Release();
        }
    }
}

[System.Serializable]
public class DialogueIndex
{
    public DialogueCode[] dialogueCodes;
}

[System.Serializable]
public class DialogueCode
{
    public int charIndex;
    public Sprite charImg;
    public string dialogueKey; // Sudah diubah
}