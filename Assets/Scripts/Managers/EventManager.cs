using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdrisDindar.HyperCasual.Managers
{
    public enum Event
    {
        // Game Events
        GameStatusChanged,
        LevelCreated,
        StartLevel,
        CompleteLevel,
        GoToNextLevel,
        RetryLevel,
        FailLevel,
        TriggerGate,
        KillPlayerMinion,
        KillEnemyMinion,
        EncounterEnemy,
        DefeatEnemy,
        
        // Input Events
        SetDeltaPosition,
        CancelMovement
    }
    
    public class EventManager
    {
        private static readonly Dictionary<Event, Delegate> _eventDictionary = new();

        public static void RegisterEvent(Event eventType, Delegate eventHandler)
        {
            if (!_eventDictionary.ContainsKey(eventType))
            {
                _eventDictionary[eventType] = eventHandler;
            }
            else
            {
                Delegate existingDelegate = _eventDictionary[eventType];
                _eventDictionary[eventType] = Delegate.Combine(existingDelegate, eventHandler);
            }
        }

        public static void UnregisterEvent(Event eventType, Delegate eventHandler)
        {
            if (_eventDictionary.TryGetValue(eventType, out var existingHandler))
            {
                Delegate newHandler = Delegate.Remove(existingHandler, eventHandler);
                if (newHandler == null)
                {
                    _eventDictionary.Remove(eventType);
                }
                else
                {
                    _eventDictionary[eventType] = newHandler;
                }
            }
        }

        public static void TriggerEvent(Event eventType, params object[] args)
        {
            if (_eventDictionary.TryGetValue(eventType, out var handler))
            {
                handler.DynamicInvoke(args);
            }
        }
    }
}