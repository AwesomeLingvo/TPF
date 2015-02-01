using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework
{
    public interface ITextLayer
    {
        String Name
        {
            get;
        }

        List<ITextLayerItem> Items
        {
            get;
        }
    }
}
