using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;


[System.Serializable]
public class DialogueDict
{
    public string npcName;
    public string[] lines;
}
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public static Dialogue instance;
    //public Dictionary<int, string[]> dialogueDict;
    public float textSpeed;
    public bool isDialogueOpen = false;

    [SerializeField]
    private List<DialogueDict> dialogueDict = new();


    private int index;
    private string[] currentLines;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        textComponent.text = string.Empty;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && isDialogueOpen)
        {
            if (textComponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = currentLines[index];
            }
        }
        else if (Input.GetButtonDown("Back") && isDialogueOpen)
        {
            StopAllCoroutines();
            textComponent.text = string.Empty;

            gameObject.transform.GetComponent<Image>().enabled = false;
            textComponent.enabled = false;
            isDialogueOpen = false;
        }
    }

    public void StartDialogue(string npcName)
    {
        index = 0;

        currentLines = GetDialogueLines(npcName);
        gameObject.transform.GetComponent<Image>().enabled = true;
        textComponent.enabled = true;
        Debug.Log("Curr Lines: " + currentLines);
        isDialogueOpen = true;
        if (currentLines != null && currentLines.Length > 0)
        {


            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.LogWarning("No dialogue found for the specified NPC.");
        }
    }
    private string[] GetDialogueLines(string npcName)
    {

        foreach (DialogueDict dialogue in dialogueDict)
        {
            Debug.Log("DialogueLines : " + dialogue.lines[1]);
            Debug.Log("NPC index : " + npcName);
            Debug.Log("Dia index : " + dialogue.npcName);
            if (dialogue.npcName == npcName)
            {
                return dialogue.lines;
            }
        }
        return null;
    }

    IEnumerator TypeLine()
    {
        Debug.Log("Start TypeLine");
        Debug.Log(currentLines[index]);
        //lines = dialogueDict[currentNPC];
        foreach (char c in currentLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < currentLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textComponent.text = string.Empty;

            gameObject.transform.GetComponent<Image>().enabled = false;
            textComponent.enabled = false;
            isDialogueOpen = false;
        }
    }
}
