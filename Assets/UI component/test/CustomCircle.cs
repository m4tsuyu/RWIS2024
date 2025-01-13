using UnityEngine;
using UnityEngine.UIElements;

public sealed class CustomCircle : VisualElement
{
    public CustomCircle()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    private static void OnGenerateVisualContent(MeshGenerationContext context)
    {
        var painter = context.painter2D;

        // �h��Ԃ��̐F��ݒ�
        painter.fillColor = Color.green;

        painter.BeginPath();
        painter.MoveTo(new Vector2(100, 100)); // 100, 100�Ɉړ�
        painter.LineTo(new Vector2(200, 200)); // 200, 200�܂Œ����Ńp�X���q��
        painter.LineTo(new Vector2(300, 100)); // 300, 100�܂Œ����Ńp�X���q��

        // �h��Ԃ�
        painter.Fill();
    }
}
