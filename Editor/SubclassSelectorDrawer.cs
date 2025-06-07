using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Reflection;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using System.IO;

namespace Rydrako.SerializedPolymorphism.Editor
{
    [CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        Dictionary<string, Type> typeMap;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (typeMap == null) BuildTypeMap(GetBaseTypeFromProperty(fieldInfo.FieldType));

#if UNITY_2021_3_OR_NEWER
            // Override the label text with the ToString() of the managed reference.
            var subclassSelectorAttribute = (SubclassSelectorAttribute)attribute;
            if (!property.hasMultipleDifferentValues)
            {
                object managedReferenceValue = property.managedReferenceValue;
                if (managedReferenceValue != null)
                {
                    label.text = managedReferenceValue.ToString();
                }
            }
#endif

            var labelRect = new Rect(position.x, position.y, position.width * (1 / 3f), EditorGUIUtility.singleLineHeight);
            var typeRect = new Rect(position.x + position.width/3, position.y, position.width * (2 / 3f), EditorGUIUtility.singleLineHeight);
            var contentRect = new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(labelRect, label);

            var typeName = property.managedReferenceFullTypename;
            var displayName = GetShortTypeName(typeName);


            if (EditorGUI.DropdownButton(typeRect, new GUIContent(displayName ?? "<null>", ScriptIconUtil.GetScriptIcon(displayName)), FocusType.Keyboard))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), SubclassSearchProvider.CreateInstance(GetBaseTypeFromProperty(fieldInfo.FieldType), typeMap, t =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(t);
                    property.serializedObject.ApplyModifiedProperties();
                }));
            }

            if (property.managedReferenceValue != null)
            {
                EditorGUI.PropertyField(contentRect, property, GUIContent.none, true);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        void BuildTypeMap (Type baseType)
        {
            typeMap = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm =>
                {
                    try { return asm.GetTypes(); }
                    catch { return Type.EmptyTypes; }
                })
                .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t) && t != baseType)
                .ToDictionary(t => ObjectNames.NicifyVariableName(t.Name), t => t);
        }

        static string GetShortTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName)) return null;
            var parts = fullTypeName.Split(' ');
            return parts.Length > 1 ? parts[1].Split('.').Last() : fullTypeName;
        }

        Type GetBaseTypeFromProperty (Type fieldInfoType)
        {
            if (fieldInfoType.IsArray)
                return fieldInfoType.GetElementType();
            else if (typeof(IEnumerable).IsAssignableFrom(fieldInfoType))
                return fieldInfoType.GetGenericArguments().Single();

            return fieldInfoType;
        }
    }
}


