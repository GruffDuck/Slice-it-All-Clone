using Borodar.FarlandSkies.Core.DotParams;
using Borodar.FarlandSkies.Core.Editor;
using Borodar.FarlandSkies.Core.ReorderableList;
using UnityEditor;
using UnityEngine;

namespace Borodar.FarlandSkies.CloudyCrownPro
{
    [CustomEditor(typeof(SkyboxDayNightCycle))]
    public class SkyboxDayNightCycleEditor : Editor
    {
        private const float LIST_CONTROLS_PAD = 20f;
        private const float TIME_WIDTH = BaseParamDrawer.TIME_FIELD_WIDHT + LIST_CONTROLS_PAD;

        // Sky
        private SerializedProperty _skyDotParams;
        // Stars
        private SerializedProperty _starsDotParams;
        // Sun
        private SerializedProperty _sunrise;
        private SerializedProperty _sunset;
        private SerializedProperty _sunAltitude;
        private SerializedProperty _sunLongitude;
        private SerializedProperty _sunOrbit;
        private SerializedProperty _sunDotParams;
        // Moon
        private SerializedProperty _moonrise;
        private SerializedProperty _moonset;
        private SerializedProperty _moonAltitude;
        private SerializedProperty _moonLongitude;
        private SerializedProperty _moonOrbit;
        private SerializedProperty _moonDotParams;
        // General
        private SerializedProperty _framesInterval;

        // Icons
        private GUIContent _skyIcon;
        private GUIContent _starsIcon;
        private GUIContent _sunIcon;
        private GUIContent _moonIcon;
        private GUIContent _generalIcon;

        private static bool _showSkyDotParams;
        private static bool _showStarsDotParams;
        private static bool _showSunDotParams;
        private static bool _showMoonDotParams;

        private GUIContent _guiContent;
        private GUIContent _skyParamsLabel;
        private GUIContent _starsParamsLabel;
        private GUIContent _sunParamsLabel;
        private GUIContent _moonParamsLabel;
        private GUIContent _framesIntervalLabel;

