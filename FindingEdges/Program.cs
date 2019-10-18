using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindingEdges
{
    class Program
    {
        public static Bitmap ToGreyScale(Bitmap bmp, string inputImage, string outputSource)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            Color p;
            Process.Start(inputImage);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    p = bmp.GetPixel(x, y);

                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    int avg = (r + g + b) / 3;

                    bmp.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                }
            }

            bmp.Save(outputSource);
            Process.Start(outputSource);
            return bmp;
        }

        public static Bitmap EdgeDetection(short[,] vector, Bitmap bmp)
        {
            Bitmap outputImage = new Bitmap(bmp);

            for (int i = 1; i < bmp.Width-1; i++)
            {
                for (int j = 1; j < bmp.Height-1; j++)
                {
                    byte r1 = (byte)(bmp.GetPixel(i - 1, j - 1).R*vector[0,0]);
                    byte r2 = (byte)(bmp.GetPixel(i - 1, j).R*vector[0,1]);
                    byte r3 = (byte)(bmp.GetPixel(i - 1, j + 1).R*vector[0,2]);

                    byte r4 = (byte)(bmp.GetPixel(i, j - 1).R * vector[1, 0]);
                    byte r5 = (byte)(bmp.GetPixel(i, j).R * vector[1, 1]);
                    byte r6 = (byte)(bmp.GetPixel(i, j + 1).R * vector[1, 2]);

                    byte r7 = (byte)(bmp.GetPixel(i + 1, j - 1).R * vector[2, 0]);
                    byte r8 = (byte)(bmp.GetPixel(i + 1, j).R * vector[2, 1]);
                    byte r9 = (byte)(bmp.GetPixel(i + 1, j + 1).R * vector[2, 2]);

                    
                    byte R = (byte)((r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8 + r9) / 9);

                    outputImage.SetPixel(i, j, Color.FromArgb(R,R,R));
                }
            }


            return outputImage;
        }

        public static Bitmap FinalProcess(Bitmap imgX, Bitmap imgY)
        {
            Bitmap outputImg = new Bitmap(imgX);

            for (int i = 0; i < imgX.Width; i++)
            {
                for (int j = 0; j < imgX.Height; j++)
                {
                    int final = (int)Math.Sqrt(Math.Pow(imgX.GetPixel(i, j).R, 2)+Math.Pow(imgY.GetPixel(i,j).R,2));

                    if (final > 127)
                        final = 255;
                    if (final <= 127)
                        final = 0;

                    outputImg.SetPixel(i, j, Color.FromArgb(final, final, final));
                }
            }

            return outputImg;
        }

        static void Main(string[] args)
        {
            string inputImage = @"C:\Users\Soma\Pictures\input.jpg";
            string outputImage = @"C:\Users\Soma\Pictures\output.jpg";
            Bitmap bmp = new Bitmap(inputImage);
            bmp = ToGreyScale(bmp, inputImage, outputImage);

            short[,] Gx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            short[,] Gy = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            Bitmap edgesX = EdgeDetection(Gx, bmp);
            Bitmap edgesY = EdgeDetection(Gy, bmp);
            edgesX.Save(@"C:\Users\Soma\Pictures\outputX.jpg");
            edgesY.Save(@"C:\Users\Soma\Pictures\outputY.jpg");

            Bitmap final = FinalProcess(edgesX, edgesY);
            final.Save(@"C:\Users\Soma\Pictures\final.jpg");

        }
    }
}
