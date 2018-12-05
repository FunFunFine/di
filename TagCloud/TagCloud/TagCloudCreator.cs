﻿using System;
using System.Collections;
using NUnit.Framework;
using System.Linq;
using TagsCloudVisualization;

namespace TagCloud
{
    
    internal class TagCloudCreator
    {
        private readonly CircularCloudLayouter.Factory layouterFactory;
        private readonly ITextReader reader;
        private readonly IWordsPreparer preparer;
        private readonly ITagCloudStatsGenerator generator;
        private readonly ITagCloudSaver saver;

        public  TagCloudCreator(CircularCloudLayouter.Factory layouterFactory, ITextReader reader, IWordsPreparer preparer, ITagCloudStatsGenerator generator, ITagCloudSaver saver)
        {
            this.layouterFactory = layouterFactory;
            this.reader = reader;
            this.preparer = preparer;
            this.generator = generator;
            this.saver = saver;
        }

        public TagCloudImage CreateImage(TagCloudOptions options)
        {
            var words = this.reader.ReadWords();
            words = this.preparer.PrepareWords(words);
            var stats = this.generator.GenerateStats(words);
            var layouter = this.layouterFactory.Invoke(options.Spiral, options.Center);
            var wordPairs = stats.Select(s => (layouter.PutNextRectangle(s.CreateRectangle()), s));
            return this.saver.CreateTagCloudImage(wordPairs, options);
        }
    }
}
