using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
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
    private GameObject popUpObject;
    [SerializeField]
    private TextMeshProUGUI resultText;

    [Space, Header("Main button settings")]
    [SerializeField]
    private Button mainButton;
    [SerializeField]
    private Image mainButtonImage;
    [SerializeField]
    private TextMeshProUGUI mainButtonText;
    [SerializeField]
    private Color redColor;
    [SerializeField]
    private Color greenColor;
    [SerializeField]
    private UnityEvent buttonFeedbackEvent;

    [Space, Header("Gameplay settings")]
    [SerializeField]
    private float bombExplodeChance = 0.5f; // 50% chance of exploding per second
    [SerializeField]
    private float initialDelay = 0.25f; // Initial delay in seconds

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
                ExplodeBomb();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            mainButton.onClick.Invoke();
    }

    private void ExplodeBomb()
    {
        bombExploded = true;
        canEndRound = false;

        mainButton.onClick.RemoveAllListeners();

        resultText.text = $"BOMB EXPLODED!\nYou lost {currentBet} coins";
        popUpObject.SetActive(true);

        currentMultiplier = 1.0f; // Reset multiplier
    }

    private void PlaceBet()
    {
        if (int.TryParse(betInputField.text, out currentBet))
        {
            canEndRound = true;
            bombExploded = false;

            SetMainButtonToStop();
        }
    }

    private void EndRound()
    {
        if (canEndRound)
        {
            mainButton.onClick.RemoveAllListeners();

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

    public void SetMainButtonToStart()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(() => StartCoroutine(StartMatchWithDelay()));
        mainButton.onClick.AddListener(buttonFeedbackEvent.Invoke);

        mainButtonImage.color = greenColor;
        mainButtonText.text = "Start";

        multiplierText.text = "Multiplier: x1.00";
    }

    private void SetMainButtonToStop()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(EndRound);
        mainButton.onClick.AddListener(buttonFeedbackEvent.Invoke);

        mainButtonImage.color = redColor;
        mainButtonText.text = "Stop!";
    }

    private IEnumerator StartMatchWithDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        PlaceBet();
    }
}
