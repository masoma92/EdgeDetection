﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindingEdges
{
    class ParallelEdgeFinding
    {
        static public object o = new object();

        public ParallelEdgeFinding()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string inputImage = @"C:\Users\Soma\Pictures\input.jpg";
            string outputImage = @"C:\Users\Soma\Pictures\greyscale_parallel.jpg";
            Bitmap bmp = new Bitmap(inputImage);
            bmp = new Bitmap(ToGreyScale(bmp, outputImage));

            short[,] Gx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            short[,] Gy = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            short[,] edgesX = new short[bmp.Width, bmp.Height];
            short[,] edgesY = new short[bmp.Width, bmp.Height];

            EdgeDetection(Gx, bmp, edgesX);
            EdgeDetection(Gy, bmp, edgesY);

            Bitmap final = FinalProcess(edgesX, edgesY);
            final.Save(@"C:\Users\Soma\Pictures\final_parallel.jpg");
            sw.Stop();
            Console.WriteLine("Parallel process: " + sw.Elapsed);
        }

        public Bitmap ToGreyScale(Bitmap bmp, string outputSource)
        {
            LockBitmap lbmp = new LockBitmap(bmp);
            lbmp.LockBits();

            Parallel.For(0, lbmp.Height, i =>
            {
                for (int j = 0; j < lbmp.Width; j++)
                {
                    Color p = lbmp.GetPixel(i, j);

                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    int avg = (r + g + b) / 3;

                    lbmp.SetPixel(i, j, Color.FromArgb(a, avg, avg, avg));
                }
            });

            lbmp.UnlockBits();

            bmp = lbmp.source;
            bmp.Save(outputSource);
            return bmp;
        }

        public void EdgeDetection(short[,] vector, Bitmap bmp, short[,] GXY)
        {
            LockBitmap lbmp = new LockBitmap(bmp);
            lbmp.LockBits();

            Parallel.For(1, lbmp.Width - 1, i =>
            {
                for (int j = 1; j < lbmp.Height - 1; j++)
                {
                    short r1 = (short)(lbmp.GetPixel(i - 1, j - 1).R * vector[0, 0]);
                    short r2 = (short)(lbmp.GetPixel(i - 1, j).R * vector[0, 1]);
                    short r3 = (short)(lbmp.GetPixel(i - 1, j + 1).R * vector[0, 2]);

                    short r4 = (short)(lbmp.GetPixel(i, j - 1).R * vector[1, 0]);
                    short r5 = (short)(lbmp.GetPixel(i, j).R * vector[1, 1]);
                    short r6 = (short)(lbmp.GetPixel(i, j + 1).R * vector[1, 2]);

                    short r7 = (short)(lbmp.GetPixel(i + 1, j - 1).R * vector[2, 0]);
                    short r8 = (short)(lbmp.GetPixel(i + 1, j).R * vector[2, 1]);
                    short r9 = (short)(lbmp.GetPixel(i + 1, j + 1).R * vector[2, 2]);

                    short R = (short)((r1 + r2 + r3 + r4 + r5 + r6 + r7 + r8 + r9));
                    GXY[i, j] = R;
                }
            });

            lbmp.UnlockBits();
        }

        public Bitmap FinalProcess(short[,] edgesX, short[,] edgesY)
        {
            Bitmap outputImg = new Bitmap(edgesX.GetLength(0), edgesX.GetLength(1));

            for (int i = 0; i < edgesX.GetLength(0); i++)
            {
                for (int j = 0; j < edgesX.GetLength(1); j++)
                {
                    short final = (short)Math.Sqrt(Math.Pow(edgesX[i, j], 2) + Math.Pow(edgesY[i, j], 2));

                    if (final > 255)
                        final = 255;

                    outputImg.SetPixel(i, j, Color.FromArgb(final, final, final));
                }
            }

            return outputImg;
        }
    }
}
