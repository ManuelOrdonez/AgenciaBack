namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using DinkToPdf;
    using DinkToPdf.Contracts;
    using System;

    public class PdfConvert
    {
        private IConverter _converter;

        public PdfConvert(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePDF(string documentContentHtml)
        {
            try
            {
                return _converter.Convert(new HtmlToPdfDocument
                {
                    Objects =
                    {
                        new ObjectSettings
                        {
                            HtmlContent = documentContentHtml
                        }
                    }
                });
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}
