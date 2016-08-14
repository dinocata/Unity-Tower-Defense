using UnityEngine;
using System.Collections;
using UnityEditor ;

using UnityEngine.UI;

[ CustomEditor ( typeof ( Track_Deform_CS ) ) ]

public class Track_Deform_CSEditor : Editor {

	SerializedProperty anchorNumProp ;
	SerializedProperty anchorArrayProp ;
	SerializedProperty widthArrayProp ;
	SerializedProperty heightArrayProp;

	void OnEnable () {
		anchorNumProp = serializedObject.FindProperty ( "anchorNum" ) ;
		anchorArrayProp = serializedObject.FindProperty ( "anchorArray" ) ;
		widthArrayProp = serializedObject.FindProperty ( "widthArray" ) ;
		heightArrayProp = serializedObject.FindProperty ( "heightArray" ) ;
	}

	public override void OnInspectorGUI () {
		if ( EditorApplication.isPlaying == false ) {
			GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
			serializedObject.Update () ;
			EditorGUILayout.Space () ;

			EditorGUILayout.IntSlider ( anchorNumProp , 1 , 64 , "Number of Anchor Wheels" ) ;
			EditorGUILayout.Space () ;

			anchorArrayProp.arraySize = anchorNumProp.intValue ;
			widthArrayProp.arraySize = anchorNumProp.intValue ;
			heightArrayProp.arraySize = anchorNumProp.intValue ;
			for ( int i = 0 ; i < anchorArrayProp.arraySize ; i++ ) {
				anchorArrayProp.GetArrayElementAtIndex ( i ).objectReferenceValue = EditorGUILayout.ObjectField ( "Anchor Wheel" , anchorArrayProp.GetArrayElementAtIndex ( i ).objectReferenceValue , typeof ( Transform ) , true ) ;
				EditorGUILayout.Slider ( widthArrayProp.GetArrayElementAtIndex ( i ) , 0.0f , 10.0f , "Weight Width" ) ;
				EditorGUILayout.Slider ( heightArrayProp.GetArrayElementAtIndex ( i ) , 0.0f , 10.0f , "Weight Height" ) ;
				EditorGUILayout.Space () ;
			}
			serializedObject.ApplyModifiedProperties () ;
		}
	}

}
