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
            currentMultiplier += multiplierIncreaseRate * Time.deltaTime;
            multiplierText.text = $"Multiplier: x{currentMultiplier.ToString("F2")}";

            bombTimer += Time.deltaTime;

            if (bombTimer >= bombDuration)
                ExplodeBomb();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            mainButton.onClick.Invoke();
    }

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

    private void PlaceBet()
    {
        bombExploded = false;
        bombDuration = Random.Range(5f, 10f);
        Debug.Log(bombDuration);

        SetMainButtonToStop();
    }

    private void RemoveBet()
    {
        betRemoved = true;

        mainButton.gameObject.SetActive(!betRemoved);
        withdrawnText.SetActive(betRemoved);

        winnings = Mathf.FloorToInt(coinsManager.currentBet * currentMultiplier);
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
        bombTimer = 0f;
        currentMultiplier = 1.0f; // Reset multiplier
        multiplierText.text = $"Multiplier: x{currentMultiplier.ToString("F2")}";
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
