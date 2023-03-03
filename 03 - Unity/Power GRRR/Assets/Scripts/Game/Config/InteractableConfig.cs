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
        private int _puzzleNumberCount = 4;
        [SerializeField]
        private int _puzzleNumberApperence = 2;
        [SerializeField]
        private int _puzzleTriesBeforeRegenerate = 3;

        public float Duration => _duration;
        public float InteractionRadius => _interactionRadius;
        public int PuzzleNumberCount => _puzzleNumberCount;
        public int PuzzleNumberApperence => _puzzleNumberApperence;
        public int PuzzleTriesBeforeRegenerate => _puzzleTriesBeforeRegenerate;


    }
}