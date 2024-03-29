![logo](https://raw.githubusercontent.com/HTO3/Hto3.CollectionHelpers/master/nuget-logo-small.png)

Hto3.CollectionHelpers
========================================

[![License](https://img.shields.io/github/license/HTO3/Hto3.CollectionHelpers)](https://github.com/HTO3/Hto3.CollectionHelpers/blob/master/LICENSE)
[![Hto3.CollectionHelpers](https://img.shields.io/nuget/v/Hto3.CollectionHelpers.svg)](https://www.nuget.org/packages/Hto3.CollectionHelpers/)
[![Downloads](https://img.shields.io/nuget/dt/Hto3.CollectionHelpers)](https://www.nuget.org/stats/packages/Hto3.CollectionHelpers?groupby=Version)
[![Build Status](https://github.com/HTO3/Hto3.CollectionHelpers/actions/workflows/publish.yml/badge.svg)](https://github.com/HTO3/Hto3.CollectionHelpers/actions/workflows/publish.yml)
[![codecov](https://codecov.io/gh/HTO3/Hto3.CollectionHelpers/branch/master/graph/badge.svg)](https://codecov.io/gh/HTO3/Hto3.CollectionHelpers)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/a0dfd6171df24d6b97676b6e8fcc799d)](https://www.codacy.com/gh/HTO3/Hto3.CollectionHelpers/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=HTO3/Hto3.CollectionHelpers&amp;utm_campaign=Badge_Grade)

Features
--------
A set of extension methods that can be used to facilitate the manipulation of collections solving common dev problems.

### EmptyIfNull

If the collection is null then returns an empty collection.

```csharp
IEnumerable<String> myCollection = null;
myCollection.EmptyIfNull().Count() == 0; //you won't get a NullReferenceException
```

### Describe

Describes a list in a user-friendly String allowing you to define the format of the item (like `String.Format`), and the string to use to separate the items. Similar experience like `String.Join`.

```csharp
var collection = new List<Int32>();
collection.Add(1);
collection.Add(2);
collection.Add(3);

collection.Describe() == "1, 2, 3";
```

### IsUnderCollectionChangedEvent

Check if the execution stack is within a CollectionChanged call of an ObservableCollection.

```csharp
var observableCollection = new ObservableCollection<String>();

observableCollection.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) =>
{
    var onEvent = observableCollection.IsUnderCollectionChangedEvent();
    if (onEvent)
    {
        Console.WriteLine("We are inside a collection changed event!");
    }
});
observableCollection.Add("first");
```

### BuildCollectionFromString

Build a typed list from a delimited string.

```csharp
var delimitedString = "banana;apple;juice;lemon";

var result = delimitedString.BuildCollectionFromString<String>(";");

result.ElementAt(0) == "banana";
result.ElementAt(1) == "apple";
result.ElementAt(2) == "juice";
result.ElementAt(3) == "lemon";
```
Or dealing with cultural content:
```csharp
var delimitedString = "30/07/2019;01/12/2020;26/02/2021";

var result = delimitedString.BuildCollectionFromString<DateTime>(";", new System.Globalization.CultureInfo("pt-BR"));

result.ElementAt(0) == new DateTime(2019, 7, 30);
result.ElementAt(1) == new DateTime(2020, 12, 1);
result.ElementAt(2) == new DateTime(2021, 2, 26);
```

### RemoveAll

Removes all elements that satisfy the condition defined by the specified predicate.

```csharp
var collection = new ObservableCollection<Int32>();
collection.Add(1);
collection.Add(2);
collection.Add(55);
collection.Add(100);

collection.RemoveAll(i => i > 10);

collection.Count == 2;
collection[0] == 1;
collection[1] == 2;
```

### ReplaceItem

Replaces an item in an ObservableCollection.

```csharp
var collection = new ObservableCollection<String>();
collection.Add("banana");
collection.Add("apple");
collection.Add("pinapple");

collection.ReplaceItem("apple", "strawberry");

collection.Count == 3;
collection[0] == "banana";
collection[1] == "strawberry";
collection[2] == "pinapple";
```

### FlatTree

Flatten a tree structure.

```csharp
//To-do put an example
```

### ReplaceAllBy

Replaces all items in an ObservableCollection with other items. It's the same as calling the <i>Clear()</i> method and then adding the new items (optmized performance).

```csharp
//To-do put an example
```

### AddRange

Add multiple items to a collection invoking the change event only one time, at the end.

```csharp
//To-do put an example
```

### AddRangeIfNotExists

Add multiple items to a collection without repeating if the item already exists.

```csharp
//To-do put an example
```

### Move

Moves a position item.

```csharp
//To-do put an example
```

### AddIfNotExists

Adds an item only if it does not exist in the collection.

```csharp
//To-do put an example
```

### RemoveIfExists

Removes an item only if it exists in the collection.

```csharp
//To-do put an example
```

### GetItemType

Gets the type of items in a homogeneous collection.

```csharp
//To-do put an example
```

### SymmetricDifference

Gets the symmetric difference of two sets using an equality comparer. The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.

```csharp
//To-do put an example
```

### ForEach

Performs immediately an action for each item in the collection.

```csharp
//To-do put an example
```

### ForEachSelect

Performs immediately an action for each item in the collection.

```csharp
//To-do put an example
```

### ToObservableCollection

Converts a generic collection into an observable collection (ObservableCollection).

```csharp
//To-do put an example
```

### Window

Makes a work window in a data collection.

```csharp
//To-do put an example
```

### TryUntilSuccess

Try something on each item of a collection. For more advanced needs, check https://www.nuget.org/packages/Polly/.

```csharp
//To-do put an example
```

### PickRandom

Pick a random item from the collection or the default item value if the sequence contains no elements.

```csharp
//To-do put an example
```

### PickRandomOrDefault

Pick a random item from the collection or the default item value if the sequence contains no elements.

```csharp
//To-do put an example
```

### Shuffle

Shuffle the collection.

```csharp
//To-do put an example
```

### Run

Force one complete evaluation of an IEnumerable collection. Subsequent evaluation can occur after use this method, in another words, the collection will continue to be IEnumerable.

```csharp
//To-do put an example
```