        protected void OnEnable()
        {
            _guiContent = new GUIContent();
            _skyParamsLabel = new GUIContent("Sky Dot Params", "List of sky colors, based on time of day. Each list item contains “time” filed that should be specified in percents (0 - 100)");
            _starsParamsLabel = new GUIContent("Stars Dot Params", "Allows you to manage stars tint color over time. Each list item contains “time” filed that should be specified in percents (0 - 100)");
            _sunParamsLabel = new GUIContent("Sun Dot Params", "Sun appearance and light params depending on time of day. Each list item contains “time” filed that should be specified in percents (0 - 100)");
            _moonParamsLabel = new GUIContent("Moon Dot Params", "Moon appearance and light params depending on time of day. Each list item contains “time” filed that should be specified in percents (0 - 100)");
            _framesIntervalLabel = new GUIContent("Frames Interval", "Reduce the skybox day-night cycle update to run every \"n\" frames");

            // Sky
            _skyDotParams = serializedObject.FindProperty("_skyParamsList").FindPropertyRelative("Params");
            // Stars
            _starsDotParams = serializedObject.FindProperty("_starsParamsList").FindPropertyRelative("Params");
            // Sun
            _sunrise = serializedObject.FindProperty("_sunrise");
            _sunset = serializedObject.FindProperty("_sunset");
            _sunAltitude = serializedObject.FindProperty("_sunAltitude");
            _sunLongitude = serializedObject.FindProperty("_sunLongitude");
            _sunOrbit = serializedObject.FindProperty("_sunOrbit");
            _sunDotParams = serializedObject.FindProperty("_sunParamsList").FindPropertyRelative("Params");
            // Moon
            _moonrise = serializedObject.FindProperty("_moonrise");
            _moonset = serializedObject.FindProperty("_moonset");
            _moonAltitude = serializedObject.FindProperty("_moonAltitude");
            _moonLongitude = serializedObject.FindProperty("_moonLongitude");
            _moonOrbit = serializedObject.FindProperty("_moonOrbit");
            _moonDotParams = serializedObject.FindProperty("_moonParamsList").FindPropertyRelative("Params");
            // General
            _framesInterval = serializedObject.FindProperty("_framesInterval");
            
            // Icons
            var skinFolder = (EditorGUIUtility.isProSkin) ? "Professional/" : "Personal/";
            
            var skyTex = FarlandSkiesEditorUtility.LoadEditorIcon(skinFolder + "Sky_16.png");
            var starsTex = FarlandSkiesEditorUtility.LoadEditorIcon(skinFolder + "Star_16.png");
            var sunTex = FarlandSkiesEditorUtility.LoadEditorIcon(skinFolder + "Sun_16.png");
            var moonTex = FarlandSkiesEditorUtility.LoadEditorIcon(skinFolder + "Moon_16.png");
            var generalTex = FarlandSkiesEditorUtility.LoadEditorIcon(skinFolder + "Tag_16.png");
            
            _skyIcon = new GUIContent(skyTex);
            _starsIcon = new GUIContent(starsTex);
            _sunIcon = new GUIContent(sunTex);
            _moonIcon = new GUIContent(moonTex);
            _generalIcon = new GUIContent(generalTex);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CustomGUILayout();
            serializedObject.ApplyModifiedProperties();
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private void CustomGUILayout()
        {
            var skyboxController = SkyboxController.Instance;
            if (skyboxController == null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("This component requires the SkyboxController instance to be present in the scene. Please add the SkyboxController prefab to your scene.", MessageType.Error);
                return;
            }

            // Rate dialog          
            RateMeDialog.DrawRateDialog(AssetInfo.ASSET_NAME, AssetInfo.ASSET_STORE_ID);
            EditorGUILayout.Space();

            SkyGUILayout();
            StarsGUILayout(skyboxController.StarsEnabled);
            SunGUILayout(skyboxController.SunEnabled);
            MoonGUILayout(skyboxController.MoonEnabled);
            GeneralGUILayout();
        }

        // Sections
        
        private void GeneralGUILayout()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(_generalIcon, GUILayout.Width(16f));
            EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            
            _framesInterval.intValue = EditorGUILayout.IntSlider(_framesIntervalLabel, _framesInterval.intValue, 1, 60);
        }

        private void SkyGUILayout()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(_skyIcon, GUILayout.Width(16f));
            EditorGUILayout.LabelField("Sky", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            _showSkyDotParams = EditorGUILayout.Foldout(_showSkyDotParams, _skyParamsLabel);
            EditorGUILayout.Space();
            if (_showSkyDotParams)
            {
                SkyParamsHeader();
                ReorderableListGUI.ListField(_skyDotParams);
            }
        }

