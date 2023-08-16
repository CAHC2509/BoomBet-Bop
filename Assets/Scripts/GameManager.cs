using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Match UI settings")]
    [SerializeField]
    private TMP_InputField betInputField;
    [SerializeField]
    private TextMeshProUGUI multiplierText;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private GameObject popUpObject;

    [Space, Header("Gameplay settings")]
    [SerializeField]
    private float bombExplodeChance = 0.5f; // 50% chance of exploding per second

    private float currentMultiplier = 1.0f;
    private float multiplierIncreaseRate = 0.2f;
    private int currentBet = 0;
    private bool bombExploded = false;
    private bool canEndRound = true;

    void Update()
    {
        if (!bombExploded && canEndRound)
        {
            currentMultiplier += multiplierIncreaseRate * Time.deltaTime;
            multiplierText.text = "Multiplier: x" + currentMultiplier.ToString("F2");

            if (Random.value < bombExplodeChance * Time.deltaTime)
            {
                ExplodeBomb();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndRound();
        }
    }

    private void ExplodeBomb()
    {
        bombExploded = true;
        canEndRound = false;

        resultText.text = $"BOMB EXPLODED!\nYou lost {currentBet} coins";
        popUpObject.SetActive(true);

        currentMultiplier = 1.0f; // Reset multiplier
    }

    public void PlaceBet()
    {
        if (int.TryParse(betInputField.text, out currentBet))
        {
            canEndRound = true;
            bombExploded = false;
            popUpObject.SetActive(false);
        }
    }

    public void EndRound()
    {
        if (canEndRound)
        {
            int winnings = Mathf.FloorToInt(currentBet * currentMultiplier);
            resultText.text = "Congratulations!\nYou won " + winnings + " coins!";
            popUpObject.SetActive(true);
            canEndRound = false;
        }
        else
        {
            resultText.text = "Too late!\nYou lost " + currentBet + " coins.";
        }
    }
}
