using UnityEngine;
using Assets.Scripts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

//RESOLVES AN EFFECT'S TARGETTYPE + TARGETID TO THE ACTUAL ITEM(S) IT APPLIES TO.
//TARGETID OF "All" RETURNS EVERY ITEM OF TARGETTYPE (EG: EVERY CURRENCY).
public static class TargetResolver
{
    public static IEnumerable<IHasValue> Find(string targetType, string targetId)
    {
        switch (targetType)
        {
            case "Currency":
                return Filter(CurrencyManager.Instance.Items.Values, targetId);
            case "Generator":
                return Filter(GeneratorManager.Instance.Items.Values, targetId);
            default:
                Debug.LogWarning($"TargetResolver: unknown TargetType '{targetType}'");
                return Array.Empty<IHasValue>();
        }
    }

    private static IEnumerable<IHasValue> Filter<T>(IEnumerable<T> items, string targetId) where T : IHasId, IHasValue
    {
        if (targetId == "All")
            return items.Cast<IHasValue>();

        var match = items.FirstOrDefault(i => i.Id == targetId);
        return match != null ? new IHasValue[] { match } : Array.Empty<IHasValue>();
    }
}
