#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using WWTWebservices;

namespace WWT.Providers
{
    [RequestEndpoint("/wwtweb/GalexToast.aspx")]
    public class GalexToastProvider : RequestProvider
    {
        private readonly IPlateTilePyramid _plateTiles;
        private readonly WwtOptions _options;

        public GalexToastProvider(IPlateTilePyramid plateTiles, WwtOptions options)
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

            if (level > 10)
            {
                context.Response.Clear();
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("No image", token);
                context.Response.End();
                return;
            }

            if (level < 9)
            {
                try
                {
                    context.Response.ContentType = "image/png";

                    using (var s = await _plateTiles.GetStreamAsync(_options.WwtTilesDir, "GalexBoth_L0to8_x0_y0.plate", level, tileX, tileY, token))
                    {
                        await s.CopyToAsync(context.Response.OutputStream, token);
                        context.Response.Flush();
                        context.Response.End();
                        return;
                    }
                }
                catch
                {
                    context.Response.Clear();
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("No image", token);
                    context.Response.End();
                    return;
                }
            }
            else
            {
                try
                {
                    int powLev3Diff = (int)Math.Pow(2, level - 3);
                    int X8 = tileX / powLev3Diff;
                    int Y8 = tileY / powLev3Diff;

                    int L3 = level - 3;
                    int X3 = tileX % powLev3Diff;
                    int Y3 = tileY % powLev3Diff;

                    context.Response.ContentType = "image/png";

                    using (var s = await _plateTiles.GetStreamAsync(_options.WwtGalexDir, $"GalexBoth_L3to10_x{X8}_y{Y8}.plate", L3, X3, Y3, token))
                    {
                        await s.CopyToAsync(context.Response.OutputStream, token);
                        context.Response.Flush();
                        context.Response.End();
                        return;
                    }
                }
                catch
                {
                    context.Response.Clear();
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("No image", token);
                    context.Response.End();
                    return;
                }
            }
        }
    }
}
