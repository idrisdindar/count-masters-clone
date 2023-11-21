using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdrisDindar.HyperCasual.Managers
{
    public class SaveManager : Manager
    {
        public SaveData SaveData { get; private set; }
        
        private static string SavePath => Path.Combine(Application.persistentDataPath, "gamedata.data");

        private void Awake()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            if (File.Exists(SavePath))
            {
                LoadChanges();
            }
            else
            {
                SaveData = new SaveData();
            }

            IsInitialized = true;
        }

        private void SaveChanges()
        {
            string saveData = JsonUtility.ToJson(SaveData, true);
            System.IO.File.WriteAllText(SavePath, saveData);
        }

        private void LoadChanges()
        {
            string fileContents = System.IO.File.ReadAllText(SavePath);
            SaveData = JsonUtility.FromJson<SaveData>(fileContents);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus)
                SaveChanges();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus)
                SaveChanges();
        }

        private void OnApplicationQuit()
        {
            SaveChanges();
        }
    }
}