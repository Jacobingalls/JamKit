#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace info.jacobingalls.jamkit
{
    [CustomEditor(typeof(PubSubSender))]
    public class PubSubSenderEditor : Editor
    {
        private bool _debug;
        private string _eventName;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _debug = EditorGUILayout.Toggle("Debug", _debug);
            if (!_debug) { return; }

            EditorGUILayout.TextField("Event Name", _eventName);

            EditorGUI.BeginDisabledGroup(Application.isEditor);
            var pubSubSender = target as PubSubSender;
            if (GUILayout.Button("Send"))
            {
                pubSubSender.Publish(_eventName);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}

#endif