        private void StarsGUILayout(bool starsEnabled)
        {
            GUI.enabled = starsEnabled;

            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(_starsIcon, GUILayout.Width(16f));
            EditorGUILayout.LabelField("Stars", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            if (starsEnabled)
            {
                _showStarsDotParams = EditorGUILayout.Foldout(_showStarsDotParams, _starsParamsLabel);
                EditorGUILayout.Space();
                if (_showStarsDotParams)
                {
                    StarsParamsHeader();
                    ReorderableListGUI.ListField(_starsDotParams);
                }
            }
            else
            {
                EditorGUILayout.Space();
            }
        }

        private void SunGUILayout(bool sunEnabled)
        {
            GUI.enabled = sunEnabled;

            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(_sunIcon, GUILayout.Width(16f));
            EditorGUILayout.LabelField("Sun", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            if (sunEnabled)
            {
                EditorGUILayout.PropertyField(_sunrise);
                EditorGUILayout.PropertyField(_sunset);
                EditorGUILayout.PropertyField(_sunAltitude);
                EditorGUILayout.PropertyField(_sunLongitude);
                EditorGUILayout.PropertyField(_sunOrbit);

                _showSunDotParams = EditorGUILayout.Foldout(_showSunDotParams, _sunParamsLabel);
                EditorGUILayout.Space();
                if (_showSunDotParams)
                {
                    CelestialsParamsHeader();
                    ReorderableListGUI.ListField(_sunDotParams);
                }
            }
            else
            {
                EditorGUILayout.Space();
            }
        }

        private void MoonGUILayout(bool moonEnabled)
        {
            GUI.enabled = moonEnabled;

            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField(_moonIcon, GUILayout.Width(16f));
            EditorGUILayout.LabelField("Moon", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            if (moonEnabled)
            {
                EditorGUILayout.PropertyField(_moonrise);
                EditorGUILayout.PropertyField(_moonset);
                EditorGUILayout.PropertyField(_moonAltitude);
                EditorGUILayout.PropertyField(_moonLongitude);
                EditorGUILayout.PropertyField(_moonOrbit);

                _showMoonDotParams = EditorGUILayout.Foldout(_showMoonDotParams, _moonParamsLabel);
                EditorGUILayout.Space();
                if (_showMoonDotParams)
                {
                    CelestialsParamsHeader();
                    ReorderableListGUI.ListField(_moonDotParams);
                }
            }
            else
            {
                EditorGUILayout.Space();
            }
        }

        // Headers

        private void SkyParamsHeader()
        {
            var position = GUILayoutUtility.GetRect(_guiContent, ReorderableListStyles.Title);
            if (Event.current.type == EventType.Repaint)
            {
                var baseWidht = position.width;
                // Time
                position.width = TIME_WIDTH;
                _guiContent.text = "Time";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
                // Top Color
                position.x += position.width;
                position.width = (baseWidht - position.width - 2f * LIST_CONTROLS_PAD) / 2f + BaseParamDrawer.H_PAD;
                _guiContent.text = "Top Color";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
                // Bottom Color
                position.x += position.width;
                position.width += LIST_CONTROLS_PAD;
                _guiContent.text = "Bottom Color";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
            }
            GUILayout.Space(-1);
        }

        private void StarsParamsHeader()
        {
            var position = GUILayoutUtility.GetRect(_guiContent, ReorderableListStyles.Title);
            if (Event.current.type == EventType.Repaint)
            {
                var baseWidht = position.width;
                // Time
                position.width = TIME_WIDTH;
                _guiContent.text = "Time";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
                // Tint Color
                position.x += position.width;
                position.width = baseWidht - position.width;
                _guiContent.text = "Tint Color";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
            }
            GUILayout.Space(-1);
        }

        private void CelestialsParamsHeader()
        {
            //Draw custom title for reorderable list
            var position = GUILayoutUtility.GetRect(_guiContent, ReorderableListStyles.Title);
            if (Event.current.type == EventType.Repaint)
            {
                var baseWidht = position.width;
                var baseHeight = position.height;
                // Time
                position.width = TIME_WIDTH;
                position.height *= 2f;
                _guiContent.text = "Time";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
                // Tint Color
                position.x += position.width;
                position.width = (baseWidht - position.width - 2f * LIST_CONTROLS_PAD) / 2f + BaseParamDrawer.H_PAD;
                position.height = baseHeight;
                _guiContent.text = "Tint Color";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
                // Light Color
                position.x += position.width;
                position.width += LIST_CONTROLS_PAD;
                _guiContent.text = "Light Color";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
            }
            GUILayout.Space(-5f);
            position = GUILayoutUtility.GetRect(_guiContent, ReorderableListStyles.Title);
            if (Event.current.type == EventType.Repaint)
            {
                // Light Intencity
                position.x += TIME_WIDTH;
                position.width -= TIME_WIDTH;
                _guiContent.text = "Light Intencity";
                ReorderableListStyles.Title.Draw(position, _guiContent, false, false, false, false);
            }
            GUILayout.Space(-1);
        }
    }
}