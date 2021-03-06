#nullable disable

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WWT.Providers
{
    [RequestEndpoint("/wwtweb/marsdem.aspx")]
    public class MarsdemProvider : RequestProvider
    {
        private readonly WwtOptions _options;

        public MarsdemProvider(WwtOptions options)
        {
            _options = options;
        }

        public override string ContentType => ContentTypes.OctetStream;

        public override async Task RunAsync(IWwtContext context, CancellationToken token)
        {
            string query = context.Request.Params["Q"];
            string[] values = query.Split(',');
            int level = Convert.ToInt32(values[0]);
            int tileX = Convert.ToInt32(values[1]);
            int tileY = Convert.ToInt32(values[2]);
            int demSize = 513 * 2;

            string filename = $@"{_options.WWTDEMDir}\toast\mars\Chunks\{level}\{tileY}.chunk";

            if (File.Exists(filename))
            {
                byte[] data = new byte[demSize];
                FileStream fs = File.OpenRead(filename);
                fs.Seek((long)(demSize * tileX), SeekOrigin.Begin);

                fs.Read(data, 0, demSize);
                fs.Close();
                await context.Response.OutputStream.WriteAsync(data, 0, demSize, token);
            }
            else
            {
                byte[] data = new byte[demSize];

                await context.Response.OutputStream.WriteAsync(data, 0, demSize, token);
            }

            context.Response.End();
        }
    }
}
