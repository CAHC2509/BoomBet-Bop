using System.Collections;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float timeMultiplier = 3f; // This is the time in seconds to end the match
    private float normalTimeScale = 1f;

    public void IncreaseTimeSpeed(float timeLeft)
    {
        float newSpeed = timeLeft / timeMultiplier;
        Time.timeScale = newSpeed; // Set the new time scale to increase speed

        StartCoroutine(ResetTimeScaleCoroutine());
    }

    private IEnumerator ResetTimeScaleCoroutine()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 3.0f)
        {
            elapsedTime += Time.unscaledDeltaTime; // unscaledDeltaTime make the loop not affected by the new time scale
            yield return null;
        }

        Time.timeScale = normalTimeScale; // Restore the original time scale
    }
}
