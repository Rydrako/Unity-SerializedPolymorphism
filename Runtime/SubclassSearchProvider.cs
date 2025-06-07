using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Rydrako.SerializedPolymorphism
{
    public class SubclassSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        Type baseType;
        Dictionary<string, Type> typeMap;
        Action<Type> onSetIndexCallback;

        public void Init(Type baseType, Dictionary<string, Type> typeMap, Action<Type> callback)
        {
            this.baseType = baseType;
            this.typeMap = typeMap;
            onSetIndexCallback = callback;
        }

        public static SubclassSearchProvider CreateInstance(Type baseType, Dictionary<string, Type> typeMap, Action<Type> callback)
        {
            var data = ScriptableObject.CreateInstance<SubclassSearchProvider>();
            data.Init(baseType, typeMap, callback);
            return data;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new()
            {
                new SearchTreeGroupEntry(new GUIContent(ObjectNames.NicifyVariableName(baseType.Name)), 0)
            };

            List<string> groups = new();
            foreach (var kvp in typeMap)
            {
                var name = kvp.Key;
                var type = kvp.Value;

                AddSubclassMenuAttribute attribute = type.GetCustomAttribute<AddSubclassMenuAttribute>();

                var path = attribute != null ? attribute.Path : name;

                string[] entryTitle = path.Split("/");
                string groupName = "";
                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last(), ScriptIconUtil.GetScriptIcon(type)));
                entry.level = entryTitle.Length;
                entry.userData = type;
                list.Add(entry);
            }

            return list;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            onSetIndexCallback?.Invoke((Type)SearchTreeEntry.userData);
            return true;
        }
    }
}