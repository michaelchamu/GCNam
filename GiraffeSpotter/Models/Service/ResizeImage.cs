using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GiraffeSpotter.Models.Service
{
    /// <summary>
    /// Class Content Retrieved From (http://i-skool.co.uk/resize-image-class-in-c-source-code/)
    /// Content Customized by Donovan Maasz on 2014-05-17 (maaszdonovan@gmail.com)
    /// </summary>
    public static class ResizeImage
    {

        public static byte[] ResizeToBase64(byte[] image, int toWidth, int toHeight)
        {
            // Set holder for image.
            Image tmpImage = null;
            byte[] resizedImage = null;

            // First byte[] to image.
            MemoryStream ms = new MemoryStream(image);
            tmpImage = Image.FromStream(ms);

            // See if image has been downloaded.
            if (tmpImage != null)
            {
                // Convert image into Base64 Encoded data.
                resizedImage = Resize(tmpImage, new Size { Width = toWidth, Height = toHeight });

                // Convert resized image to base64.
                return resizedImage;
            }
            else
            {
                // No image. Return default.
                // Generate Random Number.
                return null;
            }
        }

        private static Image ScaleImage(Image srcImage, int newWidth, int newHeight)
        {
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.None;
                gr.InterpolationMode = InterpolationMode.Low;
                gr.PixelOffsetMode = PixelOffsetMode.None;
                gr.CompositingQuality = CompositingQuality.Invalid;
                gr.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
            }

            return newImage;
        }

        private static byte[] Resize(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            byte[] img_bytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                System.Drawing.Imaging.EncoderParameter qualityParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;
                System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                b.Save(ms, jgpEncoder, encoderParams);
                img_bytes = ms.ToArray();
            }

            return img_bytes;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
