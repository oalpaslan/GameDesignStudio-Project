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
    private int currentPageIndex = 0;  // Current page index
    private string currentNoteName;  // Name of the current note

    public bool isNoteOpen = false;  // Flag to check if the note is open


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
            if (textComponent.text == pages[currentPageIndex])
            {
                NextPage();
            }
            else
            {
                textComponent.text = pages[currentPageIndex];
            }
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
        currentNoteName = noteName;
        Time.timeScale = 0;

        string noteContent = GetNoteContent(noteName);

        if (noteContent != null)
        {
            SplitTextIntoPages(noteContent);
            currentPageIndex = 0;
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

    private void SplitTextIntoPages(string text)
    {
        pages.Clear();
        int startIndex = 0;
        while (startIndex < text.Length)
        {
            int length = Mathf.Min(maxCharactersPerPage, text.Length - startIndex);

            int endIndex = startIndex + length;
            if (endIndex < text.Length)
            {
                int lastSpaceIndex = text.LastIndexOf(' ', endIndex);

                // Ensure the space is within the segment
                if (lastSpaceIndex > startIndex)
                {
                    endIndex = lastSpaceIndex;
                }
            }
            string page = text.Substring(startIndex, endIndex - startIndex).Trim();
            pages.Add(page);
            startIndex = endIndex + 1;
        }
    }

    private void DisplayCurrentPage()
    {
        if (pages.Count > 0 && currentPageIndex >= 0 && currentPageIndex < pages.Count)
        {
            textComponent.text = pages[currentPageIndex];
        }
        else
        {
            Debug.LogError("No pages to display or invalid page index");
        }
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Count - 1)
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
        currentPageIndex = 0;
        pages.Clear();
        isNoteOpen = false;  // Reset the flag when the note is closed
        Time.timeScale = 1;
        PlayerController2.instance.interactWithNote = false;

    }
}
