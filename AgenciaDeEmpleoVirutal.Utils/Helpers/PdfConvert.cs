namespace AgenciaDeEmpleoVirutal.Utils.Helpers
{
    using DinkToPdf;

    public static class PdfConvert
    {
        public static byte[] Generatepdf(string documentContentHtml)
        {
            var convert = new BasicConverter(new PdfTools());
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
    }
}
