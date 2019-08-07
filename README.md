<img alt="logo" width="150" height="150" src="nuget-logo.png">

Hto3.CollectionHelpers
========================================

#### Nuget Package
[![Hto3.CollectionHelpers](https://img.shields.io/nuget/v/Hto3.CollectionHelpers.svg)](https://www.nuget.org/packages/Hto3.CollectionHelpers/)

Features
--------
A set of extension methods that can be used to facilitate the manipulation of collections solving common dev problems.

### EmptyIfNull

If the collection is null then returns an empty collection.

```csharp
//To-do put an example
```

### Describe

Describes a list in a user-friendly String allowing you to define the format of the item (like `String.Format`), and the string to use to separate the items. Similar experience like `String.Join`.

```csharp
//To-do put an example
```

### IsUnderCollectionChangedEvent

Check if the execution stack is within a CollectionChanged call of an ObservableCollection.

```csharp
//To-do put an example
```

### BuildCollectionFromString

Build a list from a delimited string.

```csharp
//To-do put an example
```

### RemoveAll

Removes all elements that satisfy the condition defined by the specified predicate.

```csharp
//To-do put an example
```

### ReplaceItem

Replaces an item in an ObservableCollection.

```csharp
//To-do put an example
```

### FlatTree

Flatten a tree structure.

```csharp
//To-do put an example
```

### ReplaceAllBy

Replaces all items in an ObservableCollection with other items. It's the same as calling the `Clear()` method and then adding the new items, but reducing the procedure to only one step and improving the performance.

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