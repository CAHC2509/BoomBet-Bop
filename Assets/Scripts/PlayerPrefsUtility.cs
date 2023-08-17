using UnityEngine;

public static class PlayerPrefsUtility
{
    private const string CoinsKey = "Coins";
    private const string HasLoggedInKey = "HasLoggedIn";

    public static void SetHasLoggedIn()
    {
        PlayerPrefs.SetInt(HasLoggedInKey, 1);
        PlayerPrefs.Save();
    }

    public static void UpdateCoins(int amount)
    {
        int currentCoins = GetCoins();
        int newTotalCoins = currentCoins + amount;

        if (newTotalCoins < 0)
            newTotalCoins = 0; // Ensure the value is not negative

        SaveCoins(newTotalCoins);
    }

    private static void SaveCoins(int coins)
    {
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }

    public static bool HasLoggedInBefore() => PlayerPrefs.GetInt(HasLoggedInKey, 0) == 1;

    public static int GetCoins() => PlayerPrefs.GetInt(CoinsKey, 0);
}
