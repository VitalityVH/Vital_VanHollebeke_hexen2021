using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hexen.SelectionSystem
{
    public class SelectionEventArgs<TSelectableItem> : EventArgs
    {
        public TSelectableItem SelectionItem { get; }

        public SelectionEventArgs(TSelectableItem selectionItem)
        {
            SelectionItem = selectionItem;
        }
    }
    public class SelectionManager<TSelectableItem>
    {
        public event EventHandler<SelectionEventArgs<TSelectableItem>> Selected;
        public event EventHandler<SelectionEventArgs<TSelectableItem>> Deselected;

        private HashSet<TSelectableItem> _selectedItems = new HashSet<TSelectableItem>();

        public TSelectableItem SelectedItem => _selectedItems.First();

        public bool HasSelection => _selectedItems.Count > 0;

        public bool IsSelected(TSelectableItem selectableItem)
            => _selectedItems.Contains(selectableItem);

        public bool Select(TSelectableItem selectableItem)
        {
            if (_selectedItems.Add(selectableItem))
            {
                OnSelected(new SelectionEventArgs<TSelectableItem>(selectableItem));
                return true;
            }

            return false;
            // return _selectedItems.Add(selectableItem);
        }

        public bool Deselect(TSelectableItem selectableItem)
        {
            if (_selectedItems.Remove(selectableItem))
            {
                OnDeselected(new SelectionEventArgs<TSelectableItem>(selectableItem));
                return true;
            }

            return false;
            // Debug.Log($"Deselected {selectableItem}");
            // return _selectedItems.Remove(selectableItem);
        }
        
        public bool Toggle(TSelectableItem selectableItem)
        {
            if (IsSelected(selectableItem))
            {
                return Deselect(selectableItem);
            }
            else
            {
                return Select(selectableItem);
            }
        }

        public void DeselectAll()
        {
            foreach (TSelectableItem selectedItem in _selectedItems.ToList())
            {
                Deselect(selectedItem);
            }
        }

        protected virtual void OnSelected(SelectionEventArgs<TSelectableItem> e)
        {
            var handler = Selected;
            handler?.Invoke(this, e);
        }

        protected virtual void OnDeselected(SelectionEventArgs<TSelectableItem> e)
        {
            var handler = Deselected;
            handler?.Invoke(this, e);
        }
    }
}