using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework
{
    public interface ITextProcessor
    {
        String Name
        {
            get;
        }

        void Process(IText text);

        String[] InputLayerNames
        {
            get;
        }

        String[] OutputLayerNames
        {
            get;
        }
    }
}
