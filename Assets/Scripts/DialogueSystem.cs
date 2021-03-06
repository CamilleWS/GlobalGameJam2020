﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI textMesh;
    public PlayerManager playerManager;
    private string textStr;
    private float timerDelay;
    public float displayDelay = 0.1f;
    private int currentCount = 0;
    private int maxCount = 0;
    private bool isPlayedAfter;
    public bool isDisplaying = false;

    public string[] replies;
    public bool[] repliesIsPlayedJustAfter;
    int replyCount;

    void Start()
    {
        timerDelay = displayDelay;
        replyCount = 0;
        playerManager = PlayerManager.instance;
    }

    void Update()
    {
        // if (Input.GetButtonDown("Fire1"))
        //     GetComponent<DialogueSystem>().playNextReply();
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit"))
            GetComponent<DialogueSystem>().GoToNext();
        if (isDisplaying) {
            timerDelay -= Time.deltaTime;
            if (timerDelay <= 0 && currentCount < maxCount) {
                currentCount += 1;
                timerDelay = displayDelay;
                textMesh.text = textStr.Substring(0, currentCount);
            } else if (currentCount >= maxCount) {
                isDisplaying = false;
            }
        }
    }

    public void displayText(string newText, bool playedAfter)
    {
        textMesh.text = "";
        textStr = newText;
        currentCount = 0;
        maxCount = textStr.Length;
        isDisplaying = true;
        playerManager.inputPaused = true;
        isPlayedAfter = playedAfter;
    }

    public void playNextReply()
    {
        if (replyCount < replies.Length) {
            displayText(replies[replyCount], repliesIsPlayedJustAfter[replyCount]);
            dialoguePanel.SetActive(true);
            replyCount += 1;
        } else {
            SceneManager.LoadScene("YouWin");
        }
    }

    public void GoToNext()
    {
        if (!isDisplaying && isPlayedAfter) {
            playNextReply();
        } else if (isDisplaying) {
            skipReply();
        } else {
            dialoguePanel.SetActive(false);
        }
    }

    void skipReply() {
        currentCount = maxCount;
        textMesh.text = textStr.Substring(0, currentCount);
        // Debug.Log(textMesh.characterInfo[0]);
        isDisplaying = false;
        playerManager.inputPaused = false;
    }
}
