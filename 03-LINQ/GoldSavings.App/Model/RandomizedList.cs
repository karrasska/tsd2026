using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoldSavings.App.Model
{
	// Task 2.2
	public class RandomizedList<T>
	{
		private readonly List<T> _items = new List<T>();
		private readonly Random _random = new Random();

		// IsEmpty - returns true if the collection does not have elements
		public bool IsEmpty()
		{
			return _items.Count == 0;
		}

		// Add (element) – adds an element to the list either at the beginning or at the end, it depends on the random factor
		public void Add(T element)
		{
			// returns 0 or 1
			if (_random.Next(2) == 0)
			{
				// add at the beginning
				_items.Insert(0, element);
			}
			else
			{
				// add at the end
				_items.Add(element);
			}
		}

		// Get (int index) - returns element which is placed at maximum on the position index in the list, but the exact index shall be taken by random
		public T Get(int index)
		{
			if (IsEmpty())
			{
				throw new InvalidOperationException("Cannot get an element from an empty collection.");
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");
			}

			// Max index does not exceed the bounds of the list
			int maxPossibleIndex = Math.Min(index, _items.Count - 1);

			// Generate a random exact index from 0 to maxPossibleIndex (inclusive)
			int exactIndex = _random.Next(0, maxPossibleIndex + 1);

			return _items[exactIndex];
		}
	}
}