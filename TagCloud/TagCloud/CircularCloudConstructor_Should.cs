﻿using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudConstructorShould
    {
        [TestCase(0, 0, TestName = "is zero")]
        [TestCase(100, 100, TestName = "is in I quarter")]
        [TestCase(100, -100, TestName = "is in II quarter")]
        [TestCase(-100, 100, TestName = "is in III quarter")]
        [TestCase(-100, -100, TestName = "is in IV quarter")]
        public void NotThrow_WhenCenter(int x, int y)
        {
            // ReSharper disable once ObjectCreationAsStatement
            var options = new TagCloudLayoutOptions(new RoundSpiralGenerator(new Point(x, y), 3), new Point(x, y),2);
            Action creation = () => new CircularCloudLayouter(options, new RoundSpiralGenerator(new Point(x, y), 3));
            creation.Should()
                    .NotThrow();
        }
    }
}