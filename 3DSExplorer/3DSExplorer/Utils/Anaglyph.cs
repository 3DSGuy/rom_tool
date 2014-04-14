using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace _3DSExplorer
{
    class Anaglyph
    {
        private float[] methMagic;
        private float[] methOptim;

        private float[][] methMagicZeroZero;
        private float[][] methZeroZeroMagic;
        private float[][] methZeroMagicMagic;
        private float[][] methIdentZeroZero;
        private float[][] methZeroIdentIdent;
        private float[][] methOptimZeroZero;

        public float[][][] TrueAnaglyph;
        public float[][][] GrayAnaglyph;
        public float[][][] ColorAnaglyph;
        public float[][][] HalfColorAnaglyph;
        public float[][][] OptimizedAnaglyph;

        public PropertyItem PublicProp = null;

        public Anaglyph()
        {
            methMagic = new[] { 0.299f, 0.587f, 0.114f };
            methOptim = new[] { 0, 0.7f, 0.3f };

            methMagicZeroZero = new[] { 
                new[] {methMagic[0], 0, 0, 0, 0},
                new[] {methMagic[1], 0, 0, 0, 0},
                new[] {methMagic[2], 0, 0, 0, 0},
                new[] {0f, 0, 0, 1, 0},
                new[] {0f, 0, 0, 0, 1}
            };
            methZeroZeroMagic = new[] { 
                new[] {0f, 0, methMagic[0], 0, 0},
                new[] {0f, 0, methMagic[1], 0, 0},
                new[] {0f, 0, methMagic[2], 0, 0},
                new[] {0f, 0, 0, 1, 0},
                new[] {0f, 0, 0, 0, 1}
            };
            methZeroMagicMagic = new[] { 
                new[] {0f,methMagic[0],methMagic[0], 0, 0},
                new[] {0f,methMagic[1],methMagic[1], 0, 0},
                new[] {0f,methMagic[2],methMagic[2], 0, 0},
                new[] {0f, 0, 0, 1, 0},
                new[] {0f, 0, 0, 0, 1}
            };
            methIdentZeroZero = new[] { 
                new[] {1f, 0, 0, 0, 0},
                new[] {0f, 0, 0, 0, 0},
                new[] {0f, 0, 0, 0, 0},
                new[] {0f, 0, 0, 1, 0},
                new[] {0f, 0, 0, 0, 1}
            };
            methZeroIdentIdent = new[] { 
                new[] {0f, 0, 0, 0, 0},
                new[] {0f, 1, 0, 0, 0},
                new[] {0f, 0, 1, 0, 0},
                new[] {0f, 0, 0, 1, 0},
                new[] {0f, 0, 0, 0, 1}
            };
            methOptimZeroZero = new[] { 
                new[] {methOptim[0], 0, 0, 0, 0},
                new[] {methOptim[1], 0, 0, 0, 0},
                new[] {methOptim[2], 0, 0, 0, 0},
                new[] {0f, 0, 0, 1, 0},
                new[] {0f, 0, 0, 0, 1}
            };
            TrueAnaglyph = new[] { methMagicZeroZero, methZeroZeroMagic };
            GrayAnaglyph = new[] { methMagicZeroZero, methZeroMagicMagic };
            ColorAnaglyph = new[] { methIdentZeroZero, methZeroIdentIdent };
            HalfColorAnaglyph = new[] { methMagicZeroZero, methZeroIdentIdent };
            OptimizedAnaglyph = new[] { methOptimZeroZero, methZeroIdentIdent };
        }

        public static Image MakeAnaglyph(Image imageLeft, Image imageRight, float[][][] method, int parallax)
        {
            int outputWidth = Math.Max(imageLeft.Width, imageRight.Width),
                outputHeight = Math.Max(imageLeft.Height, imageRight.Height);
            Rectangle rec = new Rectangle(0, 0, outputWidth, outputHeight);

            // Create a Bitmap
            Bitmap bmpSource = new Bitmap(imageLeft);
            Bitmap bmpOutputLeft = new Bitmap(imageLeft.Width, imageLeft.Height);
            // Create a Graphics object
            Graphics g = Graphics.FromImage(bmpOutputLeft);
            g.Clear(System.Drawing.Color.White);
            // Create a ColorMatrix
            ColorMatrix clrMatrix = new ColorMatrix(method[0]);
            // Create ImageAttributes
            ImageAttributes imgAttribs = new ImageAttributes();
            // Set color matrix
            imgAttribs.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
            // Draw image with ImageAttributes
            g.DrawImage(bmpSource, rec, 0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, imgAttribs);
            // Dispose
            bmpSource.Dispose();
            g.Dispose();

            // Create a Bitmap
            bmpSource = new Bitmap(imageRight);
            Bitmap bmpOutputRight = new Bitmap(imageRight.Width, imageRight.Height);
            // Create a Graphics object
            g = Graphics.FromImage(bmpOutputRight);
            g.Clear(System.Drawing.Color.White);
            // Create a ColorMatrix
            clrMatrix = new ColorMatrix(method[1]);
            // Create ImageAttributes
            imgAttribs = new ImageAttributes();
            // Set color matrix
            imgAttribs.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
            // Draw image with ImageAttributes
            g.DrawImage(bmpSource, rec, 0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, imgAttribs);
            bmpSource.Dispose();
            g.Dispose();

            unsafe //pointers code
            {
                BitmapData bmData = bmpOutputLeft.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Color pixel;

                for (int y = 0; y < bmData.Height; y++)
                {
                    byte* row = (byte*)bmData.Scan0 + (y * bmData.Stride);
                    for (int x = 0, p = 0; x < bmData.Width + parallax; p += 3, x++)
                    {
                        pixel = bmpOutputRight.GetPixel(x - parallax, y);
                        row[p] += pixel.B;
                        row[p + 1] += pixel.G;
                        row[p + 2] += pixel.R;
                    }
                }
                bmpOutputLeft.UnlockBits(bmData);
            }
            bmpOutputRight.Dispose();
            Bitmap bmpOutput = new Bitmap(outputWidth + parallax, outputHeight);
            g = Graphics.FromImage(bmpOutput);
            g.DrawImage(bmpOutputLeft, 0, 0, new Rectangle(0, 0, outputWidth + parallax, outputHeight), GraphicsUnit.Pixel);
            bmpOutputLeft.Dispose();

            return bmpOutput;
        }

    }
}
