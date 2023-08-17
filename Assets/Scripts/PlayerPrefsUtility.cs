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

    public static bool HasLoggedInBefore()
    {
        return PlayerPrefs.GetInt(HasLoggedInKey, 0) == 1;
    }

    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(CoinsKey, 0);
    }

    public static void UpdateCoins(int amount)
    {
        int currentCoins = GetCoins();
        int newTotalCoins = currentCoins + amount;
        SaveCoins(newTotalCoins);
    }

    private static void SaveCoins(int coins)
    {
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }
}
