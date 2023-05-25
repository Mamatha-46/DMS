using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace DMS.BAL
{
    public class PdfGeneration
    {
        public void WriteToPdf(string pdfFile,string GeneratedPdfFile)
        {
            string oldFile = @"C:\\Images\\R1.pdf";
            string newFile = @"C:\\Images\\R11.pdf";
            string TitleSignFile = @"C:\\Images\\sign.png";
            string NameSignFile = @"C:\\Images\\Title.png";

            using (var reader1 = new PdfReader(oldFile))
            {
                using (var fileStream = new FileStream(newFile, FileMode.Create, FileAccess.Write))
                {
                    var document1 = new Document(reader1.GetPageSizeWithRotation(1));
                    var writer1 = PdfWriter.GetInstance(document1, fileStream);

                    document1.Open();

                    for (var i = 1; i <= reader1.NumberOfPages; i++)
                    {
                        document1.NewPage();
                        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var importedPage = writer1.GetImportedPage(reader1, i);

                        var cbx = writer1.DirectContent;
                        if (i == 8)
                        {
                            cbx.BeginText();
                            cbx.SetFontAndSize(baseFont, 12);

                            BaseFont bfx = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            cbx.SetColorFill(BaseColor.DARK_GRAY);
                            cbx.SetFontAndSize(bfx, 8);

                            iTextSharp.text.Image namesign1 = iTextSharp.text.Image.GetInstance(NameSignFile);
                            namesign1.ScaleToFit(100f, 70f);
                            namesign1.SpacingBefore = 10f;
                            namesign1.SetAbsolutePosition(110, 505);
                            namesign1.SpacingAfter = 1f;
                            namesign1.Alignment = Element.ALIGN_CENTER;
                            cbx.AddImage(namesign1);
                            iTextSharp.text.Image titlesign1 = iTextSharp.text.Image.GetInstance(TitleSignFile);
                            titlesign1.ScaleToFit(70f, 70f);
                            titlesign1.SpacingBefore = 10f;
                            titlesign1.SetAbsolutePosition(90, 460);
                            titlesign1.SpacingAfter = 1f;
                            titlesign1.Alignment = Element.ALIGN_CENTER;
                            cbx.AddImage(titlesign1);
                            cbx.EndText();
                        }


                        if (i == 11)
                        {

                            var example_html_1 = @"<p>This
                                                    <em>is</em>
                                                    <span class=""
                                                          headline""
                                                          style=""text-decoration: underline;"">some</span>
                                                    <strong>sample<em>text</em>
                                                    </strong>
                                                    <span style=""color: red;"">!!!</span>
                                                </p>";
                            var example_html_2 = @"<p>This
                                                    <em>is</em>
                                                    <span class=""
                                                          headline""
                                                          style=""text-decoration: underline;"">some</span>
                                                    <strong>New sample
                                                        <em>text</em></strong>
                                                    <span style=""color: red;"">!!!</span>
                                                </p>";
                            Font f = new Font(baseFont, 8);
                            List<IElement> htmlarraylist_1 = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(example_html_1), null);
                            ColumnText ct1 = new ColumnText(cbx);
                            ct1.SetSimpleColumn(document1.Left, 620f, 220f, 100f, 11f, 2);
                            foreach (IElement e in htmlarraylist_1)
                            {
                                ct1.AddElement(e);
                            }
                            ct1.Go();
                            List<IElement> htmlarraylist_2 = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(example_html_2), null);
                            ColumnText ct2 = new ColumnText(cbx);
                            ct2.SetSimpleColumn(document1.Left, 590f, 220f, 100f, 11f, 3);
                            foreach (IElement e in htmlarraylist_2)
                            {
                                ct2.AddElement(e);
                            }
                            ct2.Go();
                        }
                        cbx.AddTemplate(importedPage, 0, 0);
                    }

                    document1.Close();
                    writer1.Close();
                }
            }
            
        }
    }
}
