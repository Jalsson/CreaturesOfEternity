using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField]
    private float baseValue;

    private List<float> modifiers = new List<float>();

    /// <summary>
    /// Gives the value of all modifiers currently stored in this class. multiply this value with the value you want to effect
    /// </summary>
    /// <returns></returns>
    public float GetValue()
    {
        if (modifiers.Count > 0)
        {
            float finalValue = baseValue;
            modifiers.ForEach(x => finalValue += x);
            finalValue = (100 - finalValue) / 100;
            return finalValue;
        }
        else
            return 1;
    }

    public float GetOriginalValue()
    {
            float finalValue = baseValue;
            modifiers.ForEach(x => finalValue += x);
            return finalValue;
    }

    public void AddModifier (float modifier)
    {
        if (modifier != 0)
            modifiers.Add(modifier);
        
    }

    public void RemoveModifier (float modifier)
    {
        if (modifier != 0)
            modifiers.Remove(modifier);
        
    }
}
