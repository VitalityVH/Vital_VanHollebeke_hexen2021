using System;
using System.Collections;
using System.Collections.Generic;

namespace Hexen.Plugins
{
    public class BidirectionalDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private List<TKey> _keys = new List<TKey>();
        private List<TValue> _values = new List<TValue>();

        public ICollection<TKey> Keys => _keys;

        public ICollection<TValue> Values => _values;

        public int Count => _keys.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                if (key == null) throw new ArgumentNullException(nameof(key));

                if (!ContainsKey(key)) throw new KeyNotFoundException(nameof(key));

                return InternalRead(key);
            }
            set
            {
                if (key == null) throw new ArgumentNullException(nameof(key));

                if (ContainsKey(key))
                    InternalUpdate(key, value);
                else
                    InternalCreate(key, value);

            }
        }

        TKey this[TValue _value]
        {
            get
            {
                if (_value == null) throw new ArgumentNullException(nameof(_value));

                if (!ContainsValue(_value)) throw new KeyNotFoundException(nameof(_value));

                return InternalRead(_value);
            }
            set
            {
                if (_value == null) throw new ArgumentNullException(nameof(_value));

                if (ContainsValue(_value))
                    InternalUpdate(_value, value);
                else
                    InternalCreate(_value, value);

            }
        }

        public void Add(TKey key, TValue value)
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)this).IsReadOnly) throw new NotSupportedException();

            if (key == null) throw new ArgumentNullException(nameof(key));

            if (ContainsKey(key)) throw new ArgumentException(nameof(key));

            if (ContainsValue(value)) throw new ArgumentException(nameof(value));

            InternalCreate(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            var idx = _keys.IndexOf(key);
            return (idx != -1);
        }

        public bool ContainsValue(TValue value)
        {
            var idx = _values.IndexOf(value);
            return (idx != -1);
        }

        public bool Remove(TKey key)
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)this).IsReadOnly) throw new NotSupportedException();

            if (key == null) throw new ArgumentNullException(nameof(key));

            if (!ContainsKey(key)) return false;

            InternalDelete(key);

            return true;
        }

        public bool Remove(TValue value)
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)this).IsReadOnly) throw new NotSupportedException();

            if (value == null) throw new ArgumentNullException();


            if (!ContainsValue(value)) return false;

            InternalDelete(value);

            return true;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int idx = 0; idx < _keys.Count; idx++)
            {
                var key = _keys[idx];
                var value = _values[idx];

                yield return new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException();

            var idx = _keys.IndexOf(key);

            value = (idx == -1) ? default(TValue) : _values[idx];

            return (idx != -1);
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            if (value == null) throw new ArgumentNullException();

            var idx = _values.IndexOf(value);

            key = (idx == -1) ? default(TKey) : _keys[idx];

            return (idx != -1);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            if (((ICollection<KeyValuePair<TKey, TValue>>)this).IsReadOnly) throw new NotSupportedException();

            _keys.Clear();
            _values.Clear();

        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            var idxKey = _keys.IndexOf(item.Key);
            if (idxKey == -1) return false;

            var idxValue = _values.IndexOf(item.Value);
            if (idxValue == -1) return false;

            return (idxValue == idxKey);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException();

            if (arrayIndex < 0) throw new ArgumentOutOfRangeException();

            if ((array.Length - arrayIndex) < _keys.Count) throw new ArgumentException();

            for (int idx = 0; idx < _keys.Count; idx++)
            {
                var key = _keys[idx];
                var value = _values[idx];

                array[arrayIndex + idx] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            var idxKey = _keys.IndexOf(item.Key);
            if (idxKey == -1) return false;

            var idxValue = _values.IndexOf(item.Value);
            if (idxValue == -1) return false;

            if (idxKey != idxValue) return false;

            return Remove(item.Key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void InternalCreate(TKey key, TValue value)
        {
            _keys.Add(key);
            _values.Add(value);
        }

        private void InternalCreate(TValue value, TKey key)
        {
            _keys.Add(key);
            _values.Add(value);
        }

        private TValue InternalRead(TKey key)
        {
            var idx = _keys.IndexOf(key);
            return _values[idx];
        }

        private TKey InternalRead(TValue value)
        {
            var idx = _values.IndexOf(value);
            return _keys[idx];
        }

        private void InternalUpdate(TKey key, TValue value)
        {
            var idx = _keys.IndexOf(key);
            _values[idx] = value;
        }

        private void InternalUpdate(TValue value, TKey key)
        {
            var idx = _values.IndexOf(value);
            _keys[idx] = key;
        }

        private void InternalDelete(TKey key)
        {
            var idx = _keys.IndexOf(key);
            _keys.RemoveAt(idx);
            _values.RemoveAt(idx);
        }

        private void InternalDelete(TValue value)
        {
            var idx = _values.IndexOf(value);
            _keys.RemoveAt(idx);
            _values.RemoveAt(idx);
        }
    }
}
