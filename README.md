# Grouped Performance

Lightweight performance profiling framework for Unity with grouped, labeled samples.

Unlike Unity's built-in Profiler, GroupedPerformance focuses on logic-level performance analysis. It enables manual instrumentation of time measurements, organized by named groups, and outputs results directly to the console — ideal for startup sequences, initialization phases, or benchmarking game systems.

# Features

- Grouped performance measurements with labeled samples
- Millisecond precision using `System.Diagnostics.Stopwatch`
- Console output for any tracked group
- Does not rely on Unity timeline or Profiler API
- One-off sample measurement with automatic console output

# Installing

To install GroupedPerformance via Unity Package Manager:

1. Open your Unity project.
2. Go to `Window > Package Manager`.
3. Click the `+` button and choose `Add package from Git URL...`.
4. Paste the following URL:

```
https://github.com/CliffCalist/grouped-performance.git
```

5. Click `Add`.

# Usage

```csharp
PerformanceProfiler.StartSample("InitSomething");

// your code

PerformanceProfiler.StopSample("InitSomething");
```

---

```csharp
PerformanceProfiler.LogSample("InitSomething");
```

This will output:

```
[PerformanceProfiler] InitSomething
 ├ total:      37.826 ms
 ├ avg:        12.609 ms
 ├ min:        11.403 ms
 └ max:        13.281 ms
 → (0) StepOne                    — 11.403 ms
 → (1) StepTwo                    — 13.281 ms
 → (2) StepThree                  — 13.142 ms
```

## Conditional Compilation

GroupedPerformance uses `ConditionalAttribute` with a compilation symbol to remove all profiling code at compile time when profiling is disabled.

The symbol used is:

```
WA_ENABLE_PERFORMANCE_PROFILING
```

Because this is handled via `[Conditional(...)]`, you can freely call profiling methods throughout your codebase — the compiler will automatically strip these calls from the build if the symbol is not defined.

To enable profiling, you have two options:

1. **Via Editor Menu** (recommended):  
   Navigate to:

   ```
   Tools > White Arrow > Performance Profiling > Enable / Disable
   ```

   This will automatically toggle the `WA_ENABLE_PERFORMANCE_PROFILING` symbol for the current build target group.

2. **Manually**:
   - Go to **Edit > Project Settings > Player > Scripting Define Symbols**
   - Add or remove `WA_ENABLE_PERFORMANCE_PROFILING` from the active build target group
   - Or define it through build scripts, CI, or scripting APIs

For advanced use, you can reference the constant containing the symbol name:

```csharp
PerformanceProfiler.CompilationSymbol
```

This allows you to create your own conditional utilities or wrappers that align with GroupedPerformance’s compilation behavior.

## Simple measurement

For quick measurements that do not require grouping or tracking multiple samples, you can use the simple API:

```csharp
PerformanceProfiler.StartSimpleSample("Load Inventory");
// ... your code ...
PerformanceProfiler.StopSimpleSample("Load Inventory");
```

This will automatically log a result like:

```
[PerformanceProfiler] Load Inventory — 47.219 ms
```

# Roadmap

- [x] Console logging with detailed breakdown
- [ ] Optional support for `using` syntax (`using PerformanceProfiler.Scoped(...)`)
- [ ] Export to JSON or CSV
- [ ] Editor window for browsing stored data
- [ ] Integration with Unity log files
