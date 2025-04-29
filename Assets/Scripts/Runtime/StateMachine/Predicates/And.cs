using System.Collections.Generic;
using UnityEngine;

namespace CatchFire
{
    public class And : IPredicate
    {
        [SerializeField] List<IPredicate> rules = new();

        public bool Evaluate()
        {
            foreach (var rule in rules)
                if (!rule.Evaluate())
                    return false;
            return true;
        }
    }
}