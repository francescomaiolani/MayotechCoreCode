using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AutoConnect))]
public class AutoConnectPropertyDrawer : PropertyDrawer
{
	private GUIStyle _style;

	private GUIStyle style => _style ?? (_style = new GUIStyle
	{
		richText = true,
		fontStyle = FontStyle.Bold
	});
	
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.objectReferenceValue == null)
		{
			var type = fieldInfo.FieldType;

			var myAttribute = attribute as AutoConnect;

			var searchKey = string.IsNullOrEmpty(myAttribute.AssetName)
				? $"t:{type.Name}"
				: $"t:{type.Name} {myAttribute.AssetName}";
			
			Debug.Log("Search key"  + searchKey);

			if (property.objectReferenceValue != null)
			{
				GUI.enabled = false;
				EditorGUI.PropertyField(position, property, label);
				GUI.enabled = true;
				return;
			}
			
			var guids = AssetDatabase.FindAssets(searchKey);

			if (guids.Length == 0)
			{
				EditorGUI.LabelField(position, $"<color=red>Can't find any {type.Name}</color>", style);
				return;
			}

			if (guids.Length > 1)
			{
				EditorGUI.LabelField(position, $"<color=red>Found multiple assets of type {type.Name}. This is not allowed</color>", style);
				return;
			}
			
			property.serializedObject.Update();
			property.objectReferenceValue = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), type);
			property.serializedObject.ApplyModifiedProperties();
		}

		GUI.enabled = false;

		EditorGUI.PropertyField(position, property, label);
		
		GUI.enabled = true;
	}
}
