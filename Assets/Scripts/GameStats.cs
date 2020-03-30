using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats

{
    // Difficulty constants
    public const float EASY = 1f;
    public const float MEDIUM = 1.5f;
    public const float HARD = 2f;
    public const float IMPOSSIBLE = 5f;

    // Score Multipliers
    public const float EASY_MULTIPLIER = 1f;
    public const float MEDIUM_MULTIPLIER = 1.5f;
    public const float HARD_MULTIPLIER = 2f;
    public const float IMPOSSIBLE_MULTIPLIER = 5f;

    public const int KILL_SCORE = 10;
    public const int CHILD_SCORE = 100;
    public const int WAVE_SCORE = 5;
    


    public static float difficulty = MEDIUM;
    public static float difficultyMultiplier = MEDIUM_MULTIPLIER;
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
