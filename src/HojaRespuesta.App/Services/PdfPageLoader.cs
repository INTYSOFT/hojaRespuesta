using System.Drawing;
using HojaRespuesta.App.Models;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PdfiumViewer;

namespace HojaRespuesta.App.Services;

public sealed class PdfPageLoader
{
    public IReadOnlyList<PageSource> Load(string filePath, int dpi = 300)
    {
        var pages = new List<PageSource>();
        using var document = PdfDocument.Load(filePath);

        for (int pageIndex = 0; pageIndex < document.PageCount; pageIndex++)
        {
            using var bitmap = (Bitmap)document.Render(pageIndex, dpi, dpi, true);
            using var matWithAlpha = BitmapConverter.ToMat(bitmap);
            using var mat = new Mat();
            Cv2.CvtColor(matWithAlpha, mat, ColorConversionCodes.BGRA2BGR);
            pages.Add(new PageSource(pageIndex + 1, mat.Clone()));
        }

        return pages;
    }
}
