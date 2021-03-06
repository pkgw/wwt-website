#nullable disable

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WWTWebservices;

namespace WWT.Providers
{
    [RequestEndpoint("/wwtweb/MarsHirise.aspx")]
    public class MarsHiriseProvider : RequestProvider
    {
        private readonly IPlateTilePyramid _plateTiles;
        private readonly WwtOptions _options;

        public MarsHiriseProvider(IPlateTilePyramid plateTiles, WwtOptions options)
        {
            _plateTiles = plateTiles;
            _options = options;
        }

        public override string ContentType => ContentTypes.Png;

        public override async Task RunAsync(IWwtContext context, CancellationToken token)
        {
            string query = context.Request.Params["Q"];
            string[] values = query.Split(',');
            int level = Convert.ToInt32(values[0]);
            int tileX = Convert.ToInt32(values[1]);
            int tileY = Convert.ToInt32(values[2]);

            var id = values.Length > 3 ? Convert.ToInt32(values[3]) : -1;

            if (level > 17)
            {
                context.Response.StatusCode = 404;
                return;
            }

            using (Bitmap output = new Bitmap(256, 256))
            {
                using (Graphics g = Graphics.FromImage(output))
                {
                    int ll = level;
                    int xx = tileX;
                    int yy = tileY;

                    if (ll > 8)
                    {
                        int levelDif = ll - 8;
                        int scale = (int)Math.Pow(2, levelDif);
                        int tx = xx / scale;
                        int ty = yy / scale;

                        int offsetX = (xx - (tx * scale)) * (256 / scale);
                        int offsetY = (yy - (ty * scale)) * (256 / scale);
                        float width = Math.Max(2, (256 / scale));
                        float height = width;
                        if ((width + offsetX) >= 255)
                        {
                            width -= 1;
                        }
                        if ((height + offsetY) >= 255)
                        {
                            height -= 1;
                        }

                        using (var stream = await _plateTiles.GetStreamAsync(_options.WwtTilesDir, "marsbasemap.plate", -1, 8, tx, ty, token))
                        using (var bmp1 = new Bitmap(stream))
                        {
                            g.DrawImage(bmp1, new RectangleF(0, 0, 256, 256), new RectangleF(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
                        }
                    }
                    else
                    {
                        using (var stream = await _plateTiles.GetStreamAsync(_options.WwtTilesDir, "marsbasemap.plate", -1, ll, xx, yy, token))
                        using (var bmp1 = new Bitmap(stream))
                        {
                            g.DrawImageUnscaled(bmp1, new Point(0, 0));
                        }
                    }

                    try
                    {
                        using (var stream = await LoadHiRiseAsync(ll, xx, yy, id, token))
                        {
                            if (stream != null)
                            {
                                using (var bmp2 = new Bitmap(stream))
                                {
                                    g.DrawImageUnscaled(bmp2, new Point(0, 0));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                await output.SaveAsync(context.Response, ImageFormat.Png, token);
            }
        }

        private Task<Stream> LoadHiRiseAsync(int level, int tileX, int tileY, int id, CancellationToken token)
        {
            UInt32 index = ComputeHash(level, tileX, tileY) % 300;

            return _plateTiles.GetStreamAsync("https://marsstage.blob.core.windows.net/hirise", $"hiriseV5_{index}.plate", id, level, tileX, tileY, token);
        }

        private UInt32 ComputeHash(int level, int x, int y)
        {
            return DirectoryEntry.ComputeHash(level + 128, x, y);
        }
    }
}
