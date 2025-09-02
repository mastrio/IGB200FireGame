using UnityEngine;

// static class for storing any data that needs to be shared across scenes.
// e.g. what scenario is being played
public static class Global
{
    public static int scenarioNum = 1;

    // Method for resetting global data.
    // Should only be called when returning to the main menu / scenario select menu after winning/losing a scenario.
    // Any data that you don't want to be reset when returning to the main menu shouldn't be set here.
    public static void ResetData()
    {
        scenarioNum = 1;
    }
}
