using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace PelliP3
{
    public class WindowUtils
    {
        public static TagLib.IPicture ConvertImageToTagLibPicture(System.Drawing.Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                TagLib.Picture picture = new TagLib.Picture
                {
                    Data = TagLib.ByteVector.FromStream(memoryStream),
                    MimeType = "image/png",
                    Type = TagLib.PictureType.FrontCover
                };

                return picture;
            }
        }
        public static System.Drawing.Image ConvertBytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            using (var ms = new MemoryStream(bytes))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }
    }
}
