﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagCloudCreation;

namespace TagCloudApp
{
    internal abstract class UserInterface
    {
        protected readonly TagCloudCreator Creator;
        protected readonly Dictionary<string, ITextReader> Readers;

        protected UserInterface(TagCloudCreator creator, IEnumerable<ITextReader> readers)
        {
            Creator = creator;
            Readers = readers.ToDictionary(g => g.Extension);
        }

        public abstract void Run(string[] startupArgs);

        protected bool TryRead(string path, out IEnumerable<string> words)
        {
            words = null;
            var extension = Path.GetExtension(path);
            if (extension == null)
                return false;
            return Readers.TryGetValue(extension, out var reader) 
                && reader.TryReadWords(path, out words);
        }
    }
}
