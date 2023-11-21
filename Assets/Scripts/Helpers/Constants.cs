using UnityEngine;

namespace IdrisDindar.HyperCasual
{
    public static class Constants
    {
        public const string TAG_PLAYER_MINION = "Minion/Player";
        public const string TAG_ENEMY_MINION = "Minion/Enemy";
        public const string TAG_FINISH = "Finish";
        
        public static readonly int ANIM_SPEED_MULTIPLIER = Animator.StringToHash("SpeedMultiplier");
        public static readonly int ANIM_SPEED = Animator.StringToHash("Speed");
    }
}