using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework.Proto
{
    public class BaseTextProcessor
    {
        public BaseTextProcessor(String name, IEnumerable<String> inputLayers, IEnumerable<String> outputLayers)
        {
            Name = name;
            _inputLayers.AddRange(inputLayers);
            _outputLayers.AddRange(outputLayers);
        }

        List<String> _inputLayers = new List<string>();
        List<String> _outputLayers = new List<string>();

        public String Name
        {
            get;
            internal set;
        }

        public virtual TextLayer[] Process(TextInfo text)
        {
            TextLayer[] result = null;

            if (text != null)
            {

                result = new TextLayer[_outputLayers.Count];
                for (int i = 0; i < _outputLayers.Count; ++i)
                {
                    result[i] = new TextLayer(_outputLayers[i], new List<BaseTextLayerItem> { new BaseTextLayerItem() });
                }
                
            }
            return result;
        }

        public String[] InputLayerNames
        {
            get
            {
               return _inputLayers.ToArray();   
            }

        }

        public String[] OutputLayerNames
        {
            get
            {
                return _outputLayers.ToArray();
            }
        }
    }
}
