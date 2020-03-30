using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats

{
    public const float EASY = 1f;
    public const float MEDIUM = 1.5f;
    public const float HARD = 2f;
    public const float IMPOSSIBLE = 5f;
    // public const float EASY_MULTIPLIER = 1f;
    // public const float MEDIUM_MULTIPLIER = 1.5f;
    // public const float HARD_MULTIPLIER = 2f;


    public static float difficulty = EASY;
    public static float damage;
    public static int score;
    public static int kills;


    public static void ResetStats() 
    {
        difficulty = EASY;
        damage = 0f;
        score = 0;
        kills = 0;
    }
}
