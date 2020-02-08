﻿using UnityEditor;
using UnityEngine;

/// <summary>
/// Draws the generic dictionary a bit nicer than Unity would natively (not as many expand-arrows etc.).
/// </summary>
[CustomPropertyDrawer(typeof(GenericDictionary<,>))]
public class GenericDictionaryPropertyDrawer : PropertyDrawer
{
    static float lineHeight = EditorGUIUtility.singleLineHeight;
    static float vertSpace = EditorGUIUtility.standardVerticalSpacing;
    static float combined = lineHeight + vertSpace;

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        // render field name and expand arrow
        var list = property.FindPropertyRelative("list");
        var headerPos = new Rect(lineHeight, pos.y, pos.width, lineHeight);
        EditorGUI.PropertyField(headerPos, list, new GUIContent(fieldInfo.Name));

        if (list.isExpanded)
        {
            // render list size
            list.NextVisible(true);
            var newPos = new Rect(headerPos.x + combined, headerPos.y + combined, 1f, lineHeight);
            newPos = new Rect(headerPos.x, headerPos.y + combined, pos.width, lineHeight);
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(newPos, list, new GUIContent("Size"));

            // render list content
            newPos.y += vertSpace;
            while (list.depth >= 2)
            {
                if (list.name == "Key" || list.name == "Value")
                {
                    // render key or value
                    var entryPos = new Rect(newPos.x, newPos.y + combined, pos.width, lineHeight);
                    EditorGUI.PropertyField(entryPos, list, new GUIContent(list.name));
                    newPos.y += combined;
                    
                    // add spacing after each pair has rendered
                    if (list.name == "Value")
                    {
                        newPos.y += vertSpace;
                    }
                }
                if (!list.NextVisible(true)) break;
            }
        }
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Return height of list (respect if list is expanded or unexpanded)
        var listProp = property.FindPropertyRelative("list");
        if (listProp.isExpanded)
        {
            listProp.NextVisible(true);
            int listSize = listProp.intValue;
            float totHeight = listSize * 2f * combined + combined * 2f + listSize * vertSpace;
            return totHeight;
        }
        else
        {
            return lineHeight;
        }
    }
}