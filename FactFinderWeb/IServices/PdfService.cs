using DinkToPdf;
using DinkToPdf;
using DinkToPdf.Contracts;
using DinkToPdf.Contracts;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;
using System.IO;

namespace FactFinderWeb.IServices
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }
        public byte[] GeneratePdf1(string html, IWebHostEnvironment _env)
        {
            var document = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings
                    {
                        Top = 45,     // space for header image
                        Bottom = 30,  // space for footer
                        Left = 10,
                        Right = 10
                    }
                },
                Objects =
        {
            new ObjectSettings
            {
                HtmlContent = html,

                WebSettings = new WebSettings
                {
                    DefaultEncoding = "utf-8",
                    LoadImages = true,
                    EnableIntelligentShrinking = true

                },

                HeaderSettings = new HeaderSettings
                {
                  //  HtmUrl = "https://localhost:7010/header.html",
                    Spacing = 5,
                   HtmUrl = Path.Combine(_env.WebRootPath, "pdf", "header.html") 
                   //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "header.html"),
                },

                FooterSettings = new FooterSettings
                {

                    Spacing = 5,
                      HtmUrl  = Path.Combine(_env.WebRootPath, "pdf", "footer.html") 
                      //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "footer.html"),
                }
            }
        }
            };

            return _converter.Convert(document);
        }
        public byte[] GeneratePdf(string html, IWebHostEnvironment _env)
        {


            // Get absolute paths
            var headerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "header.html");
            var footerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "footer.html");


            // Convert paths to File URIs (e.g., file:///C:/...)
            string headerUri = new Uri(headerPath).AbsoluteUri;
            string footerUri = new Uri(footerPath).AbsoluteUri;
            var cssPath = Path.Combine(_env.WebRootPath, "css", "pdf-override.css");

            var document = new HtmlToPdfDocument
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                   
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings
                    {
                        Top = 28,
                        Bottom = 15,
                        Left = 0,
                        Right = 0
                    }
                },
                Objects = {
                new ObjectSettings
                {
                    HtmlContent = html,
                    WebSettings = { DefaultEncoding = "utf-8", LoadImages = true,UserStyleSheet = cssPath },
                    HeaderSettings = { HtmUrl = headerUri, Spacing = 5 },
                    FooterSettings = { HtmUrl = footerUri, Spacing = 5, Right = "Page [page] of [toPage]"}
                }
        }
            };

            return _converter.Convert(document);
        }


        public byte[] GeneratePdfsss(string html)
        {
            var document = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings
                    {
                        Top = 10,
                        Bottom = 10,
                        Left = 10,
                        Right = 10
                    }
                },
                Objects =
            {
                new ObjectSettings
                {
                    HtmlContent = html,
                    WebSettings = new WebSettings
                    {
                        DefaultEncoding = "utf-8",
                        LoadImages = true
                    }
                }
            }
            };

            return _converter.Convert(document);
        }
    }

}
