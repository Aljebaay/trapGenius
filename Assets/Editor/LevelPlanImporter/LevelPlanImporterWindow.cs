using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Tilemaps;

namespace Devilbait.Editor
{
    public class LevelPlanImporterWindow : EditorWindow
    {
        private const string RootName = "__GENERATED_LEVEL";
        private const string GridName = "__GENERATED_GRID";
        private const string PlayerCloneName = "Mehdi's_little_guy";
        private const string LevelPlansDefaultPath = "Docs/LEVEL_PLANS";

        private string _levelPlanPath = "";
        private TextAsset _levelPlanAsset;
        private Vector2 _scroll;
        private string _lastLog = "";

        [MenuItem("Tools/Devilbait/Level Plan Importer")]
        public static void ShowWindow()
        {
            var win = GetWindow<LevelPlanImporterWindow>("Level Plan Importer");
            win.minSize = new Vector2(360, 200);
        }

        private void OnGUI()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Level Plan (LEVEL_###.md)", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _levelPlanAsset = (TextAsset)EditorGUILayout.ObjectField(_levelPlanAsset, typeof(TextAsset), false);
            if (_levelPlanAsset != null)
                _levelPlanPath = AssetDatabase.GetAssetPath(_levelPlanAsset);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
                BrowseForLevelPlan();
            EditorGUILayout.EndHorizontal();
            if (!string.IsNullOrEmpty(_levelPlanPath))
                EditorGUILayout.HelpBox(_levelPlanPath, MessageType.None);

            EditorGUILayout.Space(12);
            GUI.enabled = !string.IsNullOrEmpty(_levelPlanPath) && File.Exists(GetFullLevelPlanPath());
            if (GUILayout.Button("Build Level from Blueprint", GUILayout.Height(32)))
                BuildLevel();
            GUI.enabled = true;

            EditorGUILayout.Space(8);
            if (!string.IsNullOrEmpty(_lastLog))
            {
                EditorGUILayout.LabelField("Last run log", EditorStyles.boldLabel);
                EditorGUILayout.TextArea(_lastLog, GUILayout.ExpandHeight(true));
            }

            EditorGUILayout.EndScrollView();
        }

        private void BrowseForLevelPlan()
        {
            string projectRoot = Path.GetDirectoryName(Application.dataPath);
            string startPath = Path.Combine(projectRoot, LevelPlansDefaultPath);
            if (!Directory.Exists(startPath))
                startPath = projectRoot;

            string path = EditorUtility.OpenFilePanel("Select Level Plan (LEVEL_###.md)", startPath, "md");
            if (string.IsNullOrEmpty(path)) return;

            string relative = path;
            if (path.StartsWith(projectRoot))
                relative = path.Substring(projectRoot.Length).TrimStart(Path.DirectorySeparatorChar, '/');
            _levelPlanPath = relative.Replace('\\', '/');
            _levelPlanAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(_levelPlanPath);
            if (_levelPlanAsset == null && path.StartsWith(Application.dataPath))
            {
                string assetRelative = "Assets" + path.Substring(Application.dataPath.Length).Replace('\\', '/');
                _levelPlanPath = assetRelative;
                _levelPlanAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetRelative);
            }
        }

        private string GetFullLevelPlanPath()
        {
            if (string.IsNullOrEmpty(_levelPlanPath)) return "";
            if (File.Exists(_levelPlanPath)) return _levelPlanPath;
            string projectRoot = Path.GetDirectoryName(Application.dataPath);
            string combined = Path.Combine(projectRoot, _levelPlanPath);
            return File.Exists(combined) ? combined : _levelPlanPath;
        }

