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
    private CoinsManager coinsManager;
    [SerializeField]
    private float bombExplodeChance = 0.5f; // 50% chance of exploding per second
    [SerializeField]
    private float initialDelay = 0.25f; // Initial delay in seconds

    private float currentMultiplier = 1.0f;
    private float multiplierIncreaseRate = 0.2f;
    private bool bombExploded = false;
    private bool canEndRound = true;

    void Update()
    {
        if (!bombExploded && canEndRound)
        {
            currentMultiplier += multiplierIncreaseRate * Time.deltaTime;
            multiplierText.text = $"Multiplier: x{currentMultiplier.ToString("F2")}";

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

        resultText.text = $"BOMB EXPLODED!\nYou lost {coinsManager.currentBet} coins";
        popUpObject.SetActive(true);
    }

    private void PlaceBet()
    {
        canEndRound = true;
        bombExploded = false;

        SetMainButtonToStop();
    }

    private void EndRound()
    {
        if (canEndRound)
        {
            mainButton.onClick.RemoveAllListeners();

            int winnings = Mathf.FloorToInt(coinsManager.currentBet * currentMultiplier);
            resultText.text = $"Congratulations!\nYou won {winnings} coins!";
            coinsManager.HasWonBet(winnings);
            popUpObject.SetActive(true);
            canEndRound = false;
        }
        else
        {
            resultText.text = "Too late!\nYou lost " + coinsManager.currentBet + " coins.";
            resultText.text = $"Too late!\nYou lost {coinsManager.currentBet} coins.";
        }
    }

    public void SetMainButtonToStart()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(() => StartCoroutine(StartMatchWithDelay()));
        mainButton.onClick.AddListener(buttonFeedbackEvent.Invoke);

        mainButtonImage.color = greenColor;
        mainButtonText.text = "Start";

        currentMultiplier = 1.0f; // Reset multiplier
        multiplierText.text = $"Multiplier: x{currentMultiplier.ToString("F2")}";
    }

    private void SetMainButtonToStop()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(EndRound);
        mainButton.onClick.AddListener(buttonFeedbackEvent.Invoke);

        mainButtonImage.color = redColor;
        mainButtonText.text = "Stop!";
    }

    public void ResetDefaultGameValues()
    {
        PlayerPrefs.DeleteAll(); // Clean PlayerPrefs
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // Restart the current scene
    }

    private IEnumerator StartMatchWithDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        PlaceBet();
    }
}
