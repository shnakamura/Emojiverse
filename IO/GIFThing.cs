using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Emojiverse.IO;

public sealed class GIFThing
{
    public static void Hamburger(string path) {
        using var stream = new FileStream(path, FileMode.Open);
        using var image = Image.FromStream(stream);

        var data = GetFrameData(image);
        
        for (int i = 0; i < data.FrameCount; i++) {
            image.SelectActiveFrame(data.Dimension, i);
        }
    }

    private static (FrameDimension Dimension, int FrameCount) GetFrameData(Image image) {
        var dimensions = image.FrameDimensionsList;
        var frameCount = 1;

        foreach (var id in dimensions) {
            var dimension = new FrameDimension(id);
            var dimensionFrameCount = image.GetFrameCount(dimension);
            
            if (dimensionFrameCount > 0) {
                return (dimension, frameCount);
            }
        }
        
        return (null, frameCount);
    }
}