        private void BuildLevel()
        {
            string fullPath = GetFullLevelPlanPath();
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
            {
                EditorUtility.DisplayDialog("Level Plan Importer", "Level plan file not found.", "OK");
                return;
            }

            string markdown = File.ReadAllText(fullPath);
            var data = LevelPlanParser.ParseFullPlan(markdown);

            // --- Validation: abort on any failure without touching the scene ---
            if (!data.HasPlayerSpawn)
            {
                EditorUtility.DisplayDialog("Level Plan Importer", "Blueprint is missing Player spawn position. Provide one of: PlayerSpawn, Player, Mehdi's_little_guy.", "OK");
                _lastLog = "Aborted: Player spawn position missing from blueprint.";
                return;
            }
            if (!data.HasGoal)
            {
                EditorUtility.DisplayDialog("Level Plan Importer", "Blueprint is missing Goal:(x,y).", "OK");
                _lastLog = "Aborted: Goal missing from blueprint.";
                return;
            }
            if (data.MissingPositionPrefabNames != null && data.MissingPositionPrefabNames.Count > 0)
            {
                EditorUtility.DisplayDialog("Level Plan Importer", "Prefabs without position (x,y):\n" + string.Join("\n", data.MissingPositionPrefabNames), "OK");
                _lastLog = "Aborted: missing position for: " + string.Join(", ", data.MissingPositionPrefabNames);
                return;
            }

            var prefabNames = new System.Collections.Generic.List<string>();
            foreach (var p in data.Prefabs)
                prefabNames.Add(p.PrefabName);
            var missing = PrefabResolver.ValidatePrefabNames(prefabNames);
            if (missing.Count > 0)
            {
                EditorUtility.DisplayDialog("Level Plan Importer", "Missing prefabs:\n" + string.Join("\n", missing), "OK");
                _lastLog = "Aborted: missing prefabs: " + string.Join(", ", missing);
                return;
            }

            // --- All validation passed: create/clear root and build ---
            var log = new System.Text.StringBuilder();
            log.AppendLine("Build started from: " + fullPath);

            foreach (string msg in data.AlternativeLogMessages)
            {
                log.AppendLine(msg);
                Debug.Log(msg);
            }

            GameObject root = GameObject.Find(RootName);
            if (root == null)
            {
                root = new GameObject(RootName);
                log.AppendLine("Created root: " + RootName);
            }
            else
            {
                while (root.transform.childCount > 0)
                {
                    var child = root.transform.GetChild(0);
                    DestroyImmediate(child.gameObject);
                }
                log.AppendLine("Cleared children under " + RootName);
            }

            string goalPath = PrefabResolver.GetGoalPrefabPath();

            // Player
            Vector3 playerPos = data.PlayerSpawn.ToVector3();
            GameObject playerInScene = GameObject.Find(PlayerCloneName);
            if (playerInScene != null)
            {
                playerInScene.transform.position = playerPos;
                log.AppendLine("Moved existing player to " + playerPos);
            }
            else
            {
                string playerPath = PrefabResolver.GetPlayerPrefabPath();
                var playerPrefab = PrefabResolver.LoadPrefab(playerPath);
                if (playerPrefab != null)
                {
                    var playerInstance = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
                    playerInstance.transform.position = playerPos;
                    playerInstance.name = PlayerCloneName;
                    log.AppendLine("Instantiated player at " + playerPos);
                }
                else
                    log.AppendLine("Warning: Player prefab not found at " + playerPath);
            }

            // Goal
            var goalPrefab = PrefabResolver.LoadPrefab(goalPath);
            if (goalPrefab != null)
            {
                var goalInstance = (GameObject)PrefabUtility.InstantiatePrefab(goalPrefab);
                PrefabUtility.UnpackPrefabInstance(goalInstance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                goalInstance.transform.SetParent(root.transform, true);
                goalInstance.transform.position = data.Goal.ToVector3();
                log.AppendLine("Instantiated Goal at " + data.Goal.ToVector3());
            }
            else
                log.AppendLine("Warning: Goal prefab not found at " + goalPath);

            // Trap prefabs (skip if entry resolves to Goal — Goal already placed above)
            foreach (var entry in data.Prefabs)
            {
                string path = PrefabResolver.ResolvePrefabPath(entry.PrefabName);
                if (string.IsNullOrEmpty(path)) continue;

                if (path == goalPath)
                {
                    log.AppendLine("Skipped duplicate Goal: " + entry.PrefabName);
                    continue;
                }

                var prefab = PrefabResolver.LoadPrefab(path);
                if (prefab == null)
                {
                    log.AppendLine("Skip (load failed): " + entry.PrefabName);
                    continue;
                }

                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                instance.transform.SetParent(root.transform, true);
                instance.transform.position = entry.Position.ToVector3();
                log.AppendLine("Instantiated " + instance.name + " at " + entry.Position.ToVector3());
            }

            // --- Grid for Tilemap-based prefabs (Goal, Death Zone, etc.) ---
            Transform gridTransform = root.transform.Find(GridName);
            if (gridTransform == null)
            {
                var gridGo = new GameObject(GridName);
                gridGo.AddComponent<Grid>();
                gridGo.transform.SetParent(root.transform, false);
                gridGo.transform.localPosition = Vector3.zero;
                gridTransform = gridGo.transform;
                log.AppendLine("Created " + GridName + " with Grid component.");
            }
            else if (gridTransform.GetComponent<Grid>() == null)
            {
                gridTransform.gameObject.AddComponent<Grid>();
                log.AppendLine("Added Grid component to existing " + GridName + ".");
            }

            var directChildren = new List<Transform>();
            for (int c = 0; c < root.transform.childCount; c++)
                directChildren.Add(root.transform.GetChild(c));

            foreach (Transform t in directChildren)
            {
                if (t.name == GridName) continue;
                var tilemap = t.GetComponent<Tilemap>() ?? t.GetComponentInChildren<Tilemap>(true);
                if (tilemap != null)
                {
                    Vector3 worldPos = t.position;
                    t.SetParent(gridTransform, true);
                    t.position = worldPos;
                    log.AppendLine("Reparented Tilemap object under __GENERATED_GRID: " + t.name);
                }
            }

            // --- Debug markers for editor visibility (no SpriteRenderer in hierarchy) ---
            AddDebugMarkersToPlacedObjects(root, gridTransform, log);

            _lastLog = log.ToString();
            Debug.Log(_lastLog);

            if (root != null && !Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        private static void AddDebugMarkersToPlacedObjects(GameObject root, Transform gridTransform, System.Text.StringBuilder log)
        {
            var toCheck = new List<GameObject>();

            for (int c = 0; c < root.transform.childCount; c++)
            {
                var child = root.transform.GetChild(c).gameObject;
                if (child.name != GridName)
                    toCheck.Add(child);
            }
            if (gridTransform != null)
            {
                for (int c = 0; c < gridTransform.childCount; c++)
                    toCheck.Add(gridTransform.GetChild(c).gameObject);
            }

            Texture2D whiteTex = null;
            foreach (GameObject obj in toCheck)
            {
                if (obj.GetComponentInChildren<SpriteRenderer>(true) != null) continue;

                if (whiteTex == null)
                {
                    whiteTex = new Texture2D(1, 1);
                    whiteTex.SetPixel(0, 0, Color.white);
                    whiteTex.Apply();
                }

                var marker = new GameObject("__DEBUG_MARKER");
                marker.transform.SetParent(obj.transform, false);
                marker.transform.localPosition = Vector3.zero;
                marker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                var sr = marker.AddComponent<SpriteRenderer>();
                sr.sprite = Sprite.Create(whiteTex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
                sr.sortingOrder = 1000;
            }
        }
    }
}
