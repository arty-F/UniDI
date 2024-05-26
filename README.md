# MonoInjector

Fast and easy to use dependency injection library for Unity game engine.

- **Delegate based reflection hooks** for ultra fast resolving speed.
- **Caching** reflection data for reducing allocations.
- **Small codebase**.
- **Easy to use**.
- **DOES NOT CONTAIN UNNECESSARY FUNCTIONALITY** like other popular DI frameworks.

## Features

- **Field** injection.
- **Property** injection.
- **Method** injection.

## Installation

### Install from git URL

Requires a version of unity that supports path query parameter for git packages (Unity 2021.3 or later). You can add a reference `https://github.com/arty-F/MonoInjector.git?path=Assets/MonoInjector` to Package Manager.

![Screenshot_1](https://github.com/arty-F/MonoInjector/assets/49113047/0a65d9e3-89f2-44ed-8232-713660590d6f)

### Install from .unitypackage

Download the latest `.unitypackage` file from [releases](https://github.com/arty-F/MonoInjector/releases) page and import downloaded package into unity.

![Screenshot_2](https://github.com/arty-F/MonoInjector/assets/49113047/4bb02ea9-bd94-4ab4-8d73-54a64661e2d8)

## Usage

1. Mark field/property/method into which the dependency should be injected with the `[Inject]` attribute.
```csharp
using MonoInjector;
public class TestClass
{
  [Inject] private InjectedClass1 _injectedField;
  [Inject] public InjectedClass2 InjectedProperty { get; private set; }

  [Inject]
  public void Method(InjectedClass3 p1, InjectedClass4 p2)
  {
    _injectedFromMethod1 = p1;
    _injectedFromMethod2 = p2;
  }
  private InjectedClass3 _injectedFromMethod1;
  private InjectedClass4 _injectedFromMethod2;
}
```

2. Inject instances by invoking `Inject()` method. You can use generic version of `Inject<>()` method on `GameObject` where the generic type parameter will represent the type of `GameObject` component that needs to be injected. You can also specify the lifetime of the injection:
- `Lifetime.Game` (default) : the dependency will exist as long as the application is running.
- `Lifetime.Scene` : dependencies will be cleared every time the active scene changes.
```csharp
using MonoInjector;
...
[SerializeField] GameObject _gameObjectPrefab;
[SerializeField] MyComponent _typedPrefab;
...
var csharpClass = new InjectedClass();
csharpClass.Inject();

var gameObjectPrefab = Instantiate(_gameObjectPrefab);
gameObjectPrefab.Inject<MyComponent>();

var typedPrefab = Instantiate(_typedPrefab);
typedPrefab.Inject(Lifetime.Scene);
```

3. Resolve dependencies of injection consumer classes by invoking `Resolve()` method. Or you can use `GameObject` extension method on prefab to one row instantiate and resolving dependencies (has 9 overloads like original Instantiate method).
```csharp
using MonoInjector;
...
[SerializeField] GameObject _gameObjectPrefab;
[SerializeField] MyComponent _typedPrefab;
...
var consumerClass = new TestClass();
consumerClass.Resolve();

GameObject gameObjectInstance1 = Instantiate(_gameObjectPrefab);
gameObjectInstance1.GetComponent<MyComponent>().Resolve();

GameObject gameObjectInstance2 = _gameObjectPrefab.InstantiateResolve<MyComponent>();

MyComponent typedInstance = _typedPrefab.InstantiateResolve(position, rotation);
```

## Performance

### Field Injection

four fields in instance, different types
| | Time | GC Alloc |
| ------------- | ------------- | ------------- |
| 1 instance  | 13 ms  | 46 KB  |
| 100 instances  | 15 ms  | 80 KB  |
| 1000 instances  | 28 ms  | 389 KB  |

### Property Injection

four properties in instance, different types
| | Time | GC Alloc |
| ------------- | ------------- | ------------- |
| 1 instance  | 4 ms  | 21 KB  |
| 100 instances  | 6 ms  | 56 KB  |
| 1000 instances  | 20 ms  | 365 KB  |

### Method Injection

four method parameters, different types
| | Time | GC Alloc |
| ------------- | ------------- | ------------- |
| 1 instance  | 3 ms  | 19 KB  |
| 100 instances  | 4 ms  | 19 KB  |
| 1000 instances  | 6 ms  | 19 KB  |
