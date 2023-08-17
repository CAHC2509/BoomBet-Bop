using System.Collections;
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
    private List<TextMeshProUGUI> coinsAmoutTexts;

    public int currentBet = 0;

    private void Start()
    {
        if (!PlayerPrefsUtility.HasLoggedInBefore())
        {
            PlayerPrefsUtility.UpdateCoins(50);
            PlayerPrefsUtility.SetHasLoggedIn();
        }

        UpdateCoinsTexts();
    }

    public void CheckCoinsAvailability()
    {
        if (string.IsNullOrEmpty(coinsInput.text) || coinsInput.text == "0")
        {
            currentBet = 0;
            confirmButton.interactable = false;
            return;
        }

        int actualCoins = PlayerPrefsUtility.GetCoins();
        int desiredBet = int.Parse(coinsInput.text);

        if (desiredBet > actualCoins)
            coinsInput.text = actualCoins.ToString();

        currentBet = desiredBet;
        confirmButton.interactable = true;
    }

    public void BetPlaced()
    {
        PlayerPrefsUtility.UpdateCoins(-currentBet);
        UpdateCoinsTexts();
    }

    public void HasWonBet(int winnings)
    {
        PlayerPrefsUtility.UpdateCoins(winnings);
        UpdateCoinsTexts();
    }

    public void UpdateCoinsTexts()
    {
        string coinsAmount = PlayerPrefsUtility.GetCoins().ToString();

        foreach (TextMeshProUGUI textMesh in coinsAmoutTexts)
            textMesh.text = coinsAmount;
    }
}
