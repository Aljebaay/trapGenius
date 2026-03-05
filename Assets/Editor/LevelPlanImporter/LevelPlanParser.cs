using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Devilbait.Editor
{
    /// <summary>
    /// Parses the "## Build Blueprint (Hybrid)" section from a LEVEL_###.md file.
    /// Extracts PlayerSpawn, Goal, and prefab entries with positions.
    /// </summary>
    public static class LevelPlanParser
    {
        private const string BlueprintSectionHeader = "## Build Blueprint (Hybrid)";
        private static readonly Regex CoordsRegex = new Regex(@"\(\s*(-?\d+)\s*,\s*(-?\d+)\s*\)");

        /// <summary>For spawn: lower-case, remove spaces, "-", "_", and apostrophes (' and ').</summary>
        private static string NormalizeSpawnKey(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            string t = s.Trim().ToLowerInvariant();
            t = t.Replace(" ", "").Replace("-", "").Replace("_", "").Replace("'", "").Replace("\u2019", "");
            return t;
        }

        /// <summary>Extract first (x,y) from line; supports negative numbers.</summary>
        private static Vector2IntParse? ExtractCoords(string line)
        {
            var m = CoordsRegex.Match(line);
            if (!m.Success) return null;
            return new Vector2IntParse
            {
                X = int.Parse(m.Groups[1].Value),
                Y = int.Parse(m.Groups[2].Value)
            };
        }

        private static bool IsSpawnKey(string normalizedKey)
        {
            if (string.IsNullOrEmpty(normalizedKey)) return false;
            return normalizedKey == "playerspawn"
                   || normalizedKey == "player"
                   || normalizedKey == "mehdislittleguy";
        }

        /// <summary>Key raw = text before first "(", remove leading "-", trim, part before ":"; preserves casing. Key normalized = same steps then NormalizeSpawnKey.</summary>
        private static void ExtractKeyFromLineWithCoords(string line, out string keyRaw, out string keyNormalized)
        {
            int idx = line.IndexOf('(');
            string keyText = idx >= 0 ? line.Substring(0, idx) : line;
            keyText = keyText.Trim().TrimStart('-').Trim();
            int colonIdx = keyText.IndexOf(':');
            if (colonIdx >= 0)
                keyText = keyText.Substring(0, colonIdx).Trim();
            keyRaw = keyText;
            keyNormalized = NormalizeSpawnKey(keyText);
        }

        /// <summary>True if line looks like a prefab bullet: "- X" or "- X: ...".</summary>
        private static bool LooksLikePrefabBullet(string line)
        {
            string t = line.Trim();
            if (t.Length == 0 || t[0] != '-') return false;
            t = t.Substring(1).Trim();
            return t.Length > 0;
        }

        public struct Vector2IntParse
        {
            public int X;
            public int Y;
            public Vector3 ToVector3() => new Vector3(X, Y, 0f);
        }

        public struct PrefabEntry
        {
            public string PrefabName;
            public Vector2IntParse Position;
        }

        public class BlueprintData
        {
            public bool HasPlayerSpawn;
            public Vector2IntParse PlayerSpawn;
            public bool HasGoal;
            public Vector2IntParse Goal;
            public List<PrefabEntry> Prefabs = new List<PrefabEntry>();
            /// <summary>Prefab names that appeared in the blueprint but had no explicit (x,y) — parsing error.</summary>
            public List<string> MissingPositionPrefabNames = new List<string>();
            /// <summary>Messages to log when "X or Y" was detected (e.g. "Alternative detected: 'X or Y' → picked 'X'").</summary>
            public List<string> AlternativeLogMessages = new List<string>();
        }

        /// <summary>
        /// Extract the Build Blueprint (Hybrid) section from full markdown content.
        /// Returns null if section not found.
        /// </summary>
        public static string GetBlueprintSection(string fullMarkdown)
        {
            if (string.IsNullOrEmpty(fullMarkdown)) return null;
            int start = fullMarkdown.IndexOf(BlueprintSectionHeader, StringComparison.OrdinalIgnoreCase);
            if (start < 0) return null;
            int sectionStart = start + BlueprintSectionHeader.Length;
            int nextH2 = fullMarkdown.IndexOf("\n## ", sectionStart, StringComparison.Ordinal);
            int end = nextH2 >= 0 ? nextH2 : fullMarkdown.Length;
            return fullMarkdown.Substring(sectionStart, end - sectionStart);
        }

        /// <summary>
        /// Parse blueprint section text into structured data.
        /// </summary>
        public static BlueprintData Parse(string blueprintSection)
        {
            var data = new BlueprintData();
            if (string.IsNullOrEmpty(blueprintSection)) return data;

            string[] lines = blueprintSection.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool inPrefabsSection = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();

                if (trimmed.StartsWith("Prefabs", StringComparison.OrdinalIgnoreCase))
                    inPrefabsSection = true;
                if (trimmed.StartsWith("Notes", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.StartsWith("Terrain", StringComparison.OrdinalIgnoreCase))
                    inPrefabsSection = false;

                if (!inPrefabsSection)
                    continue;

                Vector2IntParse? coords = ExtractCoords(line);

                if (coords.HasValue)
                {
                    ExtractKeyFromLineWithCoords(line, out string keyRaw, out string keyNormalized);

                    if (IsSpawnKey(keyNormalized))
                    {
                        data.HasPlayerSpawn = true;
                        data.PlayerSpawn = coords.Value;
                        data.AlternativeLogMessages.Add("Spawn detected: " + line);
                        continue;
                    }
                    if (keyNormalized == "goal")
                    {
                        data.HasGoal = true;
                        data.Goal = coords.Value;
                        data.AlternativeLogMessages.Add("Goal detected: " + line);
                        continue;
                    }

                    // Prefab entry: display name = keyRaw (preserve casing); handle "X or Y" → pick first
                    string displayName = keyRaw;
                    if (displayName.Contains(" or "))
                    {
                        string original = displayName;
                        displayName = displayName.Substring(0, displayName.IndexOf(" or ", StringComparison.Ordinal)).Trim();
                        data.AlternativeLogMessages.Add("Alternative detected: '" + original + "' → picked '" + displayName + "'");
                    }
                    if (string.IsNullOrWhiteSpace(displayName)) continue;

                    data.Prefabs.Add(new PrefabEntry { PrefabName = displayName.Trim(), Position = coords.Value });
                    data.AlternativeLogMessages.Add("Prefab detected: " + displayName.Trim() + " at (" + coords.Value.X + "," + coords.Value.Y + ")");
                    continue;
                }

                // No coords: only flag missing-position when in Prefabs section and line looks like a prefab bullet
                if (LooksLikePrefabBullet(line))
                {
                    int idx = line.IndexOf('(');
                    string keyText = idx >= 0 ? line.Substring(0, idx) : line;
                    keyText = keyText.Trim().TrimStart('-').Trim();
                    int colonIdx = keyText.IndexOf(':');
                    if (colonIdx >= 0)
                        keyText = keyText.Substring(0, colonIdx).Trim();
                    keyText = keyText.Trim();
                    string keyNorm = NormalizeSpawnKey(keyText);
                    if (!IsSpawnKey(keyNorm) && keyNorm != "goal" && keyText.Length > 0)
                        data.MissingPositionPrefabNames.Add(keyText);
                }
            }

            return data;
        }

        /// <summary>
        /// Parse full level plan markdown and return blueprint data.
        /// </summary>
        public static BlueprintData ParseFullPlan(string fullMarkdown)
        {
            string section = GetBlueprintSection(fullMarkdown);
            return Parse(section ?? string.Empty);
        }
    }
}
