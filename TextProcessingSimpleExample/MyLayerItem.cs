using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessingFramework.Proto;

namespace TextProcessingSimpleExample
{
    class MyLayerItem : BaseTextLayerItem
    {
        public String Name;
        public int CharCount;

        static readonly Dictionary<String, int> _proropertyTable = new Dictionary<string, int>()
            {
                { "Name", 0 },
                {"CharCount", 1}
            };

        public MyLayerItem(String name, int charCount = 0)
        {
            Name = name;
            CharCount = charCount;
        }

        public override object[] Properties
        {
            get
            {
                return new Object[] { Name, CharCount };
            }
        }

        public override string[] PropertiesNames
        {
            get
            {
                return _proropertyTable.Select(a => a.Key).ToArray();
            }
        }

        public override object GetProperty(string propertyName)
        {
            int index = _proropertyTable[propertyName];
            return Properties[index];
        }

        public override T GetProperty<T>(string propertyName)
        {
            return (T)GetProperty(propertyName);
        }

    }
}
