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

        // “h‚è‚Â‚Ô‚µ‚ÌF‚ğİ’è
        painter.fillColor = Color.green;

        painter.BeginPath();
        painter.MoveTo(new Vector2(100, 100)); // 100, 100‚ÉˆÚ“®
        painter.LineTo(new Vector2(200, 200)); // 200, 200‚Ü‚Å’¼ü‚ÅƒpƒX‚ğŒq‚®
        painter.LineTo(new Vector2(300, 100)); // 300, 100‚Ü‚Å’¼ü‚ÅƒpƒX‚ğŒq‚®

        // “h‚è‚Â‚Ô‚·
        painter.Fill();
    }
}
