using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AJLibrary
{

    public class IconConverter
    {

        public static void SaveIconWithMultipleSizes(string sourceImagePath, string outputIconPath, int[] sizes)
        {
            using (MemoryStream iconStream = new MemoryStream())
            {
                using (BinaryWriter iconWriter = new BinaryWriter(iconStream))
                {
                    // 写入图标文件头
                    iconWriter.Write((short)0); // Reserved
                    iconWriter.Write((short)1); // Type, 1 = Icon
                    iconWriter.Write((short)sizes.Length); // 图像数量

                    int imageOffset = 6 + (16 * sizes.Length); // 计算图像数据的偏移量

                    // 写入每个图像的目录信息
                    foreach (int size in sizes)
                    {
                        using (Bitmap resizedBitmap = new Bitmap(Image.FromFile(sourceImagePath), new Size(size, size)))
                        {
                            using (MemoryStream imageStream = new MemoryStream())
                            {
                                resizedBitmap.Save(imageStream, ImageFormat.Png);

                                iconWriter.Write((byte)size); // 图像宽度
                                iconWriter.Write((byte)size); // 图像高度
                                iconWriter.Write((byte)0);    // Color Palette
                                iconWriter.Write((byte)0);    // Reserved
                                iconWriter.Write((short)1);   // Color Planes
                                iconWriter.Write((short)32);  // Bits per Pixel
                                iconWriter.Write((int)imageStream.Length); // 图像数据大小
                                iconWriter.Write(imageOffset); // 图像数据的偏移量

                                imageOffset += (int)imageStream.Length;
                            }
                        }
                    }

                    // 写入每个图像的数据
                    foreach (int size in sizes)
                    {
                        using (Bitmap resizedBitmap = new Bitmap(Image.FromFile(sourceImagePath), new Size(size, size)))
                        {
                            using (MemoryStream imageStream = new MemoryStream())
                            {
                                resizedBitmap.Save(imageStream, ImageFormat.Png);
                                iconWriter.Write(imageStream.ToArray()); // 图像数据
                            }
                        }
                    }

                    // 将内存中的图标数据写入到文件
                    File.WriteAllBytes(outputIconPath, iconStream.ToArray());
                }
            }
        }
    }

}
