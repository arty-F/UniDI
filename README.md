# UniDI

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
- Injection lifetime.
- Local scope injection.

## Installation

### Install from git URL

Requires a version of unity that supports path query parameter for git packages (Unity 2021.3 or later). You can add a reference `https://github.com/arty-F/UniDI.git?path=Assets/UniDI` to Package Manager.

![Screenshot_1](https://github.com/arty-F/MonoInjector/assets/49113047/0a65d9e3-89f2-44ed-8232-713660590d6f)

### Install from .unitypackage

Download the latest `.unitypackage` file from [releases](https://github.com/arty-F/UniDI/releases) page and import downloaded package into unity.

![Screenshot_2](https://github.com/arty-F/MonoInjector/assets/49113047/4bb02ea9-bd94-4ab4-8d73-54a64661e2d8)

## Usage

1. Mark field/property/method into which the dependency should be injected with the `[Inject]` attribute.
```csharp
using UniDI;
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

2. Inject instances by invoking `Inject()` method. You can use generic version of `Inject<>()` method on `GameObject` where the generic type parameter will represent the type of `GameObject` component that needs to be injected.
```csharp
using UniDI;
...
[SerializeField] GameObject _gameObjectPrefab;
[SerializeField] InjectedComponent _typedPrefab;
...
//plain C# class injecting
var csharpClass = new InjectedClass();
csharpClass.Inject();

//bad way GameObject component injecting
var gameObjectInstance1 = Instantiate(_gameObjectPrefab);
gameObjectInstance1.GetComponent<InjectedComponent>().Inject();

//good way GameObject component injecting
var gameObjectInstance2 = Instantiate(_gameObjectPrefab);
gameObjectInstance2.Inject<InjectedComponent>();

//best way GameObject component injecting
InjectedComponent typedInstance = Instantiate(_typedPrefab);
typedInstance.Inject();
```

3. Resolve dependencies of injection consumer classes by invoking `Resolve()` method. Or you can use `GameObject` extension method on prefab to one row instantiate and resolving dependencies (has 9 overloads like original Instantiate method). After `GameObject` resolving all injected dependencies will be available in `Start` method (in `Awake` and `OnEnable` they still will be `null`).
```csharp
using UniDI;
...
[SerializeField] GameObject _gameObjectPrefab;
[SerializeField] ConsumerComponent _typedPrefab;
...
//plain C# class resolving
var csharpClass = new TestClass();
csharpClass.Resolve();

//bad way GameObject resolving
GameObject gameObjectInstance1 = Instantiate(_gameObjectPrefab);
gameObjectInstance1.GetComponent<ConsumerComponent>().Resolve();

//good way GameObject resolving
GameObject gameObjectInstance2 = _gameObjectPrefab.InstantiateResolve<ConsumerComponent>();

//best way GameObject resolving
ConsumerComponent typedInstance = _typedPrefab.InstantiateResolve(position, rotation);
```

## Features

### Injection lifetime

You can specify the lifetime of the injection:
- `Lifetime.Game` (default) : the dependency will exist as long as the application is running.
- `Lifetime.Scene` : dependencies will be cleared every time the active scene changes.
```csharp
var injectedClass = new InjectedClass();
injectedClass.Inject(Lifetime.Scene);
```

### Local scope injection

By default, all injected classes will be placed in the global scope. In order to set the local scope for dependency injection, you must specify the scope identifier based on `int` when injecting and resolving dependencies. When using resolve for local scope if the dependency was not found in the specified local scope, then they will try to find it in the global scope. By this way you can inject both global and local dependencies into one consumer.
```csharp
public class Consumer : MonoBehaviour
{
  [Inject] private GlobalInjectionClass _field1;
  [Inject] private LocalInjectionClass _field2;
}
...
int id = consumer.GetInstanceID();

var globalInjectionClass = new GlobalInjectionClass();
globalInjectionClass.Inject();

var localInjectionClass = new LocalInjectionClass();
localInjectionClass.Inject(id);

consumer.Resolve(id);
```

### Injection clearing

You can override or clear previously injected dependencies. For overriding just use `Inject()` on new instance. For clearing use `ReleaseDependency()` on same type instance.
- Global scope dependency clearing by `ReleaseDependency()`.
- Local scope dependency clearing by `ReleaseDependency(id)` where `id` is a local scope `int` type identifier. By default this type of clear deletes the entire local scope. To avoid completely clearing the local scope you must pass a value of `clearFullScope` is `false`, like that `ReleaseDependency(id, false)`. By passing this `false` value you will clear only the type on which this method was called in the specified local scope.
```csharp
globalDependencyInstance.ReleaseDependency();            //clear globalDependencyInstance in global scope
localDependencyInstance1.ReleaseDependency(id);          //clear all dependencies in specified local scope
localDependencyInstance2.ReleaseDependency(id, false);   //clear localDependencyInstance2 in specified local scope
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
