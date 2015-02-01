using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework.Proto
{
    public class TextLayer
    {
        public TextLayer(String name, IEnumerable<BaseTextLayerItem> items)
        {
            Name = name;
            _Items.AddRange(items);
            _Items.ForEach(a=> a.Owner = this);
        }

        public TextInfo Owner
        {
            get;
            internal set;
        }

        List<BaseTextLayerItem> _Items = new List<BaseTextLayerItem>();

        public List<BaseTextLayerItem> ItemList
        {
            get 
            {
                return _Items.ToList();
            }
        }

        public String Name
        {
            get;
            internal set;
        }

    }
}
