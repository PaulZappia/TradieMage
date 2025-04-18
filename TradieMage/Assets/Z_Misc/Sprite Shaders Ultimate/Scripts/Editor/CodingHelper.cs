#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace SpriteShadersUltimate
{
    public class CodingHelper : EditorWindow
    {
        public GUIContent labelContent;
        public MaterialProperty prop;

        public static CodingHelper lastWindow;
        bool isImage;

        void OnGUI()
        {
            //Close:
            if(labelContent == null || prop == null)
            {
                Close();
                return;
            }

            EditorGUILayout.BeginVertical();

            //Style:
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.richText = true;

            //Internal Name:
            GUI.color = new Color(1, 1, 1, 0.7f);
            EditorGUILayout.LabelField("<b><size=14>Property Name:</size></b>", labelStyle);
            GUI.color = Color.white;
            DisplayCode("<b>" + prop.name + "</b>", labelStyle);
            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.Space(); EditorGUILayout.Space();

            //Code:
            GUI.color = new Color(1, 1, 1, 0.7f);
            EditorGUILayout.LabelField("<b><size=14>Set Function:</size></b>", labelStyle);
            GUI.color = Color.white;

            string codeText = default;
            string propertyText = default;
            if (prop.type == MaterialProperty.PropType.Color)
            {
                propertyText = "public Color colorValue;";
                codeText = ".SetColor(<b>\"" + prop.name + "\"</b>, colorValue);";
            }
            else if (prop.type == MaterialProperty.PropType.Vector)
            {
                propertyText = "public Vector2 vectorValue;";
                codeText = ".SetVector(<b>\"" + prop.name + "\"</b>, vectorValue);";
            }
            else if (prop.type == MaterialProperty.PropType.Texture)
            {
                propertyText = "public Texture textureValue;";
                codeText = ".SetTexture(<b>\"" + prop.name + "\"</b>, textureValue);";
            }
            else
            {
                propertyText = "public float floatValue;";
                codeText = ".SetFloat(<b>\"" + prop.name + "\"</b>, floatValue);";
            }

            DisplayCode("material" + codeText, labelStyle);

            //Example:
            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.Space(); EditorGUILayout.Space();
            GUI.color = new Color(1, 1, 1, 0.7f);
            EditorGUILayout.LabelField("<b><size=14>Example Code:</size></b>", labelStyle);
            GUI.color = Color.white;

            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.x += lastRect.width - 160;
            lastRect.width = 100;
            if (!isImage)
            {
                GUI.enabled = false;
            }
            if (GUI.Button(lastRect, "Sprite Renderer"))
            {
                isImage = false;
                GUI.FocusControl(null);
                Repaint();
            }
            if(isImage)
            {
                GUI.enabled = false;
            }
            else
            {
                GUI.enabled = true;
            }
            lastRect.x += 100;
            lastRect.width = 60;
            if (GUI.Button(lastRect, "UI Image"))
            {
                isImage = true;
                GUI.FocusControl(null);
                Repaint();
            }
            GUI.enabled = true;

            string exampleText = default;
            if(isImage)
            {
                exampleText = @"using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    " + propertyText + @"

    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.material = Instantiate(image.material);
    }

    void Update()
    {
        image.materialForRendering" + codeText + @"
    }
}";
            }
            else
            {
                exampleText = @"using UnityEngine;

public class Example : MonoBehaviour
{
    " + propertyText + @"

    Material material;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    void Update()
    {
        material" + codeText + @"
    }
}";
            }

            DisplayCode(exampleText, labelStyle);

            //Final:
            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.Space(); EditorGUILayout.Space();
            EditorGUILayout.Space(); EditorGUILayout.Space();

            GUI.color = new Color(1, 1, 1, 0.7f);
            EditorGUILayout.LabelField("Do you need <b>more</b> help ?", labelStyle);
            EditorGUILayout.LabelField("Check out the <b>documentation</b> or <b>contact</b> me.", labelStyle);
            GUI.color = Color.white;
            EditorGUILayout.Space();
            SSUShaderGUI.DisplaySupportInformation();

            EditorGUILayout.EndVertical();

            Rect contentRect = GUILayoutUtility.GetLastRect();
            if(Mathf.Abs(position.height - contentRect.height - 50) > 5 && contentRect.height > 400)
            {
                Rect newPosition = new Rect(position);
                newPosition.height = contentRect.height + 50;
                position = newPosition;
            }

            Rect closeRect = new Rect(position);
            closeRect.width = 60;
            closeRect.height = 30;
            closeRect.x = position.width - 70;
            closeRect.y = position.height - 40;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.richText = true;
            if(GUI.Button(closeRect, "<size=16>Close</size>", buttonStyle))
            {
                Close();
            }
        }

        void DisplayCode(string codeText, GUIStyle labelStyle)
        {
            EditorGUILayout.BeginVertical("Helpbox");

            int lines = codeText.Split('\n').Length;

            EditorGUILayout.SelectableLabel(codeText, labelStyle, GUILayout.Height(lines * 16));
            EditorGUILayout.EndVertical();

            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.x += lastRect.width - 115;
            lastRect.width = 115;
            lastRect.y += lastRect.height - 1;
            lastRect.height = 20;
            if (GUI.Button(lastRect, "Copy to Clipboard"))
            {
                EditorGUIUtility.systemCopyBuffer = codeText.Replace("<b>","").Replace("</b>","");
            }
        }

        public static void Open(GUIContent labelContent, MaterialProperty prop, Shader shader, float width)
        {
            if(lastWindow != null)
            {
                lastWindow.Close();
                lastWindow = null;
            }

            CodingHelper window = CreateInstance(typeof(CodingHelper)) as CodingHelper;
            window.ShowUtility();
            window.labelContent = labelContent;
            window.prop = prop;
            window.titleContent = new GUIContent("Coding Hints - " + labelContent.text);
            window.isImage = (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Graphic>() != null) || shader.name.Contains("UI");

            Vector2 position = new Vector2(window.position.x, window.position.y);
            if(Event.current != null)
            {
                position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                position.x -= 500 + width;
                position.y -= 300;
            }

            window.position = new Rect(position.x, position.y, 500, 665);

            lastWindow = window;
        }
    }

}

#endif