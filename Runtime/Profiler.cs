using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace WhiteArrow.StackedProfiling
{
    public static class Profiler
    {
        private static readonly Dictionary<string, SampleData> _samples = new();



        public static void StartSample(string name)
        {
            if (!_samples.TryGetValue(name, out var data))
            {
                data = new();
                _samples[name] = data;
            }

            if (data.Stopwatch.IsRunning)
            {
                Debug.LogWarning($"[StackedProfiler] Sample \"{name}\" is already running.");
                return;
            }

            data.Stopwatch.Restart();
        }

        public static void StopSample(string name)
        {
            if (!_samples.TryGetValue(name, out var data) || !data.Stopwatch.IsRunning)
            {
                Debug.LogWarning($"[StackedProfiler] Cannot stop sample \"{name}\" — it was not started.");
                return;
            }

            data.Stopwatch.Stop();
            data.Durations.Add(data.Stopwatch.Elapsed.TotalMilliseconds);
        }

        public static void LogSample(string name)
        {
            if (!_samples.TryGetValue(name, out var data) || data.Durations.Count == 0)
            {
                Debug.LogWarning($"[StackedProfiler] No data for sample \"{name}\".");
                return;
            }

            var total = data.Durations.Sum();
            var log = $"[StackedProfiler] {name} summary (Total: {total:F3} ms)";

            for (int i = 0; i < data.Durations.Count; i++)
            {
                var duration = data.Durations[i];
                log += $"\n → ({i}) {duration:F3} ms";
            }

            Debug.Log(log);
        }



        private class SampleData
        {
            public readonly Stopwatch Stopwatch = new();
            public readonly List<double> Durations = new();
        }
    }
}