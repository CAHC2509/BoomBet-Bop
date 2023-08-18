using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CoinsManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField coinsInput;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private GameObject normalBetPanel;
    [SerializeField]
    private GameObject resetGamePanel;
    [SerializeField]
    private List<TextMeshProUGUI> coinsAmoutTexts;

    [HideInInspector]
    public int currentBet = 0;

    private void Start()
    {
        // Initialize player's coins if it's their first login
        if (!PlayerPrefsUtility.HasLoggedInBefore())
        {
            PlayerPrefsUtility.UpdateCoins(50);
            PlayerPrefsUtility.SetHasLoggedIn();
        }

        UpdateCoinsTexts();
    }

    /// <summary>
    /// Check if the entered bet amount is valid based on available coins.
    /// </summary>
    public void CheckCoinsAvailability()
    {
        if (string.IsNullOrEmpty(coinsInput.text) || coinsInput.text == "0")
        {
            confirmButton.interactable = false;
            return;
        }

        int actualCoins = PlayerPrefsUtility.GetCoins();
        int desiredBet = int.Parse(coinsInput.text);

        if (desiredBet > actualCoins)
        {
            coinsInput.text = actualCoins.ToString();
            desiredBet = actualCoins;
        }

        currentBet = desiredBet;
        confirmButton.interactable = true;
    }

    /// <summary>
    /// Process a placed bet by deducting coins and updating UI.
    /// </summary>
    public void BetPlaced()
    {
        if (currentBet > 0)
        {
            PlayerPrefsUtility.UpdateCoins(-currentBet);
            UpdateCoinsTexts();
            coinsInput.text = string.Empty;
        }
    }

    /// <summary>
    /// Update player's coins after winning a bet.
    /// </summary>
    public void HasWonBet(int winnings)
    {
        PlayerPrefsUtility.UpdateCoins(winnings);
        UpdateCoinsTexts();
    }

    /// <summary>
    /// Update displayed coins amount in various UI elements.
    /// </summary>
    public void UpdateCoinsTexts()
    {
        string coinsAmount = PlayerPrefsUtility.GetCoins().ToString();

        foreach (TextMeshProUGUI textMesh in coinsAmoutTexts)
            textMesh.text = coinsAmount;
    }

    /// <summary>
    /// Check if the player has enough coins for a bet or should reset the game.
    /// </summary>
    public void CheckForEnoughCoins()
    {
        bool hasEnoughCoins = PlayerPrefsUtility.GetCoins() > 0;

        normalBetPanel.SetActive(hasEnoughCoins);
        resetGamePanel.SetActive(!hasEnoughCoins);
    } 
}
