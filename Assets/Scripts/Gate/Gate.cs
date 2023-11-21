using System;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using IdrisDindar.HyperCasual.Managers;
using TMPro;
using Event = IdrisDindar.HyperCasual.Managers.Event;

namespace IdrisDindar.HyperCasual
{
    public enum GateType
    {
        Positive,
        Negative
    }
    public enum GateOperation
    {
        Sum,
        Subtraction,
        Multiplication
    }
    public class Gate : MonoBehaviour
    {
        [SerializeField]
        private GateDataSO _gateData;

        [SerializeField]
        private TextMeshProUGUI _gateText;

        private bool _isTriggered;
        private Renderer _renderer;
        
        public int Amount { get; private set; }
        public GateType Type { get; private set; }
        public GateOperation Operation { get; private set; }


        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void Initialize()
        {
            RandomizeType();
            RandomizeOperation();
            RandomizeAmount();
            HandleVisuals();
        }

        private void RandomizeType()
        {
            var values = Enum.GetValues(typeof(GateType));
            Random random = new Random();
            this.Type = (GateType)values.GetValue(random.Next(values.Length));
        }

        private void RandomizeOperation()
        {
            var values = Enum.GetValues(typeof(GateOperation)).Cast<GateOperation>();

            if (this.Type == GateType.Negative)
            {
                this.Operation = GateOperation.Subtraction;
            }
            else
            {
                values = values.Except(new[] { GateOperation.Subtraction });
                Random random = new Random();
                this.Operation = values.ElementAt(random.Next(values.Count()));
            }
        }

        private void RandomizeAmount()
        {
            switch (Operation)
            {
                case GateOperation.Sum:
                    Amount = UnityEngine.Random.Range(_gateData.MinSumAmount, _gateData.MaxSumAmount);
                    break;
                case GateOperation.Subtraction:
                    Amount = UnityEngine.Random.Range(_gateData.MinSubtractionAmount, _gateData.MaxSubtractionAmount);
                    break;
                case GateOperation.Multiplication:
                    Amount = UnityEngine.Random.Range(_gateData.MinMultiplicationAmount, _gateData.MaxMultiplicationAmount);
                    break;
            }
        }

        private void HandleVisuals()
        {
            _renderer.material = Type == GateType.Positive ? _gateData.PositiveMaterial : _gateData.NegativeMaterial;
            
            switch (Operation)
            {
                case GateOperation.Sum:
                    _gateText.text = $"+{Amount}";
                    break;
                case GateOperation.Subtraction:
                    _gateText.text = $"-{Amount}";
                    break;
                case GateOperation.Multiplication:
                    _gateText.text = $"x{Amount}";
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER_MINION) && !_isTriggered)
            {
                EventManager.TriggerEvent(Event.TriggerGate, this);
            }
        }

        public void CloseGate()
        {
            _isTriggered = true;
        }
    }
}