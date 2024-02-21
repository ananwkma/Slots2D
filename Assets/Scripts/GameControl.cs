using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static event Action HandlePulled = delegate { };
    [SerializeField] private Text bankText;
    [SerializeField] private Text prizeText;
    [SerializeField] private Row[] rows;
    [SerializeField] private Transform handle;

    private int bank;
    private int prizeValue;
    private bool resultsChecked = false;


    [SerializeField] private AudioSource casinoAmbience;
    [SerializeField] private AudioSource barAmbience;
    [SerializeField] private AudioSource jackpot;
    [SerializeField] private AudioSource coinPayout;
    [SerializeField] private AudioSource slotSound;
    

    // Start is called before the first frame update
    void Start()
    {
        bank = 100;
    }

    // Update is called once per frame
    void Update()
    {
        bankText.text = "$" + bank;
        // Show and hide results 
        if (!rows[0].rowStopped || !rows[1].rowStopped || !rows[2].rowStopped)
        {
            prizeValue = 0;
            prizeText.enabled = false;
            resultsChecked = false;
        }

        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped && !resultsChecked)
        {
            CheckResults();
            if (prizeValue != 0)
            {
                prizeText.enabled = true;
                prizeText.text = "Won $" + prizeValue + "!";
            }
            if (bank == 0)
            {
                prizeText.enabled = true;
                prizeText.text = "YOU LOSE";
            }
        }
    }

    // Response when handle pressed
    private void OnMouseDown()
    {
        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped)
            StartCoroutine(PullHandle());
    }
    
    // Handle animation 
    private IEnumerator PullHandle()
    {
        for (int i = 0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, i);
            yield return new WaitForSeconds(0.1f);
        }

        slotSound.Play();
        bank -= 100;
        HandlePulled();

        for (int i =0; i < 15; i += 5)
        {
            handle.Rotate(0f, 0f, -i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Helper functions for checking matching symbols
    private string allSame()
    {
        if (rows[0].stoppedSlot == rows[1].stoppedSlot && rows[2].stoppedSlot == rows[1].stoppedSlot) return rows[0].stoppedSlot;
        else return "";
    }

    private string twoSame()
    {
        if (rows[0].stoppedSlot == rows[1].stoppedSlot || rows[0].stoppedSlot == rows[2].stoppedSlot) return rows[0].stoppedSlot;
        else if (rows[1].stoppedSlot == rows[2].stoppedSlot) return rows[1].stoppedSlot;
        else return "";
    }

    // Sets prize depending on result
    private void CheckResults()
    {
        switch (twoSame())
        {
            case "Diamond":
                prizeValue = 100;
                break;
            case "Crown":
                prizeValue = 300;
                break;
            case "Melon":
                prizeValue = 500;
                break;
            case "Bar":
                prizeValue = 700;
                break;
            case "Seven":
                prizeValue = 1000;
                break;
            case "Cherry":
                prizeValue = 2000;
                break;
            case "Lemon":
                prizeValue = 4000;
                break;
            case "":
                break;
        }

        switch (allSame())
        {
            case "Diamond":
                prizeValue = 200; 
                break;
            case "Crown":
                prizeValue = 400; 
                break;
            case "Melon":
                prizeValue = 600; 
                break;
            case "Bar":
                prizeValue = 800; 
                break;
            case "Seven":
                prizeValue = 1500; 
                break;
            case "Cherry":
                prizeValue = 3000; 
                break;
            case "Lemon":
                prizeValue = 5000; 
                break;
            case "":
                break;
        }

        if (prizeValue != 0)
        {
            jackpot.Play();
            coinPayout.Play();
        }

        bank += prizeValue;
        resultsChecked = true;
    }
}
