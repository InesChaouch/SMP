using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using BaselineTypeDiscovery;
using DinkToPdf;

namespace AutoPost.Application.Common.Helpers.Pdf
{
    class PdfService
    {
        private readonly SynchronizedConverter _converter;

        public PdfService()
        {
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            _converter = new SynchronizedConverter(new PdfTools());
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
            },
                Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
            };
            return _converter.Convert(doc);
        }
    }
}
