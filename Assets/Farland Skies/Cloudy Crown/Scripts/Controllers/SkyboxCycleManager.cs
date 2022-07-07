using Borodar.FarlandSkies.Core.Helpers;
using UnityEngine;

namespace Borodar.FarlandSkies.CloudyCrownPro
{
    [HelpURL("http://www.borodar.com/stuff/farlandskies/cloudycrownpro/docs/QuickStart.v1.3.2.pdf")]
    public class SkyboxCycleManager : Singleton<SkyboxCycleManager>
    {
        [Tooltip("Day-night cycle duration from 0% to 100% (in seconds)")]
        public float CycleDuration = 10f;

        [Tooltip("Current time of day (in percents)")]
        public float CycleProgress;

        public bool Paused;

        private SkyboxDayNightCycle _dayNightCycle;

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        protected void Start()
        {
            _dayNightCycle = SkyboxDayNightCycle.Instance;
        }

        protected void Update()
        {
            if (!Paused)
            {
                CycleProgress += (Time.deltaTime / CycleDuration) * 100f;
                CycleProgress %= 100f;
            }

            if (_dayNightCycle != null)
                _dayNightCycle.TimeOfDay = CycleProgress;
        }
    }
}