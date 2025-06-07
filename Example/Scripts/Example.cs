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