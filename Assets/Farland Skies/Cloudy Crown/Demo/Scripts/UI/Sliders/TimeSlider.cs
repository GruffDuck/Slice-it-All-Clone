using UnityEngine;
using UnityEngine.UI;

namespace Borodar.FarlandSkies.CloudyCrownPro
{
    public class TimeSlider : MonoBehaviour
    {
        private Slider _slider;
        private SkyboxCycleManager _skyboxCycleManager;

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        protected void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        protected void Start()
        {
            _skyboxCycleManager = SkyboxCycleManager.Instance;
        }

        protected void Update()
        {
            _slider.value = _skyboxCycleManager.CycleProgress;
        }

        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------

        public void OnValueChanged(float value)
        {
            _skyboxCycleManager.CycleProgress = value;
        }
    }
}