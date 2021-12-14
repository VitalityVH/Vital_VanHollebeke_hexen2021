using System;
using System.Collections.Generic;

namespace Hexen.Plugins
{
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey,List<TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            if (TryGetValue(key, out var lst))
            {
                lst.Add(value);
            }
            else
            {
                Add(key, new List<TValue>(){value});
            }
        }

        public bool Remove(TKey key, TValue value)
        {
            if (key==null) throw new ArgumentNullException(nameof(key));
            
            if (TryGetValue(key,out var lst))
            {
                lst.Remove(value);
                if (lst.Count == 0)
                {
                    Remove(key);
                }
                return true;
            }

            return false;
        }
    }
}
