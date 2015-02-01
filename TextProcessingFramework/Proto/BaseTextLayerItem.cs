using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework.Proto
{
    public class BaseTextLayerItem 
    {
        public virtual Object[] Properties
        {
            get;
            set;
        }

        public TextLayer Owner
        {
            get;
            set;
        }

        public virtual String[] PropertiesNames
        {
            get
            {
                return new String[]{"Name"};
            }
        }

        public Object GetProperty(int propertyIndex)
        {
            return Properties[propertyIndex];  
        }

        public T GetProperty<T>(int propertyIndex)
        {
            return (T)Properties[propertyIndex];
        }

        public virtual Object GetProperty(String propertyName)
        {
            throw new NotImplementedException("GetProperty by Name");
        }

        public virtual T GetProperty<T>(String propertyName)
        {
            throw new NotImplementedException("GetProperty by Name");
        }
    }
}
