using System.Collections.Generic;

namespace AICore.Infrastructure.NoSpoonAI.Types.Data
{
    public class NoSpoonAIListData<T>
    {
        #region Variables
        
        public List<T> data { get; set; }

        #endregion

        #region Methods
        
        public List<T> GetData()
        {
            return data;
        }
        

        #endregion
    }
}