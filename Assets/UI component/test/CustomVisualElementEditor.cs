using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(UIDocument))]
public class CustomVisualElementEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // カスタムVisualElementを追加
        var container = new VisualElement();
        var customElement = new CustomCircle();
        container.Add(customElement);

        return container;
    }
}
