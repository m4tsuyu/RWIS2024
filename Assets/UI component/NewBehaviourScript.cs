using UnityEngine;
using UnityEngine.UIElements;

public sealed class ExampleElement : VisualElement
{
    public ExampleElement()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    private static void OnGenerateVisualContent(MeshGenerationContext context)
    {
        var painter = context.painter2D;

        // 塗りつぶしの色を設定
        painter.fillColor = Color.green;

        painter.BeginPath();
        painter.MoveTo(new Vector2(100, 100)); // 100, 100に移動
        painter.LineTo(new Vector2(200, 200)); // 200, 200まで直線でパスを繋ぐ
        painter.LineTo(new Vector2(300, 100)); // 300, 100まで直線でパスを繋ぐ

        // 塗りつぶす
        painter.Fill();
    }
}
