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
    [TextArea(0, 20)]
    public string lines;
}
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public static Dialogue instance;
    public float textSpeed;
    public bool isDialogueOpen = false;

    [SerializeField]
    private List<DialogueDict> dialogueDict = new();


    private int index, lineIndex,
        firstCharIndex, lastCharIndex, currentPageCount;
    private string currentLines;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        textComponent.text = string.Empty;
        GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().pageToDisplay = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && isDialogueOpen)
        {
            Debug.Log("INDEX: " + index);
            Debug.Log("PAGETODISPLAY: " + GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().pageToDisplay);
            NextLine();
        }
        else if (Input.GetButtonDown("Back") && isDialogueOpen || !PlayerController2.instance.interactWithNPC)
        {
            textComponent.text = string.Empty;
            index = 1;
            gameObject.transform.GetComponent<Image>().enabled = false;
            textComponent.enabled = false;
            isDialogueOpen = false;
        }
    }

    public void StartDialogue(string npcName)
    {
        index = 1;

        currentLines = GetDialogueLines(npcName);
        gameObject.transform.GetComponent<Image>().enabled = true;
        textComponent.enabled = true;

        isDialogueOpen = true;
        Debug.Log("isdiaopen: " + isDialogueOpen);
        if (currentLines != null && currentLines.Length > 0)
        {
            textComponent.text = currentLines;
            GameObject.Find("Text (TMP) - Dia").GetComponent<TextMeshProUGUI>().pageToDisplay = index;

        }
        else
        {
            Debug.LogWarning("No dialogue found for the specified NPC.");
        }
    }
    private string GetDialogueLines(string npcName)
    {

        foreach (DialogueDict dialogue in dialogueDict)
        {
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
        Debug.Log(currentLines);
        //lines = dialogueDict[currentNPC];
        foreach (char c in currentLines.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < GameObject.Find("Text (TMP) - Dia").GetComponent<TextMeshProUGUI>().textInfo.pageCount)
        {
            Debug.Log("nexte girdi:" + index);
            index++;
            GameObject.Find("Text (TMP) - Dia").GetComponent<TextMeshProUGUI>().pageToDisplay = index;
        }
        else
        {
            Debug.Log("buraya mý girdiaq");
            textComponent.text = string.Empty;
            index = 1;
            gameObject.transform.GetComponent<Image>().enabled = false;
            textComponent.enabled = false;
            isDialogueOpen = false;
        }
    }
}
