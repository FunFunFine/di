﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagCloudVisualization
{
    public class TagCloudImageCreator
    {
        private const double Expander = 1.2;
        private readonly CompositeDrawer compositeDrawer;
        private readonly Func<Point, CircularCloudLayouter> layouterFactory;

        public TagCloudImageCreator(CompositeDrawer drawer, Func<Point, CircularCloudLayouter> layouterFactory)
        {
            compositeDrawer = drawer;
            this.layouterFactory = layouterFactory;
        }

        public float MaxFontSize => 100;

        public virtual Bitmap CreateTagCloudImage(IEnumerable<WordInfo> tagCloud, ImageCreatingOptions options)
        {
            tagCloud = SetRectanglesToCloud(tagCloud, options).ToList();

            var (width, height, center) = GetTagCloudDimensions(tagCloud);

            if (options.ImageSize != null)
            {
                width = options.ImageSize.Value.Width;
                height = options.ImageSize.Value.Height;
            }

            var image = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(image))
                foreach (var wordInfo in tagCloud)
                {
                    var rectangle = wordInfo.Rectangle;

                    var fontScale = wordInfo.Scale;

                    using (var font = new Font(options.FontName, fontScale))
                    {
                        rectangle.Offset(center);
                        DrawSingleWord(graphics, options, wordInfo, font);
                    }
                }

            return image;
        }

        private static (int width, int height, Point center) GetTagCloudDimensions(IEnumerable<WordInfo> tagCloud)
        {
            var areaSize = tagCloud.Select(w => w.Rectangle)
                                   .GetUnitedSize();

            areaSize = new Size((int) (areaSize.Width * Expander), (int) (areaSize.Height * Expander));

            var width = areaSize.Width;
            var height = areaSize.Height;
            var center = new Point(areaSize.Width / 2, areaSize.Height / 2);
            return (width, height, center);
        }

        private IEnumerable<WordInfo> SetRectanglesToCloud(IEnumerable<WordInfo> tagCloud, ImageCreatingOptions options)
        {
            var layouter = layouterFactory(options.Center);
            foreach (var wordInfo in tagCloud)
            {
                Size size;
                using (var font = new Font(options.FontName, wordInfo.Scale))
                    size = TextRenderer.MeasureText(wordInfo.Word, font);

                var rectangle = layouter.PutNextRectangle(size);
                yield return wordInfo.With(rectangle);
            }
        }

        private protected void DrawSingleWord(
            Graphics graphics,
            ImageCreatingOptions options,
            WordInfo wordInfo,
            Font font)
        {
            if (compositeDrawer.TryGetDrawer(wordInfo, out var drawer))
                drawer.DrawWord(graphics, options, wordInfo, font);
            else
                throw new ArgumentException($"There is no drawer that can draw given word: {wordInfo.Word}");
        }
    }
}
