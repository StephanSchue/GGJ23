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
        [SerializeField] private int _allGoodRewardSec = 1;
        [SerializeField] private int _loosePointsSec = 1;


        [Header("Design Tuning")]
        [Range(0f, 900f)] [SerializeField] private float _lossPointsNeeded = 100;
        [Range(5f, 30f)] [SerializeField] private float _maxDifficulty = 15f;
        [Range(2.5f, 20f)] [SerializeField] private float _timeToMaxDifficulty = 10f;
        [Range(1f, 50f)] [SerializeField] private float _maxDifficultyFromDistance = 15f;
        [Range(200f, 2000f)] [SerializeField] private float _distanceToMaxDifficulty = 500f;
        [Range(0.1f, 1.5f)] [SerializeField] private float _difficultyFactorFromMultipleBreakages = 0.235f;

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
        public float AllGoodRewardSec => _allGoodRewardSec;
        public float LoosePointsSec => _loosePointsSec;


    }
}