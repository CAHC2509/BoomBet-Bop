using System.Collections;
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
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private GameObject popUpObject;
    [SerializeField]
    private GameObject withdrawnText;

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
    private TimeController timeController;
    [SerializeField]
    private float initialDelay = 0.25f; // Initial delay in seconds

    private float currentMultiplier = 1f;
    private float multiplierIncreaseRate = 0.2f;
    private int winnings;

    private float bombDuration = 0f;
    private float bombTimer = 0f;

    private bool bombExploded = false;
    private bool betRemoved = false;

    void Update()
    {
        if (!bombExploded)
        {
            // Increase the current multiplier over time
            currentMultiplier += multiplierIncreaseRate * Time.deltaTime;
            multiplierText.text = $"Multiplier: x{currentMultiplier.ToString("F2")}";

            bombTimer += Time.deltaTime;

            // Update the timer text
            string formattedTime = timeController.FormatTime(bombTimer);
            timerText.text = formattedTime;

            if (bombTimer >= bombDuration)
                ExplodeBomb();
        }

        // Check for Space key during match
        if (Input.GetKeyDown(KeyCode.Space))
            mainButton.onClick.Invoke();
    }

    /// <summary>
    /// Handles the bomb explosion event.
    /// </summary>
    private void ExplodeBomb()
    {
        bombExploded = true;
        mainButton.onClick.RemoveAllListeners();

        if (!betRemoved)
        {
            resultText.text = $"BOMB EXPLODED!\nYou lost {coinsManager.currentBet} coins";
        }
        else
        {
            resultText.text = $"Congratulations!\nYou won {winnings} coins!";
            coinsManager.HasWonBet(winnings);
        }

        popUpObject.SetActive(true);
    }

    /// <summary>
    /// Set the initial bet values
    /// </summary>
    private void PlaceBet()
    {
        bombExploded = false;
        bombDuration = Random.Range(0f, 60f); // Choise a random time from 0 to 60 seconds
        Debug.Log(bombDuration);

        SetMainButtonToStop();
    }

    /// <summary>
    /// Remove the bet and increase time speed if needed.
    /// </summary>
    private void RemoveBet()
    {
        betRemoved = true;

        mainButton.onClick.RemoveAllListeners();
        mainButton.gameObject.SetActive(!betRemoved);
        withdrawnText.SetActive(betRemoved);

        // Calculate winnings based on the current bet and multiplier
        winnings = Mathf.FloorToInt(coinsManager.currentBet * currentMultiplier);

        float timeLeft = bombDuration - bombTimer;

        // If timeLeft is more than timeMultiplier, increase time speed
        if (timeLeft > timeController.timeMultiplier)
            timeController.IncreaseTimeSpeed(timeLeft);
    }

    public void SetMainButtonToStart()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(() => StartCoroutine(StartMatchWithDelay()));
        mainButton.onClick.AddListener(buttonFeedbackEvent.Invoke);

        mainButtonImage.color = greenColor;
        mainButtonText.text = "Start";
    }

    private void SetMainButtonToStop()
    {
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(RemoveBet);
        mainButton.onClick.AddListener(buttonFeedbackEvent.Invoke);

        mainButtonImage.color = redColor;
        mainButtonText.text = "Stop!";
    }

    public void ResetMatchValues()
    {
        betRemoved = false;
        bombTimer = 0f; // Reset timer
        timerText.text = timeController.FormatTime(bombTimer);

        currentMultiplier = 1.0f; // Reset multiplier
        multiplierText.text = $"Multiplier: x{currentMultiplier.ToString("F2")}";

        withdrawnText.SetActive(false);
        mainButton.gameObject.SetActive(true);

        SetMainButtonToStart();
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
