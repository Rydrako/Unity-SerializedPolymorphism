using System;
using UnityEngine;

namespace Rydrako.SerializedPolymorphism
{
    /// <summary>
    /// Attribute to specify the 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.GenericParameter)]
    public class SubclassSelectorAttribute : PropertyAttribute
    {

    }
}