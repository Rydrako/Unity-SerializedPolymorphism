# Serialized Polymorphism

The 'SubclassSelector' attribute allows you to easily set subclasses to the fields of abstract classes in the Editor that are serialized by the 'SerializeReference' attribute.

#### Features
- Easily set subclass by popup for fields and lists with the 'SubclassSelector' attribute
- Override the display name and path with the 'AddSubclassMenu' attribute
- Displays custom Script icons

## 📥 Installation

#### Install via `.unitypackage`

Download any version from releases.

Releases: https://github.com/Rydrako/Unity-SerializedPolymorphism/releases

#### Install via git URL

Or, you can add this package by opening PackageManager and entering

```
https://github.com/Rydrako/Unity-SerializedPolymorphism.git?path=Assets/Rydrako/Rydrako.SerializedPolymorphism
```

from the `Add package from git URL` option.

If you are specifying a version, enter `#{VERSION}` at the end, as shown below.

```
https://github.com/Rydrako/Unity-SerializedPolymorphism.git?path=Assets/Rydrako/Rydrako.SerializedPolymorphism#1.1.1
```


## 🔰 Usage

Notes
- The 'SerializeReference' attribute is required to be on the same field for the 'SubclassSelector' attribute to work.

```cs
using Rydrako.SerializedPolymorphism;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubclassSelectorExample", menuName = "SubclassSelectorExample")]
public class Example : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    public Action _action;

    // Supports interfaces
    [SerializeReference, SubclassSelector]
    public IAction _interfaceAction;

    // Supports Collections
    [SerializeReference, SubclassSelector]
    public IAction[] _arrayActions = Array.Empty<IAction>();

    [SerializeReference, SubclassSelector]
    public List<IAction> _listActions = new List<IAction>();
}

public interface IAction
{
    void Execute();
}

[Serializable]
public abstract class Action : IAction
{
    abstract public void Execute();
}

[Serializable]
public class DebugAction : Action
{
    [SerializeField] string _message;

    public override void Execute()
    {
        Debug.Log(_message);
    }
}

[Serializable]
public class InstantiateAction : IAction
{
    [SerializeField] GameObject _prefab;

    public void Execute()
    {
        UnityEngine.Object.Instantiate(_prefab);
    }
}

[AddSubclassMenu("Example/Add Subclass Menu Action")]
[Serializable]
public class AddSubclassMenuAction : IAction
{
    public void Execute()
    {
        Debug.Log("Executed AddSubclassMenuAction");
    }
}
```


## ❓ FAQ

### If the type is renamed, the reference is lost.

It is a limitation of `SerializeReference` of Unity.

When serializing a `SerializeReference` reference, the type name, namespace, and assembly name are used, so if any of these are changed, the reference cannot be resolved during deserialization.

To solve this problem, `UnityEngine.Scripting.APIUpdating.MovedFromAttribute` can be used. More info on it can be found [here](https://forum.unity.com/threads/serializereference-data-loss-when-class-name-is-changed.736874/).

#### References
- https://www.youtube.com/watch?v=6qd22ulEds4