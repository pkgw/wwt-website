using NSubstitute;
using System;
using System.IO;
using WWTWebservices;

namespace WWT.Providers.Tests
{
    public class DSSTests : ProviderTests<DSSProvider>
    {
        protected override int MaxLevel => 12;

        protected override Action<IResponse> StreamExceptionResponseHandler => null;

        protected override Action<IResponse> NullStreamResponseHandler => null;

        protected override Stream GetStreamFromPlateTilePyramid(IPlateTilePyramid plateTiles, int level, int x, int y)
        {
            if (level > 12)
            {
                return null;
            }
            else if (level < 8)
            {
                return plateTiles.GetStream(Options.WwtTilesDir, "dssterrapixel.plate", level, x, y);
            }
            else
            {
                var powLev5Diff = (int)Math.Pow(2, level - 5);
                var X32 = x / powLev5Diff;
                var Y32 = y / powLev5Diff;

                var L5 = level - 5;
                var X5 = x % powLev5Diff;
                var Y5 = y % powLev5Diff;

                return plateTiles.GetStream(Options.DssTerapixelDir, $"DSSpngL5to12_x{X32}_y{Y32}.plate", L5, X5, Y5);
            }
        }

        protected override void ExpectedResponseAboveMaxLevel(IResponse response)
        {
            response.Received(1).Write("No image");
        }
    }
}
