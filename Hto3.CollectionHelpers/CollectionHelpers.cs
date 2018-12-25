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
    public static class CollectionHelpers
    {
        /// <summary>
        /// If the collection is null, then it returns an empty collection
        /// </summary>
        /// <typeparam name="T">Tipo da coleção</typeparam>
        /// <param name="source">Coleção a testar se é nulo</param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// If the collection is null, then it returns an empty collection
        /// </summary>
        /// <param name="source">Coleção a testar se é nulo</param>
        /// <returns></returns>
        public static IEnumerable EmptyIfNull(this IEnumerable source)
        {
            return source ?? Enumerable.Empty<Object>();
        }
        /// <summary>
        /// Describes a list in a user-friendly String. <example>Lista de Int32: 1, 2, 3, 4 ou 1, 2, 3 e 4</example>
        /// </summary>
        /// <typeparam name="T">Tipo da coleção</typeparam>
        /// <param name="collection">Lista do que implemente a Interface IList</param>
        /// <param name="comLetraE">Se verdadeiro, coloca a letra "e" entre o penúltimo e o último elemento da lista</param>
        /// <param name="format">String de Formato, a mesma empregada no <i>String.Format</i>. É necessário ter um, e somente um token {0}.</param>
        /// <param name="separator">Texto que separa os itens para descreve-los</param>
        /// <returns>String contendo a lista separada por virgulas</returns>
        public static String DescreverColecao<T>(this IEnumerable<T> collection, String format = "{0}", String separator = ", ")
        {
            if (!format.Contains("{0") || !format.Contains("}"))
                throw new FormatException("O formato deve ter um e somente um token {0}!");

            var retorno = new StringBuilder();
            var colecaoAsArray = collection.ToArray();

            for (Int32 i = 0; i <= colecaoAsArray.Length - 1; i++)
            {
                retorno.AppendFormat(format, colecaoAsArray[i]);

                if (i == colecaoAsArray.Length - 1)
                    retorno.Remove(retorno.Length - separator.Length, separator.Length);
            }

            return retorno.ToString();
        }
        /// <summary>
        /// Check if the execution stack is within a CollectionChanged call of an ObservableCollection
        /// </summary>
        /// <typeparam name="T">Tipo do item da coleção</typeparam>
        /// <param name="collection">A instância da ObservableCollection</param>
        /// <returns></returns>
        public static Boolean IsReentrancy(this INotifyCollectionChanged collection)
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
        /// I build a list from a string separated by a separator character.
        /// </summary>
        /// <typeparam name="TReturn">Tipo da coleção a ser construída</typeparam>
        /// <param name="delimitedList">texto contendo uma lista de valores delimitados de uma maneira padrão</param>
        /// <param name="separator">separador padrão do texto</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ConstruirColecao<TReturn>(this String delimitedList, String separator)
        {
            foreach (var item in delimitedList.Split(new String[] { separator }, StringSplitOptions.RemoveEmptyEntries))
                yield return (TReturn)Convert.ChangeType(item, typeof(TReturn));
        }
        /// <summary>
        /// Removes all elements that satisfy the condition defined by the specified predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">A coleção a se trabalhar.</param>
        /// <param name="match">Um delegate com o predicado que define as condições para escolher os elementos a remover.</param>
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, Boolean> match)
        {
            for (var i = collection.Count - 1; i >= 0; i--)
            {
                if (match(collection[i]))
                    collection.RemoveAt(i);
            }
        }
        /// <summary>
        /// Replaces an item in the collection
        /// </summary>
        /// <typeparam name="T">Tipo da coleção genérica</typeparam>
        /// <param name="collection">Coleção</param>
        /// <param name="oldItem">Item velho (o item velho precisa existir)</param>
        /// <param name="newItem">Item novo</param>
        public static void SubstituirItem<T>(this ObservableCollection<T> collection, T oldItem, T newItem)
        {
            var indexDoVelho = collection.IndexOf(oldItem);
            if (indexDoVelho == -1)
                throw new InvalidOperationException("O item velho não existe na coleção!");

            typeof(ObservableCollection<T>)
                .GetMethod("SetItem", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new Object[] { indexDoVelho, newItem });
        }
        /// <summary>
        /// Replaces an item in the collection
        /// </summary>
        /// <typeparam name="T">Tipo da coleção genérica</typeparam>
        /// <param name="collection">Coleção</param>
        /// <param name="index">Index do item a ser substituído</param>
        /// <param name="newItem">Item novo</param>
        public static void SubstituirItem<T>(this ObservableCollection<T> collection, Int32 index, T newItem)
        {
            typeof(ObservableCollection<T>)
                .GetMethod("SetItem", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new Object[] { index, newItem });
        }
        /// <summary>
        /// Aplaina (flat) uma estrutura de árvore
        /// </summary>
        /// <typeparam name="T">Tipo do item dos nós da árvore</typeparam>
        /// <param name="tree">Árvore</param>
        /// <param name="branchLocator">Expressão que informa a prorpiedade das folhas</param>
        /// <returns></returns>
        public static IEnumerable<T> FlatTree<T>(this IEnumerable<T> tree, Func<T, IEnumerable<T>> branchLocator)
        {
            return tree.EmptyIfNull().SelectMany(c => branchLocator(c).FlatTree(branchLocator)).Concat(tree.EmptyIfNull());
        }
        /// <summary>
        /// Replaces all items in an ObservableCollection with other items. It would be the same as calling the Clear() method and then adding the new items.
        /// </summary>
        /// <typeparam name="T">Tipos dos itens da coleção</typeparam>
        /// <param name="observableCollection">Coleção a se trabalhar</param>
        /// <param name="toAdd">Itens a adicionar na coleção</param>
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
        /// Add multiple items to a collection
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="observableCollection">Instância da ObservableCollection</param>
        /// <param name="toAdd">Coleção de itens a adicionar</param>
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
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> toAdd)
        {
            if (toAdd != null)
            {
                foreach (var item in toAdd)
                    list.Add(item);
            }
        }
        /// <summary>
        /// Add multiple items to a collection without repeating if the item already exists
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="predicate">Pedicado que testa a existência do item</param>
        /// <param name="toAdd">Coleção de itens a adicionar</param>
        public static void AddRangeIfNotExists<T>(this IList<T> list, Func<T, T, Boolean> predicate, IEnumerable<T> toAdd)
        {
            if (toAdd != null)
            {
                foreach (var item in toAdd)
                {
                    if (!list.Any(c => predicate(c, item)))
                        list.Add(item);
                }
            }
        }
        /// <summary>
        /// Moves a position item
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="predicate">Pedicado que obtém o item</param>
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
        /// Add multiple items to a collection without repeating
        /// </summary>
        /// <typeparam name="T">Tipo dos itens da coleção</typeparam>
        /// <param name="list">Instância da coleção</param>
        /// <param name="predicate">Pedicado que testa a existência do item</param>
        /// <param name="toAdd">Coleção de itens a adicionar</param>
        public static void AddRangeIfNotExists<T>(this IList<T> list, IEnumerable<T> toAdd)
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
        /// Adds an item only if it does not exist in the collection
        /// </summary>
        /// <param name="list">Coleção de dados</param>
        /// <param name="item">Item a adicionar</param>
        public static void AddIfNotExists(this IList list, Object item)
        {
            if (list == null)
                return;

            if (!list.Contains(item))
                list.Add(item);
        }
        /// <summary>
        /// Adds an item only if it does not exist in the collection
        /// </summary>
        /// <typeparam name="T">Tipo do item</typeparam>
        /// <param name="list">Coleção de dados</param>
        /// <param name="predicate">Pedicado que testa a existência do item</param>
        /// <param name="item">Item a adicionar</param>
        public static void AddIfNotExists<T>(this IList<T> list, Func<T, Boolean> predicate, T item)
        {
            if (list == null)
                return;

            if (!list.Any(predicate))
                list.Add(item);
        }
        /// <summary>
        /// Removes an item only if it exists in the collection
        /// </summary>
        /// <param name="list">Coleção de dados</param>
        /// <param name="item">Item a remover</param>
        public static void RemoveIfExists(this IList list, Object item)
        {
            if (list == null)
                return;

            if (list.Contains(item))
                list.Remove(item);
        }
        /// <summary>
        /// Gets the type of items in a homogeneous collection
        /// </summary>
        /// <param name="collection">Coleção de dados</param>
        /// <returns>O tipo dos itens</returns>
        public static Type ObterTipoDoItemDaColecao(this ICollection collection)
        {
            var enumerable_type = collection.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerable_type != null)
                return enumerable_type.GenericTypeArguments[0];

            if (collection.Count == 0)
                throw new InvalidOperationException("Não é possível obter o tipo de uma coleção ICollection que não possui nenhum item.");

            return collection.First().GetType();
        }
        /// <summary>
        /// Gets the type of items in a homogeneous collection
        /// </summary>
        /// <param name="collection">Coleção de dados</param>
        /// <returns>O tipo dos itens</returns>
        public static Type ObterTipoDoItemDaColecao(this IEnumerable collection)
        {
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
                    throw new InvalidOperationException("Não é possível obter o tipo de uma coleção IEnumerable que não possui nenhum item.");

                while (enumerator.Current == null && enumerator.MoveNext()) ;

                if (enumerator.Current == null)
                    throw new InvalidOperationException("Não é possível obter o tipo de uma coleção IEnumerable que possui somente itens nulos.");

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
        /// Checks if there is any element in the collection
        /// </summary>
        /// <param name="collection">Coleção de dados</param>
        /// <returns>Verdadeiro, se houver elementos</returns>
        public static Boolean Any(this IEnumerable collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            IEnumerator enumerator = null;

            try
            {
                enumerator = collection.GetEnumerator();
                if (enumerator.MoveNext())
                    return true;
            }
            finally
            {
                var disposableEnumerator = enumerator as IDisposable;
                if (disposableEnumerator != null)
                    disposableEnumerator.Dispose();
            }

            return false;
        }
        /// <summary>
        /// Checks if there is any element in the collection
        /// </summary>
        /// <param name="collection">Coleção de dados</param>
        /// <returns>Verdadeiro, se houver elementos</returns>
        public static Object First(this IEnumerable collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            IEnumerator enumerator = null;

            try
            {
                enumerator = collection.GetEnumerator();
                if (enumerator.MoveNext())
                    return enumerator.Current;
                else
                    throw new InvalidOperationException("A coleção não tem nenhum item!");
            }
            finally
            {
                var disposableEnumerator = enumerator as IDisposable;
                if (disposableEnumerator != null)
                    disposableEnumerator.Dispose();
            }
        }
        /// <summary>
        /// Performs an action for each item in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            var lista = new List<T>();

            foreach (T item in enumeration)
            {
                action(item);
                lista.Add(item);
            }

            return lista;
        }
        /// <summary>
        /// Performs an action for each item in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static IEnumerable<O> ForEachSelect<T, O>(this IEnumerable<T> enumeration, Func<T, O> action)
        {
            List<O> lista = new List<O>();

            foreach (T item in enumeration)
            {
                lista.Add(action(item));
            }

            return lista;
        }
        /// <summary>
        /// Converts a generic collection into an observable collection (ObservableCollection)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumeration)
        {
            return new ObservableCollection<T>(enumeration);
        }
        /// <summary>
        /// Makes a work window in a data collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration">Coleção de dados a ser trabalhada com janela</param>
        /// <param name="windowSize">Quantidade de itens em uma janela</param>
        /// <param name="action">Trabalho a ser realizado em cada janela</param>
        public static void Window<T>(this IEnumerable<T> enumeration, Int32 windowSize, Action<IEnumerable<T>> action)
        {
            var percorridos = 0;
            var janela = default(IEnumerable<T>);

            while (true)
            {
                janela = enumeration.Skip(percorridos).Take(windowSize);
                percorridos += windowSize;

                if (janela.Any())
                    action(janela);
                else
                    return;
            }
        }
    }
}
