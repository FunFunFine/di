﻿using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloudCreation;
using TagCloudVisualization;

namespace TagCloudApp
{
    [TestFixture]
    public class SelectedBoringWordsRemover_Should
    {
        private SelectedBoringWordsRemover remover;

        [SetUp]
        public void SetUp()
        {
            var fake = A.Fake<ITextReader>();
            IEnumerable<string> _;
            A.CallTo(() => fake.TryReadWords(A<string>.Ignored, out _))
             .Returns(true)
             .AssignsOutAndRefParameters(new List<string>(){"a"});
            remover = new SelectedBoringWordsRemover(fake);


        }

        [Test]
        public void ReturnNull_IfWordIsBoring()
        {
            remover.PrepareWord(new WordInfo("a", 1), new TagCloudCreationOptions(null, "path"))
                   .Should()
                   .BeNull();
        }
        [Test]
        public void ReturnWord_IfWordIsNotBoring()
        {
            var wordInfo = new WordInfo("b", 1);
            remover.PrepareWord(wordInfo, new TagCloudCreationOptions(null, "path"))
                   .Should()
                   .Be(wordInfo);
        }
    }
}