using UnityEngine;

namespace GGJ23.Game.Config
{
    [CreateAssetMenu(fileName = "InteractableConfig_Default", menuName = "ScriptableObjects/InteractableConfig", order = 1)]
    public class InteractableConfig : ScriptableObject
    {
        [SerializeField]
        private float _duration = 2f;
        [SerializeField]
        private float _interactionRadius = 1f;
        [SerializeField]
        private int _brokenOnStartPuzzleNumberCount = 4;
        [SerializeField]
        private int _broekenOnStartPuzzleNumberApperence = 2;
        [SerializeField]
        private int _puzzleTriesBeforeRegenerate = 3;
        [SerializeField]
        private float _puzzleSolveEnergyRestore = 10;
        
        public float Duration => _duration;
        public float InteractionRadius => _interactionRadius;
        public int BrokenOnStartPuzzleNumberCount => _brokenOnStartPuzzleNumberCount;
        public int BrokenOnStartPuzzleNumberApperence => _broekenOnStartPuzzleNumberApperence;
        public int PuzzleTriesBeforeRegenerate => _puzzleTriesBeforeRegenerate;
        public float PuzzleSolveEnergyRestore => _puzzleSolveEnergyRestore;
    }
}