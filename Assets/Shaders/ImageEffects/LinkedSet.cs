using System;
using System.Collections.Generic;

namespace Negi
{
	public class LinkedSet<T> : IEnumerable<T>
	{

		private LinkedList<T> _list;

		private Dictionary<T, LinkedListNode<T>> _dictionary;

		public LinkedSet()
		{
			_list = new LinkedList<T>();
			_dictionary = new Dictionary<T, LinkedListNode<T>>();
		}

		public LinkedSet(IEqualityComparer<T> comparer)
		{
			_list = new LinkedList<T>();
			_dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
		}

		public bool Contains(T t)
		{
			return _dictionary.ContainsKey(t);
		}

		public bool Add(T t)
		{

			if (_dictionary.ContainsKey(t))
			{
				return false;
			}

			LinkedListNode<T> node = _list.AddLast(t);
			_dictionary.Add(t, node);
			return true;

		}

		public void Clear()
		{
			_list.Clear();
			_dictionary.Clear();
		}

		public AddType AddOrMoveToEnd(T t)
		{

			LinkedListNode<T> node;

			if (_dictionary.Comparer.Equals(t, _list.Last.Value))
			{
				return AddType.NO_CHANGE;
			}
			else if (_dictionary.TryGetValue(t, out node))
			{
				_list.Remove(node);
				node = _list.AddLast(t);
				_dictionary[t] = node;
				return AddType.MOVED;
			}
			else
			{
				node = _list.AddLast(t);
				_dictionary[t] = node;
				return AddType.ADDED;
			}

		}

		public bool Remove(T t)
		{

			LinkedListNode<T> node;

			if (_dictionary.TryGetValue(t, out node) && _dictionary.Remove(t))
			{
				_list.Remove(node);
				return true;
			}
			else
			{
				return false;
			}

		}

		public void ExceptWith(IEnumerable<T> enumerable)
		{
			foreach (T t in enumerable)
			{
				Remove(t);
			}
		}

		public IEnumerator<T> GetEnumerator ()
		{
			return _list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return _list.GetEnumerator();
		}

		public enum AddType
		{

			/// <summary>
			/// No changes were made
			/// </summary>
			NO_CHANGE,

			/// <summary>
			/// The value was added
			/// </summary>
			ADDED,

			/// <summary>
			/// The value was moved to the end.
			/// </summary>
			MOVED

		}

	}
}

