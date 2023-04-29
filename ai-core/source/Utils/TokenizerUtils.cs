using System.Collections.Generic;
using AICore.Services.Types.Data;
using AICore.Utils.Tokenizer;
using AICore.Utils.Tokenizer.BERT;

namespace AICore.Utils
{
    public static class TokenizerUtils
    {
        #region Variables

        private static Dictionary<AIModel, ITokenizer> _tokenizers = new Dictionary<AIModel, ITokenizer>();

        #endregion
        
        #region Methods

        public static ITokenizer GetTokenizer(AIModel type)
        {
            if (_tokenizers.ContainsKey(type))
            {
                return _tokenizers[type];
            }
            else
            {
                var tokenizer = (ITokenizer) new BERTTokenizer(type);
                _tokenizers.Add(type, tokenizer);
                return tokenizer;
            }
        }

        #endregion
        
        
    }
}