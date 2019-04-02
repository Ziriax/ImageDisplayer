﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageDisplayer
{
    public partial class ImageView : Form
    {
        private const int ImageWidth = 256;
        private const int ImageHeight = 192;
        
        private byte[] buffer;
        private byte[] tmpBuffer;

        private byte[] y;
        private byte[] u;
        private byte[] v;
        private byte[] r;
        private byte[] g;
        private byte[] b;



        public ImageView()
        {
            InitializeComponent();

            buffer = File.ReadAllBytes(Environment.CurrentDirectory + "/image.yuv");
            r = new byte[sizeof(byte) * ImageWidth * ImageHeight * 3];
            g = new byte[sizeof(byte) * ImageWidth * ImageHeight * 3];
            b = new byte[sizeof(byte) * ImageWidth * ImageHeight * 3];

            y = buffer.Skip(0).Take(ImageWidth * ImageHeight).ToArray();
            u = buffer.Skip(ImageWidth * ImageHeight).Take(ImageWidth * ImageHeight / 4).ToArray();
            v = buffer.Skip(ImageWidth * ImageHeight + ImageWidth * ImageHeight / 4).Take(ImageWidth * ImageHeight).ToArray();

            // https://docs.microsoft.com/en-us/previous-versions/aa917087(v=msdn.10)
            for (int i = 0; i < (ImageWidth * ImageHeight); i++)
            {
                byte c = y[i];
                byte d = u[i];
                byte e = v[i];

                r[i] = AsByte((298 * c + 409 * e + 128) >> 8);
                g[i] = AsByte((298 * c - 100 * d - 208 * e + 128) >> 8);
                b[i] = AsByte((298 * c + 516 * d + 128) >> 8);
            }
        }

        private static byte AsByte(int value)
        {
            //return (byte)value;
            if (value > 255)
                return 255;
            if (value < 0)
                return 0;
            return (byte)value;
        }
    }
}
