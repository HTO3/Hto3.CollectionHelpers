using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Hto3.CollectionHelpers
{
    /// <summary>
    /// Contains helper methods for collection manipulation.
    /// </summary>
    public static class CollectionHelpers
    {
        /// <summary>
        /// If the collection is null, returns an empty collection.
        /// </summary>
        /// <typeparam name="T">Collection item type</typeparam>
        /// <param name="source">Collection to test if is null</param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
        /// <summary>
        /// Determines whether the specified sequence is null or contains no elements.
        /// </summary>
        /// <remarks>Use this method to safely check for both null and empty sequences when working with
        /// collections. This extension method is useful for avoiding null reference exceptions and simplifying
        /// conditional logic.</remarks>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence to check for null or emptiness.</param>
        /// <returns>true if the sequence is null or contains no elements; otherwise, false.</returns>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
        /// <summary>
        /// If the collection is null, returns an empty collection.
        /// </summary>
        /// <param name="source">Collection to test if is null</param>
        /// <returns></returns>
        public static IEnumerable EmptyIfNull(this IEnumerable source)
        {
            return source ?? Enumerable.Empty<Object>();
        }
        /// <summary>
        /// Describes a list as a user-friendly string, allowing you to define the format for each item (like <c>String.Format</c>) and the separator string. Similar to <c>String.Join</c>.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="format">Format string, the same as used in <i>String.Format</i>. The format must contain exactly one {0} token.</param>
        /// <param name="separator">String to separate the items.</param>
        /// <returns></returns>
        public static String Describe<T>(this IEnumerable<T> collection, String format = "{0}", String separator = ", ")
        {
            if (!format.Contains("{0") || !format.Contains("}"))
                throw new FormatException("The format must have one and only one token {0}!");

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var output = new StringBuilder();

            foreach (var item in collection)
            {
                output.AppendFormat(format, item);
                output.Append(separator);
            }

            if (output.Length > 0 && !String.IsNullOrEmpty(separator))
                output.Remove(output.Length - separator.Length, separator.Length);

            return output.ToString();
        }
#if NETFRAMEWORK
        /// <summary>
        /// Checks whether the execution stack is within an ObservableCollection's <c>CollectionChanged</c> call.
        /// </summary>
        /// <param name="collection">The observable collection</param>
        /// <returns></returns>
        public static Boolean IsUnderCollectionChangedEvent<T>(this ObservableCollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var monitor = collection.GetType()
                .GetField("_monitor", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(collection);

            var monitorIsBusy = (Boolean)monitor.GetType()
                .GetProperty("Busy")
                .GetValue(monitor);

            return monitorIsBusy;
        }
#endif
        /// <summary>
        /// Builds a typed list from a delimited string.
        /// </summary>
        /// <typeparam name="TReturn">Type of the itens of the collection</typeparam>
        /// <param name="delimitedList">String containing a list of delimited values</param>
        /// <param name="separator">Separator that is used to delimit the itens</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> BuildCollectionFromString<TReturn>(this String delimitedList, String separator)
        {
            return BuildCollectionFromString<TReturn>(delimitedList, separator, Thread.CurrentThread.CurrentCulture);
        }
        /// <summary>
        /// Builds a typed list from a delimited string.
        /// </summary>
        /// <typeparam name="TReturn">Type of the items in the collection.</typeparam>
        /// <param name="delimitedList">String containing a list of delimited values.</param>
        /// <param name="separator">Separator used to delimit the items.</param>
        /// <param name="formatProvider">Format provider used to parse each item.</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> BuildCollectionFromString<TReturn>(this String delimitedList, String separator, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
                throw new ArgumentNullException(nameof(formatProvider), $"The {formatProvider} cannot be null.");

            foreach (var item in delimitedList.Split(new String[] { separator }, StringSplitOptions.RemoveEmptyEntries))
                yield return (TReturn)Convert.ChangeType(item, typeof(TReturn), formatProvider);
        }
        /// <summary>
        /// Removes all elements that satisfy the specified predicate.
        /// </summary>
        /// <typeparam name="T">Type of the collection item</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="match">A delegate with the predicate that defines the conditions for choosing which elements to remove.</param>
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, Boolean> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            for (var i = collection.Count - 1; i >= 0; i--)
            {
                if (match(collection[i]))
                    collection.RemoveAt(i);
            }
        }
        /// <summary>
        /// Replaces an item in an ObservableCollection.
        /// </summary>
        /// <typeparam name="T">Type of the collection item</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="oldItem">Old item. The old item must exist in the collection.</param>
        /// <param name="newItem">The new item</param>
        public static void ReplaceItem<T>(this ObservableCollection<T> collection, T oldItem, T newItem)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var oldIndex = collection.IndexOf(oldItem);
            if (oldIndex == -1)
                throw new InvalidOperationException("The old item does not exist in collection!");

            typeof(ObservableCollection<T>)
                .GetMethod("SetItem", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new Object[] { oldIndex, newItem });
        }
        /// <summary>
        /// Replaces an item in an ObservableCollection by index.
        /// </summary>
        /// <typeparam name="T">Type of the collection item</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="index">The index of the item to replace</param>
        /// <param name="newItem">The new item</param>
        public static void ReplaceItem<T>(this ObservableCollection<T> collection, Int32 index, T newItem)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            typeof(ObservableCollection<T>)
                .GetMethod("SetItem", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new Object[] { index, newItem });
        }
        /// <summary>
        /// Flattens a tree structure.
        /// </summary>
        /// <typeparam name="T">Tree node item type.</typeparam>
        /// <param name="roots">The multi-root tree.</param>
        /// <param name="branchLocator">Function that returns the children of a node.</param>
        /// <returns>A set with all visited nodes.</returns>
        public static HashSet<T> FlatTree<T>(this IEnumerable<T> roots, Func<T, IEnumerable<T>> branchLocator)
        {
            var flattedTree = new HashSet<T>();
            CollectionHelpers.InternalFlatTree(roots, branchLocator, flattedTree);
            return flattedTree;
        }
        /// <summary>
        /// Flattens a tree starting from a single root node.
        /// </summary>
        /// <typeparam name="T">Tree node item type.</typeparam>
        /// <param name="root">The root node.</param>
        /// <param name="branchLocator">Function that returns the children of a node.</param>
        /// <returns>A set with all visited nodes.</returns>
        public static HashSet<T> FlatTree<T>(this T root, Func<T, IEnumerable<T>> branchLocator)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            var alreadyVisited = new HashSet<T>();
            alreadyVisited.Add(root);
            CollectionHelpers.InternalFlatTree(branchLocator(root), branchLocator, alreadyVisited);
            return alreadyVisited;
        }
        /// <summary>
        /// Internal method to flatten a tree structure which uses a collection to store visited nodes.
        /// </summary>
        /// <typeparam name="T">Tree node item type.</typeparam>
        /// <param name="branches">The branches to traverse.</param>
        /// <param name="branchLocator">Function that returns the children of a node.</param>
        /// <param name="alreadyVisited">Collection used to store visited nodes.</param>
        /// <returns></returns>
        private static void InternalFlatTree<T>(IEnumerable<T> branches, Func<T, IEnumerable<T>> branchLocator, HashSet<T> alreadyVisited)
        {
            foreach (var node in branches.EmptyIfNull())
            {
                if (alreadyVisited.Add(node))
                    CollectionHelpers.InternalFlatTree(branchLocator(node), branchLocator, alreadyVisited);
            }
        }
        /// <summary>
        /// Forces a complete evaluation of an <see cref="IEnumerable{T}"/> collection.
        /// Subsequent evaluations can still occur after using this method; in other words, the collection remains an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="collection">IEnumerable collection</param>
        /// <returns></returns>
        public static IEnumerable<T> Run<T>(this IEnumerable<T> collection)
        {
            foreach (var _ in collection);
            return collection;
        }
        /// <summary>
        /// Replaces all items in an ObservableCollection with other items. It's equivalent to calling <i>Clear()</i> and then adding the new items (optimized for performance).
        /// </summary>
        /// <typeparam name="T">Type of the collection item</typeparam>
        /// <param name="observableCollection">The observable collection</param>
        /// <param name="toAdd">Items to add</param>
        public static void ReplaceAllBy<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> toAdd)
        {
            if (observableCollection == null)
                return;

            if (toAdd == null)
                throw new ArgumentNullException(nameof(toAdd), $"The parameter {nameof(toAdd)} cannot be null.");

            if (!toAdd.Any())
            {
                observableCollection.Clear();
                return;
            }

            var type = observableCollection.GetType();

            type.InvokeMember("CheckReentrancy", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, null);
            var itemsProp = type.BaseType.GetProperty("Items", BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
            var privateItems = itemsProp.GetValue(observableCollection) as IList;

            privateItems.Clear();
            foreach (var item in toAdd.EmptyIfNull())
                privateItems.Add(item);

            type.InvokeMember("OnPropertyChanged", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, new object[] { new PropertyChangedEventArgs("Count") });
            type.InvokeMember("OnPropertyChanged", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, new object[] { new PropertyChangedEventArgs("Item[]") });
            type.InvokeMember("OnCollectionChanged", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, new object[] { new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) });
        }
        /// <summary>
        /// Adds multiple items to a collection, raising the change event only once at the end.
        /// </summary>
        /// <typeparam name="T">Type of the collection item.</typeparam>
        /// <param name="observableCollection">The observable collection.</param>
        /// <param name="toAdd">Items to add.</param>
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> toAdd)
        {
            if (observableCollection == null || !toAdd.EmptyIfNull().Any())
                return;

            var type = observableCollection.GetType();

            type.InvokeMember("CheckReentrancy", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, null);
            var itemsProp = type.BaseType.GetProperty("Items", BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
            var privateItems = itemsProp.GetValue(observableCollection) as IList<T>;

            foreach (var item in toAdd.EmptyIfNull())
                privateItems.Add(item);

            type.InvokeMember("OnPropertyChanged", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, new object[] { new PropertyChangedEventArgs(nameof(observableCollection.Count)) });
            type.InvokeMember("OnPropertyChanged", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, new object[] { new PropertyChangedEventArgs("Item[]") });
            type.InvokeMember("OnCollectionChanged", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, observableCollection, new object[] { new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) });
        }
        /// <summary>
        /// Add multiple items to an ArrayList.
        /// </summary>
        /// <param name="arrayList">ArrayList instance.</param>
        /// <param name="toAdd">Items to add.</param>
        public static void AddRange(this ArrayList arrayList, IEnumerable toAdd)
        {
            if (arrayList == null)
                throw new ArgumentNullException(nameof(arrayList));
            if (toAdd == null)
                return;

            foreach (var item in toAdd)
                arrayList.Add(item);
        }
        /// <summary>
        /// Adds multiple items to a collection without duplicating existing items. Compares items using a predicate; if the predicate returns true for an existing item, the new item will not be added.
        /// </summary>
        /// <typeparam name="T">Collection item type.</typeparam>
        /// <param name="list">Collection instance.</param>
        /// <param name="predicate">Predicate used to determine equality between items.</param>
        /// <param name="toAdd">Items to add.</param>
        public static void AddRangeIfNotExists<T>(this ICollection<T> list, Func<T, T, Boolean> predicate, IEnumerable<T> toAdd)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (toAdd == null)
                return;

            foreach (var item in toAdd)
            {
                if (!list.Any(c => predicate(c, item)))
                    list.Add(item);
            }
        }
        /// <summary>
        /// Adds multiple items to a collection without duplicating existing items.
        /// </summary>
        /// <typeparam name="T">Collection item type.</typeparam>
        /// <param name="list">Collection instance.</param>
        /// <param name="toAdd">Items to add.</param>
        public static void AddRangeIfNotExists<T>(this ICollection<T> list, IEnumerable<T> toAdd)
        {
            if (toAdd != null)
            {
                foreach (var item in toAdd)
                {
                    if (!list.Contains(item))
                        list.Add(item);
                }
            }
        }
        /// <summary>
        /// Moves an item to a different position in the list.
        /// </summary>
        /// <typeparam name="T">Collection item type.</typeparam>
        /// <param name="list">Collection instance.</param>
        /// <param name="item">Item to move.</param>
        /// <param name="toIndex">Desired index location.</param>
        public static void Move<T>(this IList<T> list, T item, Int32 toIndex)
        {
            list.Remove(item);
            list.Insert(toIndex, item);
        }
        /// <summary>
        /// Moves an item that matches a predicate to a different position in the list.
        /// </summary>
        /// <typeparam name="T">Collection item type.</typeparam>
        /// <param name="list">Collection instance.</param>
        /// <param name="predicate">Predicate to identify the item to move. Exactly one item must match this predicate.</param>
        /// <param name="toIndex">Desired index location.</param>
        public static void Move<T>(this IList<T> list, Func<T, Boolean> predicate, Int32 toIndex)
        {
            var item = list.Single(predicate);
            list.Remove(item);
            list.Insert(toIndex, item);
        }

        /// <summary>
        /// Adds an item only if it does not exist in the collection.
        /// </summary>
        /// <param name="list">Collection instance.</param>
        /// <param name="item">Item to add.</param>
        public static void AddIfNotExists(this IList list, Object item)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (!list.Contains(item))
                list.Add(item);
        }
        /// <summary>
        /// Adds an item only if it does not exist in the collection.
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="predicate">Predicate to ensure equality.</param>
        /// <param name="item">Item to add.</param>
        public static void AddIfNotExists<T>(this ICollection<T> collection, Func<T, Boolean> predicate, T item)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (!collection.Any(predicate))
                collection.Add(item);
        }
        /// <summary>
        /// Removes an item only if it exists in the collection.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="item">Item to remove.</param>
        public static void RemoveIfExists(this IList list, Object item)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Contains(item))
                list.Remove(item);
        }
        /// <summary>
        /// Gets the type of items in a homogeneous collection.
        /// </summary>
        /// <param name="collection">Collection instance.</param>
        /// <returns>The item <see cref="Type"/>.</returns>
        public static Type GetItemType(this ICollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var enumerable_type = collection.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerable_type != null)
                return enumerable_type.GenericTypeArguments[0];

            if (collection.Count == 0)
                throw new InvalidOperationException("Cannot discover the collection item type in an empty ICollection.");

            return collection.First().GetType();
        }
        private static Object First(this IEnumerable collection)
        {
            IEnumerator enumerator = null;

            try
            {
                enumerator = collection.GetEnumerator();
                if (enumerator.MoveNext())
                    return enumerator.Current;
                else
                    throw new InvalidOperationException("The collection is empty!");
            }
            finally
            {
                var disposableEnumerator = enumerator as IDisposable;
                if (disposableEnumerator != null)
                    disposableEnumerator.Dispose();
            }
        }
        /// <summary>
        /// Gets the type of items in a homogeneous collection.
        /// </summary>
        /// <param name="collection">Collection instance.</param>
        /// <returns>The item <see cref="Type"/>.</returns>
        public static Type GetItemType(this IEnumerable collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var enumerable_type = collection.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerable_type != null && enumerable_type.GenericTypeArguments[0] != typeof(Object))
                return enumerable_type.GenericTypeArguments[0];

            IEnumerator enumerator = null;
            Type type = null;

            try
            {
                enumerator = collection.GetEnumerator();
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException("Can not get the type of an IEnumerable collection that has no items.");

                while (enumerator.Current == null && enumerator.MoveNext()) ;

                if (enumerator.Current == null)
                    throw new InvalidOperationException("Can not get the type of an IEnumerable collection that has only null items.");

                type = enumerator.Current.GetType();
            }
            finally
            {
                var disposableEnumerator = enumerator as IDisposable;
                if (disposableEnumerator != null)
                    disposableEnumerator.Dispose();
            }

            return type;
        }
        /// <summary>
        /// Gets the symmetric difference of two sets using an equality comparer.
        /// The symmetric difference is the set of elements that are in one set but not in both.
        /// </summary>
        /// <remarks>
        /// If one set has duplicate items when evaluated using the comparer, then the resulting symmetric difference will only
        /// contain one copy of the the duplicate item and only if it doesn't appear in the other set.
        /// </remarks>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="value">The first enumerable.</param>
        /// <param name="secondSet">The second enumerable to compare against the first.</param>
        /// <param name="comparer">The comparer object to use to compare each item in the collection.  If null uses EqualityComparer(T).Default.</param>
        /// <returns></returns>
        public static IEnumerable<T> SymmetricDifference<T>(this IEnumerable<T> value, IEnumerable<T> secondSet, IEqualityComparer<T> comparer)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (secondSet == null)
                throw new ArgumentNullException(nameof(secondSet));
            
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            var result = value.Except(secondSet, comparer).Union(secondSet.Except(value, comparer), comparer);
            return result;
        }
        /// <summary>
        /// Immediately performs an action for each item in the collection.
        /// </summary>
        /// <typeparam name="T">Item collection to iterate.</typeparam>
        /// <param name="enumeration">The collection instance.</param>
        /// <param name="action">Action to perform.</param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            if (enumeration == null)
                throw new ArgumentNullException(nameof(enumeration));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var lista = new List<T>();

            foreach (T item in enumeration)
            {
                action(item);
                lista.Add(item);
            }

            return lista;
        }
        /// <summary>
        /// Immediately performs an action for each item in the collection and returns a new collection of the results.
        /// </summary>
        /// <typeparam name="T">Item collection to iterate.</typeparam>
        /// <typeparam name="O">Projected result type.</typeparam>
        /// <param name="enumeration">The collection instance.</param>
        /// <param name="action">Function to perform for each item.</param>
        public static IEnumerable<O> ForEachSelect<T, O>(this IEnumerable<T> enumeration, Func<T, O> action)
        {
            if (enumeration == null)
                throw new ArgumentNullException(nameof(enumeration));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var list = new List<O>();

            foreach (T item in enumeration)
            {
                list.Add(action(item));
            }

            return list;
        }
        /// <summary>
        /// Converts a generic collection into an <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="enumeration">Collection to transform into an ObservableCollection.</param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumeration)
        {
            if (enumeration == null)
                throw new ArgumentNullException(nameof(enumeration));

            return new ObservableCollection<T>(enumeration);
        }
        /// <summary>
        /// Creates a sliding window over a collection and executes an action for each window.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="enumeration">Collection of data to process in windows.</param>
        /// <param name="windowSize">Number of items in each window.</param>
        /// <param name="action">Action to execute for each window.</param>
        public static void Window<T>(this IEnumerable<T> enumeration, Int32 windowSize, Action<IEnumerable<T>> action)
        {
            if (enumeration == null)
                throw new ArgumentNullException(nameof(enumeration));
            if (windowSize > -1)
                throw new ArgumentOutOfRangeException(nameof(windowSize));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var cursor = 0;
            var window = default(IEnumerable<T>);

            while (true)
            {
                window = enumeration.Skip(cursor).Take(windowSize);
                cursor += windowSize;

                if (window.Any())
                    action(window);
                else
                    return;
            }
        }
        /// <summary>
        /// Tries an action on each item of a collection until one attempt succeeds.
        /// </summary>
        /// <remarks>
        /// For more advanced needs, see https://www.nuget.org/packages/Polly/.
        /// </remarks>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="enumeration">The collection.</param>
        /// <param name="attempt">Action that can fail, throwing an exception. The most recent exception (if any) is provided as the second parameter.</param>
        /// <param name="stopIfExceptionType">If an exception that is assignable from this type is thrown, attempts will stop and the exception will be rethrown.</param>
        public static void TryUntilSuccess<T>(this IEnumerable<T> enumeration, Action<T, Exception> attempt, Type stopIfExceptionType = null)
        {
            if (enumeration == null)
                throw new ArgumentNullException(nameof(enumeration));
            if (stopIfExceptionType != null && !typeof(Exception).IsAssignableFrom(stopIfExceptionType))
                throw new ArgumentException($"The type {stopIfExceptionType} is not an exception");

            var exceptionList = new List<Exception>();

            foreach (var item in enumeration)
            {
                try
                {
                    attempt(item, exceptionList.LastOrDefault());
                    return;
                }
                catch (Exception ex)
                {
                    if (stopIfExceptionType != null && stopIfExceptionType.IsAssignableFrom(ex.GetType()))
                        throw;
                    exceptionList.Add(ex);
                }
            }

            throw new AggregateException(exceptionList);
        }
        /// <summary>
        /// Picks a random item from the collection.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.InvalidOperationException">The source sequence is empty.</exception>
        /// <returns></returns>
        public static T PickRandom<T>(this IEnumerable<T> collection)
        {
            return collection.PickRandom(1).First<T>();
        }
        /// <summary>
        /// Picks a random item from the collection or the default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="collection">The collection</param>
        /// <returns></returns>
        public static T PickRandomOrDefault<T>(this IEnumerable<T> collection)
        {
            return collection.PickRandom(1).FirstOrDefault();
        }
        /// <summary>
        /// Picks a random number of items from the collection.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="count">Amount of itens.</param>
        /// <returns></returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> collection, int count)
        {
            return collection.Shuffle().Take(count);
        }
        /// <summary>
        /// Shuffles the collection.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            return collection.OrderBy(i => Guid.NewGuid());
        }
    }
}
