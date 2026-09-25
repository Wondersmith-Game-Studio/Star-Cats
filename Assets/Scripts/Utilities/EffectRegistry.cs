using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

//MAPS AN EFFECT'S "OPERATION" TO THE MATH THAT APPLIES IT.
//ADDING A NEW OPERATION MEANS ADDING ONE ENTRY HERE - NOTHING ELSE HAS TO CHANGE.
public static class EffectRegistry
{
    private static readonly Dictionary<string, Action<IHasValue, double>> _operations = new()
    {
        ["Add"] = (target, value) => target.Value += value,
        ["Multiply"] = (target, value) => target.Value *= value,
    };

    public static void Apply(Effect effect)
    {
        if (!_operations.TryGetValue(effect.Operation, out var operation))
        {
            Debug.LogWarning($"EffectRegistry: no handler registered for operation '{effect.Operation}'");
            return;
        }

        var targets = TargetResolver.Find(effect.TargetType, effect.TargetId).ToList();
        if (targets.Count == 0)
        {
            Debug.LogWarning($"EffectRegistry: no targets found for {effect.TargetType} '{effect.TargetId}'");
            return;
        }

        foreach (var target in targets)
            operation(target, effect.Value);
    }
}
