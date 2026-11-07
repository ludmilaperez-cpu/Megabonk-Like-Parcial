using System;

public static class EventManager
{
    public static event Action<float> OnCoinsChanged;

    public static void RaiseCoinsChanged(float totalCoins)
    {
        OnCoinsChanged?.Invoke(totalCoins);
    }
}
