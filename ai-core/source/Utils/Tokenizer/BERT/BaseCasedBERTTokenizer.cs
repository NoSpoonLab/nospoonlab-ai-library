using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AICore.Services.Extension;

namespace AICore.Utils.Tokenizer.BERT
{
    public class BaseCasedBERTTokenizer : BaseBERTTokenizer
    {
        public BaseCasedBERTTokenizer(string vocabularyFilePath) : base(vocabularyFilePath) { }
        public BaseCasedBERTTokenizer(StreamReader vocabularyFile) : base(vocabularyFile) { }

        protected override IEnumerable<string> TokenizeSentence(string text)
        {
            return text.Split(new string[] { " ", "   ", "\r\n" }, StringSplitOptions.None)
                .SelectMany(o => o.SplitAndKeep(".,;:\\/?!#$%()=+-*\"'–_`<>&^@{}[]|~'".ToArray()));
        }
    }
}