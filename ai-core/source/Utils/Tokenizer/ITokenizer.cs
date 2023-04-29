using System.Collections.Generic;

namespace AICore.Utils.Tokenizer
{
    public interface ITokenizer
    {
        List<(long InputIds, long TokenTypeIds, long AttentionMask)> Encode(int sequenceLength, params string[] texts);
        List<string> UnTokenize(List<string> tokens);
        string IdToToken(int id);
        List<(string Token, int VocabularyIndex, long SegmentIndex)> Tokenize(params string[] texts);
        
    }
}