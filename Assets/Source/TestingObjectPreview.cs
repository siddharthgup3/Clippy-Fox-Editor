using UnityEditor;
using UnityEngine;

namespace Source
{
    [CustomEditor(typeof(TestingScriptableObject))]
    public class TestingObjectPreview : Editor
    {

        private PreviewRenderUtility _previewRenderUtility;

        private void Validate()
        {
            if (_previewRenderUtility == null)
            {
                _previewRenderUtility = new PreviewRenderUtility();
                _previewRenderUtility.camera.transform.position = new Vector3(0, 0, -5);
                _previewRenderUtility.camera.transform.rotation = Quaternion.identity;

            }
        }
        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            base.OnPreviewGUI(r, background);
        }
    }
}