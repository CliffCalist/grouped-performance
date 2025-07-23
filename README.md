# StackedProfiling

Custom profiler for Unity that focuses on sequential sample analysis rather than live timeline inspection.  
Unlike Unity's built-in Profiler, which is optimized for real-time performance spikes, StackedProfiling stores all measurement data in memory until the end of the game session — making it ideal for analyzing startup sequences, custom logic, or initialization flows.

# Features

- Manual sample start/stop by name
- Stores all durations in memory
- Millisecond precision using `System.Diagnostics.Stopwatch`
- Console output for any tracked sample
- Zero GC, thread-safe API (if used carefully)
- Does not rely on Unity timeline or Profiler API

# Installing

Add the following line to your `manifest.json` under `dependencies`:

```json
"com.white-arrow.stacked-profiling": "https://github.com/white-arrow/stacked-profiling.git"
```

# Usage

## Basic profiling

```csharp
StackedProfiler.StartSample("InitSomething");

// your code

StackedProfiler.StopSample("InitSomething");
```

## Logging results

```csharp
StackedProfiler.LogSample("InitSomething");
```

This will output:

```
[StackedProfiler] InitSomething summary (Total: 37.826 ms)
 → (0) 11.403 ms
 → (1) 13.281 ms
 → (2) 13.142 ms
```

You can call `StartSample` / `StopSample` multiple times with the same name — the profiler will track each measurement independently.

# Roadmap

- [x] Manual sample tracking with string keys
- [x] Console logging with detailed breakdown
- [ ] Support for `IDisposable` usage (`using StackedProfiler.Sample(...)`)
- [ ] Grouped samples (e.g., per frame, per system)
- [ ] Sample nesting (parent/child hierarchy)
- [ ] Export to JSON or CSV
- [ ] Editor window for browsing stored data
- [ ] Integration with Unity log files
