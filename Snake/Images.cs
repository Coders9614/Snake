﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Snake
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");

        public readonly static ImageSource Body = LoadImage("Body.png");

        public readonly static ImageSource Head = LoadImage("Head.png");

        public readonly static ImageSource Food = LoadImage("Food.png");

        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");

        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");

       // public readonly static ImageSource Icon = LoadImage("Icon.ico");

        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}
