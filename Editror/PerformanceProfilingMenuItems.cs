using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using WhiteArrow.GroupedPerformance;

namespace WhiteArrowEditor.GroupedPerformance
{
    public static class PerformanceProfilingMenuItems
    {
        private const string MenuRoot = "Tools/WhiteArrow/Performance Profiling/";



        private static NamedBuildTarget CurrentNamedBuildTarget
        {
            get
            {
#if UNITY_SERVER
                return NamedBuildTarget.Server;
#else
                var buildTarget = EditorUserBuildSettings.activeBuildTarget;
                var targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                return NamedBuildTarget.FromBuildTargetGroup(targetGroup);
#endif
            }
        }



        [MenuItem(MenuRoot + "Enable", validate = true)]
        private static bool Enable_Validate()
        {
            return !IsEnabled();
        }

        [MenuItem(MenuRoot + "Enable")]
        private static void Enable()
        {
            EnableProfiling();
        }



        [MenuItem(MenuRoot + "Disable", validate = true)]
        private static bool Disable_Validate()
        {
            return IsEnabled();
        }

        [MenuItem(MenuRoot + "Disable")]
        private static void Disable()
        {
            DisableProfiling();
        }



        private static bool IsEnabled()
        {
            return GetScriptDefineSymbols().Contains(PerformanceProfiler.COMPILATION_SYMBOL);
        }

        private static void EnableProfiling()
        {
            var defines = GetScriptDefineSymbols();
            var alreadyPresent = defines.Contains(PerformanceProfiler.COMPILATION_SYMBOL);

            if (!alreadyPresent)
            {
                defines.Add(PerformanceProfiler.COMPILATION_SYMBOL);
                PlayerSettings.SetScriptingDefineSymbols(CurrentNamedBuildTarget, string.Join(";", defines));
            }
        }

        private static void DisableProfiling()
        {
            var defines = GetScriptDefineSymbols();
            var alreadyPresent = defines.Contains(PerformanceProfiler.COMPILATION_SYMBOL);

            if (alreadyPresent)
            {
                defines.Remove(PerformanceProfiler.COMPILATION_SYMBOL);
                PlayerSettings.SetScriptingDefineSymbols(CurrentNamedBuildTarget, string.Join(";", defines));
            }
        }



        private static List<string> GetScriptDefineSymbols()
        {
            return PlayerSettings.GetScriptingDefineSymbols(CurrentNamedBuildTarget)
                .Split(';')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }
    }
}