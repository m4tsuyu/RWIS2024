using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(UIDocument))]
public class CustomVisualElementEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // ƒJƒXƒ^ƒ€VisualElement‚ð’Ç‰Á
        var container = new VisualElement();
        var customElement = new CustomCircle();
        container.Add(customElement);

        return container;
    }
}
