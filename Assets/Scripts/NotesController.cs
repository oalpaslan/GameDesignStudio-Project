using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class NotesController : MonoBehaviour
{
    public static NotesController instance;
    public TextMeshProUGUI textComponent;
    public string[] notes;
    public int maxCharactersPerPage = 900;  // Maximum number of characters per page

    private List<string> pages = new List<string>();  // List to hold the split pages
    private int currentPageIndex = 0;  // Current page index
    private int noteIndex = -1;  // Index of the current note

    public bool isNoteOpen = false;  // Flag to check if the note is open

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        textComponent.text = string.Empty;
        gameObject.transform.GetComponent<Image>().enabled = false;
        textComponent.enabled = false;
    }

    void Update()
    {

    }

    public void OpenNote(string note)
    {
        Debug.Log("Note: " + note);
        noteIndex = Convert.ToInt32(note[note.Length - 1].ToString()) - 1;
        Debug.Log("Note index: " + noteIndex);
        Time.timeScale = 0;
        if (noteIndex >= 0 && noteIndex < notes.Length)
        {
            SplitTextIntoPages(notes[noteIndex]);
            currentPageIndex = 0;
            DisplayCurrentPage();
            gameObject.transform.GetComponent<Image>().enabled = true;
            textComponent.enabled = true;
            isNoteOpen = true;  // Set the flag to indicate the note is open
            Debug.Log("Opened note with " + pages.Count + " pages.");
        }
        else
        {
            Debug.LogError("Invalid note index");
        }
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
            Debug.Log("Start ind: " + startIndex + " End Ind: " + endIndex + " length: " + length + " textlength: " + text.Length);
            string page = text.Substring(startIndex, endIndex - startIndex).Trim();
            pages.Add(page);
            startIndex = endIndex + 1;
            Debug.Log("Added page: " + page);
        }
    }

    private void DisplayCurrentPage()
    {
        if (pages.Count > 0 && currentPageIndex >= 0 && currentPageIndex < pages.Count)
        {
            textComponent.text = pages[currentPageIndex];
            Debug.Log("Displaying page: " + currentPageIndex + " Content: " + pages[currentPageIndex]);
        }
        else
        {
            Debug.LogError("No pages to display or invalid page index");
        }
    }

    public void NextPage()
    {
        Debug.Log("Current page index: " + currentPageIndex + ", Total pages: " + pages.Count);
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

    public void CloseNote()
    {
        gameObject.transform.GetComponent<Image>().enabled = false;
        textComponent.enabled = false;
        currentPageIndex = 0;
        pages.Clear();
        isNoteOpen = false;  // Reset the flag when the note is closed
        Debug.Log("Closed note");
        Time.timeScale = 1;
        PlayerController2.instance.interactWithNote = false;

    }
}
