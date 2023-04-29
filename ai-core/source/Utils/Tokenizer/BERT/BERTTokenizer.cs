using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AICore.Services.Types.Data;

namespace AICore.Utils.Tokenizer.BERT
{
    public class BERTTokenizer : ITokenizer
    {
        #region Variables

        private const string _vocabularyFilePath = "AICore.source.Utils.Tokenizer.BERT.Data.";
        private IBERTTokenizer _instance;

        #endregion

        #region Constructors

        public BERTTokenizer(AIModel type)
        {
            _instance = GetTokenizerByType(type);
        }
        
        public BERTTokenizer(string vocabularyFilePath, bool isCased = false)
        {
            _instance = isCased ? (IBERTTokenizer) new BaseCasedBERTTokenizer(vocabularyFilePath) : new BaseUncasedBERTTokenizer(vocabularyFilePath);
        }

        #endregion

        #region Private Methods

        private IBERTTokenizer GetTokenizerByType(AIModel type)
        {
            
            var assembly = Assembly.GetExecutingAssembly();
            switch (type)
            {
                case AIModel.embedding_bert_base_uncased:
                    return new BaseUncasedBERTTokenizer(new StreamReader(assembly.GetManifestResourceStream(_vocabularyFilePath + "base_uncased.txt")));
                case AIModel.embedding_bert_base_large_uncased:
                    return new BaseUncasedBERTTokenizer(new StreamReader(assembly.GetManifestResourceStream(_vocabularyFilePath + "base_uncased_large.txt")));
                case AIModel.embedding_bert_base_cased:
                    return new BaseCasedBERTTokenizer(new StreamReader(assembly.GetManifestResourceStream(_vocabularyFilePath + "base_cased.txt")));
                case AIModel.embedding_bert_base_large_cased:
                    return new BaseCasedBERTTokenizer(new StreamReader(assembly.GetManifestResourceStream(_vocabularyFilePath + "base_cased_large.txt")));
                case AIModel.embedding_bert_base_multilingual_cased:
                    return new BaseCasedBERTTokenizer(new StreamReader(assembly.GetManifestResourceStream(_vocabularyFilePath + "base_cased_multilingual.txt")));
                case AIModel.embedding_bert_base_german_cased:
                    return new BaseCasedBERTTokenizer(new StreamReader(assembly.GetManifestResourceStream(_vocabularyFilePath + "base_cased_german.txt")));
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion

        #region Public Methods
        
        public List<(long InputIds, long TokenTypeIds, long AttentionMask)> Encode(int sequenceLength, params string[] texts)
        {
            return _instance.Encode(sequenceLength, texts);
        }

        public List<string> UnTokenize(List<string> tokens)
        {
            return _instance.UnTokenize(tokens);
        }

        public string IdToToken(int id)
        {
            return _instance.IdToToken(id);
        }

        public List<(string Token, int VocabularyIndex, long SegmentIndex)> Tokenize(params string[] texts)
        {
            return _instance.Tokenize(texts);
        }
        
        #endregion
    }
}