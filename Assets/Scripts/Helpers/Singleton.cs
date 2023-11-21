using System;
using System.Collections.Generic;
using UnityEngine;
using IdrisDindar.HyperCasual.Managers;

namespace IdrisDindar.HyperCasual
{
    
    /// <summary>
    /// Singleton class is a service locator that provides access to important game services.
    /// Encapsulates an instance of the Singleton class and references to other managers.
    /// Ensures than only one instance of Singleton class is used throughout the entire application.
    /// </summary>
    public class Singleton : MonoBehaviour
    {
        public static Singleton Instance { get; private set; }
        public InputManager InputManager { get; private set; }
        public GameManager GameManager { get; private set; }
        
        public SaveManager SaveManager { get; private set; }
        
        public LevelManager LevelManager { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            
            DontDestroyOnLoad(this);
            InputManager = GetComponentInChildren<InputManager>();
            GameManager = GetComponentInChildren<GameManager>();
            SaveManager = GetComponentInChildren<SaveManager>();
            LevelManager = GetComponentInChildren<LevelManager>();
        }
    }
}