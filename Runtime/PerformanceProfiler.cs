using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Debug = UnityEngine.Debug;

namespace WhiteArrow.GroupedPerformance
{
    public static class PerformanceProfiler
    {
        private static readonly Dictionary<string, SampleGroup> s_groups = new();
        private static readonly Dictionary<string, Stopwatch> _simpleSamples = new();




        [Conditional("ENABLE_STACKED_PROFILING")]
        public static void StartSimpleSample(string label)
        {
            if (_simpleSamples.ContainsKey(label))
            {
                Debug.LogWarning($"[PerformanceProfiler] Simple sample '{label}' is already running.");
                return;
            }

            var sw = Stopwatch.StartNew();
            _simpleSamples[label] = sw;
        }

        [Conditional("ENABLE_STACKED_PROFILING")]
        public static void StopSimpleSample(string label)
        {
            if (!_simpleSamples.TryGetValue(label, out var sw))
            {
                Debug.LogWarning($"[PerformanceProfiler] Simple sample '{label}' was not started.");
                return;
            }

            sw.Stop();
            _simpleSamples.Remove(label);

            Debug.Log($"[PerformanceProfiler] {label} — {sw.Elapsed.TotalMilliseconds:F3} ms");
        }



        [Conditional("ENABLE_PROFILING")]
        public static void StartSample(string groupName, string label)
        {
            if (!s_groups.TryGetValue(groupName, out var group))
            {
                group = new();
                s_groups[groupName] = group;
            }

            var sample = new LabeledSample
            {
                Label = label,
                Stopwatch = Stopwatch.StartNew()
            };

            group.Samples.Add(sample);
        }

        [Conditional("ENABLE_PROFILING")]
        public static void StopSample(string groupName, string label)
        {
            if (!s_groups.TryGetValue(groupName, out var group))
                return;

            var sample = group.Samples.LastOrDefault(s => s.Label == label && s.Stopwatch.IsRunning);
            if (sample == null)
                return;

            sample.Stopwatch.Stop();
            sample.DurationMs = sample.Stopwatch.Elapsed.TotalMilliseconds;
        }



        [Conditional("ENABLE_PROFILING")]
        public static void LogGroup(string groupName)
        {
            if (!s_groups.TryGetValue(groupName, out var group) || group.Samples.Count == 0)
            {
                Debug.LogWarning($"[StackedProfiler] No group found with name '{groupName}'.");
                return;
            }

            var finished = group.Samples.Where(s => s.IsFinished).ToList();

            var total = finished.Sum(s => s.DurationMs);
            var avg = finished.Count > 0 ? finished.Average(s => s.DurationMs) : 0;
            var min = finished.Count > 0 ? finished.Min(s => s.DurationMs) : 0;
            var max = finished.Count > 0 ? finished.Max(s => s.DurationMs) : 0;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"[StackedProfiler] {groupName}");
            sb.AppendLine($" ├ total:\t{total,10:F3} ms");
            sb.AppendLine($" ├ avg:\t\t{avg,10:F3} ms");
            sb.AppendLine($" ├ min:\t\t{min,10:F3} ms");
            sb.AppendLine($" └ max:\t\t{max,10:F3} ms");

            for (int i = 0; i < group.Samples.Count; i++)
            {
                var sample = group.Samples[i];

                if (sample.IsFinished)
                {
                    sb.AppendLine($" → ({i}) {sample.Label,-30} — {sample.DurationMs,8:F3} ms");
                }
                else
                {
                    var current = sample.Stopwatch.Elapsed.TotalMilliseconds;
                    sb.AppendLine($" → ({i}) {sample.Label,-30} — {current,8:F3} ms (in progress)");
                }
            }

            Debug.Log(sb.ToString());
        }



        private class SampleGroup
        {
            public readonly List<LabeledSample> Samples = new();
        }

        private class LabeledSample
        {
            public string Label;
            public Stopwatch Stopwatch;
            public double DurationMs;
            public bool IsFinished => Stopwatch == null ? false : !Stopwatch.IsRunning;
        }
    }
}