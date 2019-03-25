using System;
using UnityEngine;

namespace BestHTTP.Examples
{
    public static class GUIHelper
    {
        public static string BaseURL = "https://besthttpdemosite.azurewebsites.net";

        private static GUIStyle centerAlignedLabel;
        private static GUIStyle rightAlignedLabel;

        public static Rect ClientArea;

        private static void Setup()
        {
            // These has to be called from OnGUI
            if (centerAlignedLabel == null)
            {
                centerAlignedLabel = new GUIStyle(GUI.skin.label);
                centerAlignedLabel.alignment = TextAnchor.MiddleCenter;

                rightAlignedLabel = new GUIStyle(GUI.skin.label);
                rightAlignedLabel.alignment = TextAnchor.MiddleRight;
            }
        }

        public static void DrawArea(Rect area, bool drawHeader, Action action)
        {
            Setup();

            // Draw background
            GUI.Box(area, string.Empty);
            GUILayout.BeginArea(area);

            if (drawHeader)
            {
                GUIHelper.DrawCenteredText(SampleSelector.SelectedSample.DisplayName);
                GUILayout.Space(5);
            }

            if (action != null)
                action();

            GUILayout.EndArea();
        }

        public static void DrawCenteredText(string msg)
        {
            Setup();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(msg, centerAlignedLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void DrawRow(string key, string value)
        {
            Setup();

            GUILayout.BeginHorizontal();
            GUILayout.Label(key);
            GUILayout.FlexibleSpace();
            GUILayout.Label(value, rightAlignedLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class GUIMessageList
    {
        System.Collections.Generic.List<string> messages = new System.Collections.Generic.List<string>();
        Vector2 scrollPos;

        public void Draw()
        {
            Draw(Screen.width, 0);
        }

        public void Draw(float minWidth, float minHeight)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.MinHeight(minHeight));
            for (int i = 0; i < messages.Count; ++i)
                GUILayout.Label(messages[i], GUILayout.MinWidth(minWidth));
            GUILayout.EndScrollView();
        }

        public void Add(string msg)
        {
            messages.Add(msg);
            scrollPos = new Vector2(scrollPos.x, float.MaxValue);
        }

        public void Clear()
        {
            messages.Clear();
        }
    }
}