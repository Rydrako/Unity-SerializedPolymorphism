using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Rydrako.SerializedPolymorphism
{
    public static class ScriptIconUtil
    {
        public static Texture GetScriptIcon (Type type)
        {
            return GetScriptIcon(type.Name);
        }

        public static Texture GetScriptIcon(string typeName)
        {
            return AssetDatabase.GetCachedIcon(GetAssetPathFor(typeName));
        }

        public static string GetAssetPathFor (Type type)
        {
            return GetAssetPathFor(type.Name);
        }

        private static string GetAssetPathFor (string typeName)
        {
            var asset = "";
            var guids = AssetDatabase.FindAssets(string.Format("{0} t:script", typeName));

            if (guids.Length > 1)
            {
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    var filename = Path.GetFileNameWithoutExtension(assetPath);
                    if (filename == typeName)
                    {
                        asset = guid;
                        break;
                    }
                }
            }
            else if (guids.Length == 1)
            {
                asset = guids[0];
            }
            else
            {
                return null; // Unable to locate file
            }

            var path = AssetDatabase.GUIDToAssetPath(asset);
            return path;
        }
    }
}
