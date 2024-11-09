using NuGet.Protocol.Plugins;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace Victuz.Methods
{
    public class QrGeneration
    {
        public static byte[] GenerateQrFromGuid(string guid)
        {

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(guid, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var bitmap = qrCode.GetGraphic(20);
            using var ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }
    }


    public class ImageCache
    {
        private static readonly Dictionary<string, byte[]> _cache = new Dictionary<string, byte[]>();

        public static byte[] GetOrAdd(string key, Func<string, byte[]> generateImage)
        {
            if (!_cache.TryGetValue(key, out var image))
            {
                image = generateImage(key);
                _cache[key] = image;
            }
            return image;
        }
    }
}
