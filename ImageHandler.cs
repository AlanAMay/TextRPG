using System.Diagnostics;

namespace TextRPGOne
{
    partial class Program
    {
        class ImageHandler
        {
            public static void DisplayAsciiLocation(string imagePath)
            {
                Process.Start("ascii-image-converter", $"{imagePath} -C -b -f")?.WaitForExit();
            }
        }
    }
}