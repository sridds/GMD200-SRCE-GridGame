using UnityEngine;
using UnityEditor;

/// <summary>
/// Followed a code monkey tutorial for this custom editor script
/// https://www.youtube.com/watch?v=E91NYvDqsy8&ab_channel=CodeMonkey
/// </summary>
[CustomEditor(typeof(RecipeSO))]
public class RecipeSOEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RecipeSO recipeSO = (RecipeSO)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("OUTPUT", new GUIStyle { fontStyle = FontStyle.Bold });
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();

        Texture texture = null;
        if (recipeSO.OutputItem != null)
        {
            texture = recipeSO.OutputItem.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OutputItem"), GUIContent.none, true, GUILayout.Width(150));

        GUILayout.Label("Quantity:", new GUIStyle { fontStyle = FontStyle.Bold });
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OutputAmount"), GUIContent.none, true, GUILayout.Width(150));

        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Space();


        GUILayout.Label("RECIPE", new GUIStyle { fontStyle = FontStyle.Bold });
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_02 != null)
        {
            texture = recipeSO.item_02.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_02"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_12 != null)
        {
            texture = recipeSO.item_12.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_12"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_22 != null)
        {
            texture = recipeSO.item_22.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_22"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_01 != null)
        {
            texture = recipeSO.item_01.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_01"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_11 != null)
        {
            texture = recipeSO.item_11.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_11"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_21 != null)
        {
            texture = recipeSO.item_21.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_21"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();




        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_00 != null)
        {
            texture = recipeSO.item_00.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_00"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_10 != null)
        {
            texture = recipeSO.item_10.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_10"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeSO.item_20 != null)
        {
            texture = recipeSO.item_20.ItemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_20"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }*/
}
