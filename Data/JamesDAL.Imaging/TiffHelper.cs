using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace James.Data.Imaging;

/// <summary>
///     A small helper class to handle TIFF files
/// </summary>
/// <remarks>Adapted from https://github.com/Darkseal/MergeTIFF.git</remarks>
[SupportedOSPlatform("windows")]
public static class TiffHelper
{
    /// <summary>
    ///     Merges multiple TIFF files (including multipage TIFFs) into a single multipage TIFF file.
    /// </summary>
    /// <remarks>Output must be disposed after using.</remarks>
    public static MemoryStream MergeTiffToStream(params byte[][] tiffFiles)
    {
        return MergeTiffToStream(tiffFiles.Select(f => new MemoryStream(f)));
    }

    /// <summary>
    ///     Merges multiple TIFF files (including multipage TIFFs) into a single multipage TIFF file.
    /// </summary>
    /// <remarks>Output must be disposed after using.</remarks>
    public static MemoryStream MergeTiffToStream(IEnumerable<Image> tiffImages)
    {
        var msMerge = new MemoryStream();
        //get the codec for tiff files
        var ici = ImageCodecInfo.GetImageEncoders().First(ie=>ie.MimeType=="image/tiff");

        var enc = Encoder.SaveFlag;
        var ep = new EncoderParameters(1);

        Bitmap pages = null;
        var frame = 0;

        foreach (var tiffImage in tiffImages)
        {
            //using var imageStream = new MemoryStream(tiffFile);
            //using var tiffImage = Image.FromStream(tiffFile);
            foreach (var guid in tiffImage.FrameDimensionsList)
            {
                //create the frame dimension 
                var dimension = new FrameDimension(guid);
                //Gets the total number of frames in the .tiff file 
                var noOfPages = tiffImage.GetFrameCount(dimension);

                for (var index = 0; index < noOfPages; index++)
                {
                    var currentFrame = new FrameDimension(guid);
                    tiffImage.SelectActiveFrame(currentFrame, index);
                    using var tempImg = new MemoryStream();
                    tiffImage.Save(tempImg, ImageFormat.Tiff);
                    {
                        if (frame == 0)
                        {
                            //save the first frame
                            pages = (Bitmap) Image.FromStream(tempImg);
                            ep.Param[0] = new EncoderParameter(enc, (long) EncoderValue.MultiFrame);
                            pages.Save(msMerge, ici, ep);
                        }
                        else
                        {
                            //save the intermediate frames
                            ep.Param[0] = new EncoderParameter(enc,
                                (long) EncoderValue.FrameDimensionPage);
                            pages.SaveAdd((Bitmap) Image.FromStream(tempImg), ep);
                        }
                    }
                    frame++;
                }
            }
            tiffImage.Dispose();//Must dispose input streams after use.
        }

        if (frame > 0)
        {
            //flush and close.
            ep.Param[0] = new EncoderParameter(enc, (long) EncoderValue.Flush);
            pages.SaveAdd(ep);
        }
        msMerge.Position = 0;
        return msMerge;
        }

    /// <summary>
    ///     Merges multiple TIFF files (including multipage TIFFs) into a single multipage TIFF file.
    /// </summary>
    /// <remarks>Output must be disposed after using.</remarks>
    public static MemoryStream MergeTiffToStream(IEnumerable<Stream> tiffFiles)
    {
        List<Image> tiffImages = new();
        try
        {
            tiffImages.AddRange(tiffFiles.Select(tf =>
            {
                var img = Image.FromStream(tf);
                tf.Dispose();
                return img;
            }));
            return MergeTiffToStream(tiffImages);
        }
        finally
        {
            foreach (var ti in tiffImages)
                ti.Dispose();
        }
    }

    /// <summary>
    ///     Merges multiple TIFF files (including multipage TIFFs) into a single multipage TIFF file.
    /// </summary>
    public static byte[] MergeTiff(params byte[][] tiffFiles)
    {
        MemoryStream? tiffStream = null;
        try
        {
            tiffStream = MergeTiffToStream(tiffFiles);
            return tiffStream.ToArray();
        }
        finally
        {
            tiffStream?.Dispose();
        }
    }
}