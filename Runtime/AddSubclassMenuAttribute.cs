using System;
using UnityEngine;

namespace Rydrako.SerializedPolymorphism
{
    /// <summary>
    /// Add a custom path for a class or struct in the SubclassSelector popup menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AddSubclassMenuAttribute : Attribute
    {
        public string Path { get; private set; }

        public AddSubclassMenuAttribute(string path) => Path = path;
    }
}
