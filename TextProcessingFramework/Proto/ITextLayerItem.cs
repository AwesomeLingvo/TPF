using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework
{
    public interface ITextLayerItem
    {
        Object GetProperty(String propertyName);
        T GetProperty<T>(String propertyName);
    }
}
