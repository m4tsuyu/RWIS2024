using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(UIDocument))]
public class CustomVisualElementEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // �J�X�^��VisualElement��ǉ�
        var container = new VisualElement();
        var customElement = new CustomCircle();
        container.Add(customElement);

        return container;
    }
}
