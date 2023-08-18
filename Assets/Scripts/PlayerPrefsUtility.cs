using UnityEngine;

/// <summary>
/// Utility class for managing player's coins and login status using PlayerPrefs.
/// </summary>
public static class PlayerPrefsUtility
{
    private const string CoinsKey = "Coins";
    private const string HasLoggedInKey = "HasLoggedIn";

    /// <summary>
    /// Mark that the player has logged in.
    /// </summary>
    public static void SetHasLoggedIn()
    {
        PlayerPrefs.SetInt(HasLoggedInKey, 1);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Update the player's coin amount by the given amount.
    /// </summary>
    public static void UpdateCoins(int amount)
    {
        int currentCoins = GetCoins();
        int newTotalCoins = currentCoins + amount;

        if (newTotalCoins < 0)
            newTotalCoins = 0; // Ensure the value is not negative

        SaveCoins(newTotalCoins);
    }

    /// <summary>
    /// Save the provided coin amount to PlayerPrefs.
    /// </summary>
    /// <param name="coins">The coin amount to save.</param>
    private static void SaveCoins(int coins)
    {
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Check if the player has logged in before.
    /// </summary>
    /// <returns>True if the player has logged in before, false otherwise.</returns>
    public static bool HasLoggedInBefore() => PlayerPrefs.GetInt(HasLoggedInKey, 0) == 1;

    /// <summary>
    /// Get the current amount of coins the player has.
    /// </summary>
    /// <returns>The current amount of coins.</returns>
    public static int GetCoins() => PlayerPrefs.GetInt(CoinsKey, 0);
}
