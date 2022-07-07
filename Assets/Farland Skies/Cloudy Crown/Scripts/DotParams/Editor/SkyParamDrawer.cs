using Borodar.FarlandSkies.Core.DotParams;
using UnityEditor;
using UnityEngine;

namespace Borodar.FarlandSkies.CloudyCrownPro.DotParams
{
    [CustomPropertyDrawer(typeof(SkyParam))]
    public class SkyParamDrawer : BaseParamDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            // Top Color
            position.x += TIME_FIELD_WIDHT;
            position.y += V_PAD;
            position.width = (position.width - TIME_FIELD_WIDHT) / 2f - 0.5f * H_PAD;
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("TopColor"), GUIContent.none);
            // Bottom Color
            position.x += position.width + H_PAD;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("BottomColor"), GUIContent.none);
        }
    }
}