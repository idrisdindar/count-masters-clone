using System;

namespace IdrisDindar.HyperCasual
{
    [Serializable]
    public class SaveData
    {
        public LevelSaveData Level;
        
        public SaveData()
        {
            Level = new LevelSaveData();
        }
        
        [Serializable]
        public class LevelSaveData
        {
            public int CurrentLevel;

            public LevelSaveData()
            {
                CurrentLevel = 0;
            }
        }
    }
}