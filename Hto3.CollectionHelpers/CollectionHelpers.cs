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

namespace Hto3.CollectionHelpers
{
    /// <summary>
    /// Class containing helpers for collection manipulation.
    /// </summary>
    public static class CollectionHelpers
    {
        /// <summary>
        /// If the collection is null then returns an empty collection
        /// </summary>
        /// <typeparam name="T">Collection item type</typeparam>
        /// <param name="source">Collection to test if is null</param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// If the collection is null then returns an empty collection
        /// </summary>
        /// <param name="source">Collection to test if is null</param>
        /// <returns></returns>
        public static IEnumerable EmptyIfNull(this IEnumerable source)
        {
            return source ?? Enumerable.Empty<Object>();
        }
        /// <summary>
        /// Describes a list in a user-friendly String allowing you to define the format of the item (like String.Format), and the string to use to separate the items. Similar experience like String.Join <example>List of Int32: 1, 2, 3, 4</example>
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="format">Format string, the same as used in <i>String.Format</i> method. It's necessary to use one, and only one {0} token.</param>
        /// <param name="separator">String to separate the itens</param>
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
        /// <summary>
        /// Check if the execution stack is within a CollectionChanged call of an ObservableCollection
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
        /// <summary>
        /// Build a list from a delimited string.
        /// </summary>
        /// <typeparam name="TReturn">Type of the itens of the collection</typeparam>
        /// <param name="delimitedList">String containing a list of delimited values</param>
        /// <param name="separator">Separator that is used to delimit the itens</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> BuildCollectionFromString<TReturn>(this String delimitedList, String separator)
        {
            foreach (var item in delimitedList.Split(new String[] { separator }, StringSplitOptions.RemoveEmptyEntries))
                yield return (TReturn)Convert.ChangeType(item, typeof(TReturn));
        }
        /// <summary>
        /// Removes all elements that satisfy the condition defined by the specified predicate
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
        /// <param name="oldItem">Old item (old item must exist)</param>
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
        /// Replaces an item in an ObservableCollection.
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
        /// Flatten a tree structure
        /// </summary>
        /// <typeparam name="T">Tree nodes item type</typeparam>
        /// <param name="roots">The multi root tree</param>
        /// <param name="branchLocator">Expression that indicate where is the branch property</param>
        /// <returns></returns>
        public static HashSet<T> FlatTree<T>(this IEnumerable<T> roots, Func<T, IEnumerable<T>> branchLocator)
        {
            var flattedTree = new HashSet<T>();
            CollectionHelpers.InternalFlatTree(roots, branchLocator, flattedTree);
            return flattedTree;
        }
        /// <summary>
        /// Flatten a tree structure
        /// </summary>
        /// <typeparam name="T">Tree nodes item type</typeparam>
        /// <param name="root">The Tree</param>
        /// <param name="branchLocator">Expression that indicate where is the branch property</param>
        /// <returns></returns>
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
        /// Internal method to flat a tree structure, witch contains a walking collection to store the nodes.
        /// </summary>
        /// <typeparam name="T">Tree nodes item type</typeparam>
        /// <param name="branches">The multi root tree</param>
        /// <param name="branchLocator">Expression that indicate where is the branch property</param>
        /// <param name="alreadyVisited">Collection to store the nodes</param>
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
        /// Force one complete evaluation of an IEnumerable collection. Subsequent evaluation can occur after use this method, in another words, the collection will continue to be IEnumerable.
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
        /// Replaces all items in an ObservableCollection with other items. It's the same as calling the <i>Clear()</i> method and then adding the new items, but reducing the procedure to only one step and improving the performance.
        /// </summary>
        /// <typeparam name="T">Type of the collection item</typeparam>
        /// <param name="observableCollection">The observable collection</param>
        /// <param name="toAdd">Items to add</param>
        public static void ReplaceAllBy<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> toAdd)
        {
            if (observableCollection == null || !toAdd.EmptyIfNull().Any())
                return;

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
        /// Add multiple items to a collection invoking the change event only one time, at the end.
        /// </summary>
        /// <typeparam name="T">>Type of the collection item</typeparam>
        /// <param name="observableCollection">The observable collection</param>
        /// <param name="toAdd">Items to add</param>
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
        /// Add multiple items to a collection
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="toAdd">Coleção de itens a adicionar</param>
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> toAdd)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (toAdd == null)
                return;

            foreach (var item in toAdd)
                list.Add(item);
        }
        /// <summary>
        /// Add multiple items to a collection without repeating if the item already exists
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="predicate">Pedicado que testa a existência do item</param>
        /// <param name="toAdd">Coleção de itens a adicionar</param>
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
        /// Add multiple items to a collection without repeating
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="toAdd">Coleção de itens a adicionar</param>
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
        /// Moves a position item
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="item"></param>
        /// <param name="toIndex">Lugar aonde colocar o item</param>
        public static void Move<T>(this IList<T> list, T item, Int32 toIndex)
        {
            list.Remove(item);
            list.Insert(toIndex, item);
        }
        /// <summary>
        /// Moves a position item
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="predicate">Pedicado que obtém o item</param>
        /// <param name="toIndex">Lugar aonde colocar o item</param>
        public static void Move<T>(this IList<T> list, Func<T, Boolean> predicate, Int32 toIndex)
        {
            var item = list.SingleOrDefault(predicate);
            list.Remove(item);
            list.Insert(toIndex, item);
        }
        
        /// <summary>
        /// Adds an item only if it does not exist in the collection
        /// </summary>
        /// <param name="list">Coleção de dados</param>
        /// <param name="item">Item a adicionar</param>
        public static void AddIfNotExists(this IList list, Object item)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (!list.Contains(item))
                list.Add(item);
        }
        /// <summary>
        /// Adds an item only if it does not exist in the collection
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <param name="collection">Coleção de dados</param>
        /// <param name="predicate">Pedicado que testa a existência do item</param>
        /// <param name="item">Item a adicionar</param>
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
        /// Removes an item only if it exists in the collection
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="item">Item to remove</param>
        public static void RemoveIfExists(this IList list, Object item)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Contains(item))
                list.Remove(item);
        }
        /// <summary>
        /// Gets the type of items in a homogeneous collection
        /// </summary>
        /// <param name="collection">Coleção de dados</param>
        /// <returns>O tipo dos itens</returns>
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
        /// Gets the type of items in a homogeneous collection
        /// </summary>
        /// <param name="collection">Coleção de dados</param>
        /// <returns>O tipo dos itens</returns>
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
        /// The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.
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
        /// Performs immediately an action for each item in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
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
        /// Performs immediately an action for each item in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
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
        /// Converts a generic collection into an observable collection (ObservableCollection)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumeration)
        {
            if (enumeration == null)
                throw new ArgumentNullException(nameof(enumeration));

            return new ObservableCollection<T>(enumeration);
        }
        /// <summary>
        /// Makes a work window in a data collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration">Collection of data to be worked with window</param>
        /// <param name="windowSize">Amount of items in each window</param>
        /// <param name="action">Work to do in each window</param>
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
    }
}
