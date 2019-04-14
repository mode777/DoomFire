using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoomFire
{
    public class FireBox : Panel
    {
        private Bitmap _bitmap;
        private Random _rand = new Random();

        public FireBox()
        {
            _bitmap = new Bitmap(256,256, PixelFormat.Format8bppIndexed);

            // init pal
            var pal = _bitmap.Palette;

            for (int i = 0; i < _palette.Length; i += 3)
            {
                pal.Entries[i / 3] = Color.FromArgb(_palette[i], _palette[i + 1], _palette[i + 2]);
            }

            _bitmap.Palette = pal;

            // init 
            var data = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);

            unsafe
            {
                byte* dstPointer = (byte*)data.Scan0;
                var yOffset = (_bitmap.Height - 1) * _bitmap.Width;

                for (int i = 0; i < _bitmap.Width; i++)
                {
                    dstPointer[yOffset + i] = 36;
                }
            }

            _bitmap.UnlockBits(data);

            DoubleBuffered = true;
        }
        
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            //base.OnPaint(paintEventArgs);
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            paintEventArgs.Graphics.DrawImage(_bitmap, 0, 0, this.Width, this.Height);

        }

        public void Step()
        {
            var data = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);

            unsafe
            {
                byte* dstPointer = (byte*)data.Scan0;
                DoFire(dstPointer);
            }

            _bitmap.UnlockBits(data);

            Invalidate();
        }

        private unsafe void DoFire(byte* pixels)
        {
            for (var x = 0; x < _bitmap.Width; x++)
            {
                for (var y = 1; y < _bitmap.Height; y++)
                {
                    SpreadFire(pixels , y * _bitmap.Width + x);
                }
            }
        }

        private unsafe void SpreadFire(byte* pixels, int offset)
        {
            var rand = _rand.Next(3);
            var dst = offset - rand + 1;
            //pixels[dst - _bitmap.Width] = (byte)Math.Max(pixels[offset] - (rand & 1), 0);
            var lookup = Math.Max(Math.Min(dst - _bitmap.Width, _bitmap.Width * _bitmap.Height - 1), 0);
            pixels[lookup] = (byte)Math.Max(pixels[offset] - (rand & 1), 0);
        }

        static byte[] _palette = {
            0x07, 0x07, 0x07,
            0x1F, 0x07, 0x07,
            0x2F, 0x0F, 0x07,
            0x47, 0x0F, 0x07,
            0x57, 0x17, 0x07,
            0x67, 0x1F, 0x07,
            0x77, 0x1F, 0x07,
            0x8F, 0x27, 0x07,
            0x9F, 0x2F, 0x07,
            0xAF, 0x3F, 0x07,
            0xBF, 0x47, 0x07,
            0xC7, 0x47, 0x07,
            0xDF, 0x4F, 0x07,
            0xDF, 0x57, 0x07,
            0xDF, 0x57, 0x07,
            0xD7, 0x5F, 0x07,
            0xD7, 0x5F, 0x07,
            0xD7, 0x67, 0x0F,
            0xCF, 0x6F, 0x0F,
            0xCF, 0x77, 0x0F,
            0xCF, 0x7F, 0x0F,
            0xCF, 0x87, 0x17,
            0xC7, 0x87, 0x17,
            0xC7, 0x8F, 0x17,
            0xC7, 0x97, 0x1F,
            0xBF, 0x9F, 0x1F,
            0xBF, 0x9F, 0x1F,
            0xBF, 0xA7, 0x27,
            0xBF, 0xA7, 0x27,
            0xBF, 0xAF, 0x2F,
            0xB7, 0xAF, 0x2F,
            0xB7, 0xB7, 0x2F,
            0xB7, 0xB7, 0x37,
            0xCF, 0xCF, 0x6F,
            0xDF, 0xDF, 0x9F,
            0xEF, 0xEF, 0xC7,
            0xFF, 0xFF, 0xFF
        };
    }
}
