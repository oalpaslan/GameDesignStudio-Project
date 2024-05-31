using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public class NoteDict
{
    public string noteName;  // Name of the note
    public string content;  // Content of the note
}
public class NotesController : MonoBehaviour
{
    public static NotesController instance;
    public TextMeshProUGUI textComponent;
    public int maxCharactersPerPage = 900;  // Maximum number of characters per page

    [SerializeField]
    private List<NoteDict> noteDict = new();

    private List<string> pages = new();  // List to hold the split pages
    //private int currentPageIndex = -1;  // Current page index
    private int currentPageIndex = 0;  // Current page index

    public bool isNoteOpen = false;  // Flag to check if the note is open

    public string noteContent;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.transform.GetComponent<Image>().enabled = false;
        GameObject.Find("BackgroundUI").GetComponent<Image>().enabled = false;

        textComponent.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isNoteOpen)
        {
            NextPage();
        }
        else if (Input.GetButtonDown("Cancel") && isNoteOpen)
        {
            CloseNote();
        }
        else if (Input.GetButtonDown("Back") && isNoteOpen)
        {
            PreviousPage();
        }
    }

    public void OpenNote(string noteName)
    {
        Time.timeScale = 0;

        noteContent = GetNoteContent(noteName);
        if (noteContent != null)
        {
            Debug.Log(noteContent);
            currentPageIndex = 1;
            DisplayCurrentPage();
            gameObject.transform.GetComponent<Image>().enabled = true;
            GameObject.Find("BackgroundUI").GetComponent<Image>().enabled = true;
            textComponent.enabled = true;
            isNoteOpen = true;  // Set the flag to indicate the note is open
        }
        else
        {
            Debug.LogError("Invalid note index");
        }
    }
    private string GetNoteContent(string noteName)
    {
        foreach (NoteDict note in noteDict)
        {
            if (note.noteName == noteName)
            {
                return note.content;
            }
        }
        return null;
    }

    private void DisplayCurrentPage()
    {

        if (noteContent != null)
        {
            textComponent.text = noteContent;
            GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().pageToDisplay = currentPageIndex;
        }
        else
        {
            Debug.LogError("No pages to display or invalid page index");
        }

    }

    public void NextPage()
    {

        if (currentPageIndex < GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().textInfo.pageCount)
        {
            currentPageIndex++;
            DisplayCurrentPage();
        }
        else
        {
            CloseNote();
        }
    }
    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            DisplayCurrentPage();
        }


    }

    public void CloseNote()
    {
        gameObject.transform.GetComponent<Image>().enabled = false;
        GameObject.Find("BackgroundUI").GetComponent<Image>().enabled = false;

        textComponent.enabled = false;
        //currentPageIndex = 0;
        currentPageIndex = 1;
        pages.Clear();
        isNoteOpen = false;  // Reset the flag when the note is closed
        Time.timeScale = 1;
        PlayerController2.instance.interactWithNote = false;

    }
}
