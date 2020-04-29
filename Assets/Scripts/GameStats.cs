using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats
{
    // Difficulty constants
    // Multiplier for number of enemies and rough multiplier for number of children.
    public const float EASY = 1f;
    public const float MEDIUM = 1.5f;
    public const float HARD = 2f;
    public const float IMPOSSIBLE = 5f;

    public const int BASE_CHILDREN = 2; // Represents the number of children on easy difficulty

    // Multiplier constants
    public const float EASY_MULTIPLIER = 1f;
    public const float MEDIUM_MULTIPLIER = 1.5f;
    public const float HARD_MULTIPLIER = 2f;
    public const float IMPOSSIBLE_MULTIPLIER = 5f;

    public const int KILL_SCORE = 25;
    public const int CHILD_SCORE = 250;
    public const int WAVE_SCORE = 50;

    
    


    // Stats
    public static float difficulty = EASY;
    public static float difficultyMultiplier = EASY_MULTIPLIER;

    public static int totalChildren = (int)(BASE_CHILDREN * difficultyMultiplier);

    public static int childrenSaved;
    public static int kills;
    public static int damage;
    public static int score;


    public static void ResetStats() 
    {
        difficulty = EASY;
        difficultyMultiplier = EASY_MULTIPLIER;

        totalChildren = BASE_CHILDREN;

        childrenSaved = 0;
        kills = 0;
        damage = 0;
        score = 0;
    }
}
