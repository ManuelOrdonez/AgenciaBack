namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using DinkToPdf;
    using System;

    public static class PdfConvert
    {
        public static byte[] GeneratePDF(string documentContentHtml)
        {
            var convert = new BasicConverter(new PdfTools());
            try
            {
                return convert.Convert(new HtmlToPdfDocument()
                {
                    Objects =
                    {
                        new ObjectSettings()
                        {
                            HtmlContent = documentContentHtml
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }        
    }
}
