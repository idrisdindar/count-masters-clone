using System;
using System.Linq;
using IdrisDindar.HyperCasual.Managers;
using UnityEngine;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual
{
    public class GateHolder : MonoBehaviour
    {
        private Gate[] _gates;

        private Delegate _gateTriggeredHandler;

        private void Awake()
        {
            _gates = GetComponentsInChildren<Gate>();
            _gateTriggeredHandler = new Action<Gate>(OnGateTriggered);
        }

        private void OnEnable()
        {
            EventManager.RegisterEvent(Event.TriggerGate, _gateTriggeredHandler);
        }

        private void OnDisable()
        {
            EventManager.UnregisterEvent(Event.TriggerGate, _gateTriggeredHandler);
        }

        private void Start()
        {
            InitializeGates();
        }

        private void OnGateTriggered(Gate gate)
        {
            if (! _gates.Any(myGate => myGate == gate))
                return;

            foreach (var myGate in _gates)
            {
                myGate.CloseGate();
            }
        }

        private void InitializeGates()
        {
            foreach (var gate in _gates)
            {
                gate.Initialize();
            }
            
            if (_gates.Any(gate => gate.Type == GateType.Positive))
                return;

            do
            {
                foreach (var gate in _gates)
                {
                    gate.Initialize();
                }
            } while (_gates.All(gate => gate.Type == GateType.Negative));
        }
    }
}