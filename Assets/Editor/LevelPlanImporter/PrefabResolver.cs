using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Devilbait.Editor
{
    /// <summary>
    /// Resolves blueprint prefab names to Unity prefab assets.
    /// Searches AssetDatabase under Assets/Prefabs (Level Builder Tools, Interactables, etc.).
    /// </summary>
    public static class PrefabResolver
    {
        private const string PrefabsRoot = "Assets/Prefabs";
        private const string GoalPath = "Assets/Prefabs/Interactables TileMap Version/Goal.prefab";
        private const string PlayerPath = "Assets/Prefabs/Mehdi's_little_guy.prefab";

        private static readonly Dictionary<string, string> KnownAliases = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
        {
            { "FakePlatform", "Fake Platform" },
            { "DeathZone", "Death Zone" },
            { "AttemptTrigger", "Attempt Trigger" },
            { "SpikeTrap", "Spike Trap" },
            { "AmbushTrap-PopUp", "AmbushTrap-PopUp" },
            { "AmbushTrap-Falling", "AmbushTrap-Falling" },
            { "TrapSequencer", "Trap Sequencer" },
            { "AreaTrigger", "Area Trigger" },
            { "CrushingWall", "Crushing Wall" },
            { "MovingTrap", "Moving" },
            { "ArrowTrap", "ArrowTrap" },
            { "DisappearingPlatform", "Disapearing" },
            { "BreakingPlatform", "Collapsing" },
            { "FakeWall", "Fake Wall" },
            { "KillPlayer", "Kill Player" },
            { "SlipperyFloor", "Slippery Floor" },
        };

        /// <summary>
        /// Get the player prefab path (fixed).
        /// </summary>
        public static string GetPlayerPrefabPath() => PlayerPath;

        /// <summary>
        /// Get the goal prefab path (fixed).
        /// </summary>
        public static string GetGoalPrefabPath() => GoalPath;

        /// <summary>
        /// Resolve a blueprint prefab name to an asset path. Returns null if not found.
        /// </summary>
        public static string ResolvePrefabPath(string blueprintName)
        {
            if (string.IsNullOrEmpty(blueprintName)) return null;

            string searchName = blueprintName.Trim();
            if (KnownAliases.TryGetValue(searchName, out string alias))
                searchName = alias;

            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabsRoot });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                if (string.Equals(fileName, searchName, System.StringComparison.OrdinalIgnoreCase))
                    return path;

                if (fileName.IndexOf(searchName, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    return path;

                string normalizedFile = fileName.Replace(" ", "").Replace("-", "").Replace("_", "");
                string normalizedSearch = searchName.Replace(" ", "").Replace("-", "").Replace("_", "");
                if (normalizedFile.IndexOf(normalizedSearch, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    return path;
            }

            return null;
        }

        /// <summary>
        /// Validate that all blueprint prefab names can be resolved.
        /// Returns list of names that could not be resolved (empty if all found).
        /// </summary>
        public static List<string> ValidatePrefabNames(IEnumerable<string> blueprintNames)
        {
            var missing = new List<string>();
            foreach (string name in blueprintNames)
            {
                if (string.IsNullOrWhiteSpace(name)) continue;
                if (ResolvePrefabPath(name) == null)
                    missing.Add(name);
            }
            return missing;
        }

        /// <summary>
        /// Load a prefab at the given path. Returns null if path invalid or load fails.
        /// </summary>
        public static GameObject LoadPrefab(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab;
        }
    }
}
