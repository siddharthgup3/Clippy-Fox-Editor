using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class MyEditorWindow : EditorWindow
    {
        private static PreviewRenderUtility _previewRenderUtility;
        private static GameObject _prefab;

        private static Vector2 _drag;

        private static GameObject instance;
        private static Animator _animator;

        private static double previewStartTime;
        private static double lastTime;

        [MenuItem("MyMenu/ShowWindow")]
        public static void Init()
        {
            _previewRenderUtility = null;
            MyEditorWindow window = GetWindow<MyEditorWindow>();
            window.titleContent = new GUIContent("Foxy");
            window.Show();
            Setup();
        }

        private void Awake()
        {
            EditorApplication.update += MyFunc;
        }

        private static void Setup()
        {
            if (_previewRenderUtility == null)
                _previewRenderUtility = new PreviewRenderUtility();

            var transform = _previewRenderUtility.camera.transform;

            transform.position = new Vector3(0, 0, 10);
            transform.rotation = Quaternion.Euler(0, 180, 0);

            _prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Fox/Prefabs/Fox.prefab");

            instance = Instantiate(_prefab);
           // instance.hideFlags = HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;

            previewStartTime = Time.realtimeSinceStartup;
            lastTime = Time.realtimeSinceStartup;
        }

        public static Rect rect;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void MyFunc()
        {
            Repaint();
        }
        
        public void OnGUI()
        {
            _animator = instance.GetComponent<Animator>();

            
            rect = position;

            _animator.Update((float)Time.realtimeSinceStartup - (float)lastTime);

            lastTime = Time.realtimeSinceStartup;
            _previewRenderUtility.BeginPreview(rect, new GUIStyle());


            Mesh mesh = new Mesh();
            instance.GetComponentInChildren<SkinnedMeshRenderer>().BakeMesh(mesh);
            _previewRenderUtility.DrawMesh(mesh, Matrix4x4.identity,
                instance.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial, 0);

            _previewRenderUtility.camera.Render();

            Texture outputTexture = _previewRenderUtility.EndPreview();

            GUI.DrawTexture(rect, outputTexture, ScaleMode.StretchToFill, false);
            if (GUILayout.Button("Attack"))
            {
                _animator.SetTrigger(Attack);
            }
            if (GUILayout.Button("Jump"))
            {
                _animator.SetTrigger("Jump");
            }
            if (GUILayout.Button("Somersault"))
            {
                _animator.SetTrigger("Somersault");
            }
            if (GUILayout.Button("Walk"))
            {
                _animator.SetTrigger("Walk");
            }
            if (GUILayout.Button("Sit"))
            {
                _animator.SetTrigger("Sit");
            }
            _drag = Drag2D(_drag, rect);

        }

        public void OnDestroy()
        {
            DestroyImmediate(instance);
            _previewRenderUtility.Cleanup();
        }

        public static Vector2 Drag2D(Vector2 scrollPosition, Rect position)
        {
            int controlID = GUIUtility.GetControlID("Slider".GetHashCode(), FocusType.Passive);
            Event current = Event.current;
            switch (current.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                    if (position.Contains(current.mousePosition) && position.width > 50f)
                    {
                        GUIUtility.hotControl = controlID;
                        current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }

                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                    }

                    EditorGUIUtility.SetWantsMouseJumping(0);
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        scrollPosition -= current.delta * (float) ((!current.shift) ? 1 : 3) /
                            Mathf.Min(position.width, position.height) * 140f;
                        scrollPosition.y = Mathf.Clamp(scrollPosition.y, -90f, 90f);
                        current.Use();
                        GUI.changed = true;
                    }

                    break;
            }

            return scrollPosition;
        }
    }
}