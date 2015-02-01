using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework
{
    public interface IText
    {
        String Text
        {
            get;
        }

        Boolean ContainsLayer(String LayerName);

        ITextLayer GetLayer(String LayerName);

        ITextLayer[] Layers
        {
            get;
        }
    }
}
