# Grouped Performance

Lightweight performance profiling framework for Unity with grouped, labeled samples.

Unlike Unity's built-in Profiler, GroupedPerformance focuses on logic-level performance analysis. It enables manual instrumentation of time measurements, organized by named groups, and outputs results directly to the console — ideal for startup sequences, initialization phases, or benchmarking game systems.

# Features

- Grouped performance measurements with labeled samples
- Millisecond precision using `System.Diagnostics.Stopwatch`
- Console output for any tracked group
- Does not rely on Unity timeline or Profiler API

# Installing

To install GroupedPerformance via Unity Package Manager:

1. Open your Unity project.
2. Go to `Window > Package Manager`.
3. Click the `+` button and choose `Add package from Git URL...`.
4. Paste the following URL:

```
https://github.com/white-arrow/grouped-performance.git
```

5. Click `Add`.

# Usage

## Basic profiling

```csharp
PerformanceProfiler.StartSample("InitSomething");

// your code

PerformanceProfiler.StopSample("InitSomething");
```

## Logging results

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

# Roadmap

- [x] Console logging with detailed breakdown
- [ ] Optional support for `using` syntax (`using PerformanceProfiler.Scoped(...)`)
- [ ] Export to JSON or CSV
- [ ] Editor window for browsing stored data
- [ ] Integration with Unity log files
