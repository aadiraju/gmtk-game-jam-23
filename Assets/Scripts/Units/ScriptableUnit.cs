using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Type type;
    public BaseUnit UnitPrefab;
}

public enum Type {
    Guard = 0,
    Intruder = 1
}
