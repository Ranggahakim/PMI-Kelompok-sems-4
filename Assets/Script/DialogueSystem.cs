using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

            dialogueText.text = dialogue.dialogueText;
        }
        else
        {
            char2.SetActive(true);
            char1.SetActive(false);

            if (dialogue.charImg != null)
                char2Img.sprite = dialogue.charImg;

            dialogueText.text = dialogue.dialogueText;
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
    public string dialogueText;
}