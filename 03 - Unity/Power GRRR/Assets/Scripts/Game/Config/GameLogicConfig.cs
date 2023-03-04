using UnityEngine;

namespace GGJ23.Game.Config
{
    [CreateAssetMenu(fileName = "GameLogicConfig_Default", menuName = "ScriptableObjects/GameLogicConfig", order = 1)]
    public class GameLogicConfig : ScriptableObject
    {
        // --- Variables ---
        [Header("General")]
        [SerializeField] private int _earlyDayDurationMs = 10000;
        [SerializeField] private int _finalDayDurationMs = 40000;
        [SerializeField] private int _nightDurationMs = 10000;
        [SerializeField] private float _fullEnergyDaySpeedMultiplier = 2f;

        [Header("Loose Points")]
        [SerializeField] private float _loosePointsSec = 1.0f;
        [SerializeField] private float _allGoodLoosePointRegainSec = 1.0f;

        [Header("Score")]
        [SerializeField] private float _powerIncreaseScoreMultiplier = 1f;
        [SerializeField] private float _powerDecreaseScoreMultiplier = 0.1f;
        [SerializeField] private float _fullEnergyScoreMultiplier = 2f;

        [Header("Design Tuning")]
        [Range(0f, 900f)] [SerializeField] private float _lossPointsNeeded = 100;
        [Range(5f, 30f)] [SerializeField] private float _maxDifficulty = 15f;
        [Range(2.5f, 20f)] [SerializeField] private float _timeToMaxDifficulty = 10f;
        [Range(1f, 50f)] [SerializeField] private float _maxDifficultyFromDistance = 15f;
        [Range(200f, 2000f)] [SerializeField] private float _distanceToMaxDifficulty = 500f;
        [Range(0.1f, 1.5f)] [SerializeField] private float _difficultyFactorFromMultipleBreakages = 0.235f;

        [Header("Design Tuning (Puzzle)")]
        [Range(1, 20)] [SerializeField] private int _timeToMaxPuzzleCountDifficulty = 10;
        [Range(1, 20)] [SerializeField] private int _puzzleMinNumber = 2;
        [Range(1, 20)] [SerializeField] private int _puzzleMaxNumber = 6;
        [Range(1, 20)] [SerializeField] private int _timeToMaxPuzzleRepetionDifficulty = 4;
        [Range(1, 20)] [SerializeField] private int _puzzleStartRepetion = 2;
        [Range(1, 20)] [SerializeField] private int _puzzleEndRepetion = 1;

        // --- Properties ---
        public int FinalDayDurationMs => _finalDayDurationMs;
        public int EarlyDayDurationMs => _earlyDayDurationMs;
        public int NightDurationMs => _nightDurationMs;

        public float LossPointsNeeded => _lossPointsNeeded;
        public float MaxDifficulty => _maxDifficulty;
        public float TimeToMaxDifficulty => _timeToMaxDifficulty;
        public float MaxDifficultyFromDistance => _maxDifficultyFromDistance;
        public float DistanceToMaxDifficulty => _distanceToMaxDifficulty;
        public float DifficultyFactorFromMultipleBreakages => _difficultyFactorFromMultipleBreakages;
        public float FullEnergyDaySpeedMultiplier => _fullEnergyDaySpeedMultiplier;
        public float AllGoodLoosePointRegainSec => _allGoodLoosePointRegainSec;
        public float LoosePointsSec => _loosePointsSec;
        
        public float FullEnergyScoreMultiplier => _fullEnergyScoreMultiplier;
        public float PowerIncreaseScoreMultiplier => _powerIncreaseScoreMultiplier;
        public float PowerDecreaseScoreMultiplier => _powerDecreaseScoreMultiplier;

        // --- Puzzle ---
        public float TimeToMaxPuzzleCountDifficulty => _timeToMaxPuzzleCountDifficulty;
        public int PuzzleMinNumber => _puzzleMinNumber;
        public int PuzzleMaxNumber => _puzzleMaxNumber;
        public float TimeToMaxPuzzleRepetionDifficulty => _timeToMaxPuzzleRepetionDifficulty;
        public int PuzzleStartRepetion => _puzzleStartRepetion;
        public float PuzzleEndRepetion => _puzzleEndRepetion;
    }
}