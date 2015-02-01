using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessingFramework.Proto;

namespace TextProcessingFramework
{
    public class TextProcessingManager
    {
        public enum RegisterState
        {
            SuccessfullyRegistered = 0,
            AlreadyRegistered,
            OutputLayerNotUniq,
            InputLayerNotFound
        }

        Dictionary<String, int> _layerNameTable = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
        Dictionary<String, int> _processorNameTable = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);
        List<int> _processorByOutputLayer = new List<int>();
        List<List<int>> _processorByInputLayer = new List<List<int>>();

        internal List<BaseTextProcessor> _processorList = new List<BaseTextProcessor>();

        public RegisterState RegisterProcessor(BaseTextProcessor processor)
        {
            int proc_index = 0;
            if (!_processorNameTable.TryGetValue(processor.Name, out proc_index))
            {
                String[] input = processor.InputLayerNames;
                List<List<int>> inputRegisterCache = new List<List<int>>();
                int len = input.Length;

                for(int i = 0; i < len; ++i)
                {
                    int layerIndex = 0;
                    if(!TryGetLayerIndex(input[i], out layerIndex))
                    {
                        len = i;
                        break;
                    }

                    if (layerIndex == _processorByInputLayer.Count)
                    {
                        _processorByInputLayer.Add(new List<int>());   
                    }
                    List<int> procByInput = _processorByInputLayer[layerIndex]; 
                    inputRegisterCache.Add(procByInput);
                    procByInput.Add(_processorList.Count);
                }

                if (len != input.Length)  //очистка в случае неудачи
                {
                    for(int i = 0; i < inputRegisterCache.Count; ++i)
                        inputRegisterCache[i].RemoveAt(inputRegisterCache[i].Count - 1);

                    return RegisterState.InputLayerNotFound;
                }


                String[] output = processor.OutputLayerNames;
                len = output.Length;

                for(int i = 0; i < len; ++i)
                {
                    if(_layerNameTable.ContainsKey(output[i]))
                    {
                        len = i;
                        break;
                    }
  
                    _layerNameTable.Add(output[i], _processorByOutputLayer.Count);
                    _processorByOutputLayer.Add(_processorList.Count);
                }

                if (len != output.Length) //очистка в случае неудачи
                {
                    for (int i = 0; i < inputRegisterCache.Count; ++i)
                        inputRegisterCache[i].RemoveAt(inputRegisterCache[i].Count - 1);

                    for (int i = 0; i < len; ++i)
                    {
                        _layerNameTable.Remove(output[i]);
                    }
                    _processorByOutputLayer.RemoveRange(_processorByOutputLayer.Count - len, len);

                    return RegisterState.OutputLayerNotUniq;
                }

                _processorList.Add(processor);

                return RegisterState.SuccessfullyRegistered;
            }
            else
                return RegisterState.AlreadyRegistered;
        }

        public List<BaseTextProcessor> RegisteredProcessors
        {
            get
            {
                return _processorList.ToList();
            }
        }

        public List<String> RegisteredLayers
        {
            get
            {
                return _layerNameTable.Select(a=>a.Key).ToList();
            }
        }

        internal Int32 LayersCount
        {
            get { return _processorByOutputLayer.Count; }
        }

        public bool TryGetLayerIndex(String layerName, out int index)
        {
            return _layerNameTable.TryGetValue(layerName, out index);
        }

        public BaseTextProcessor GetProcessorByOutputLayerIndex(int layerIndex, out int processorIndex) //with sync object
        {
            processorIndex = _processorByOutputLayer[layerIndex];
            return _processorList[_processorByOutputLayer[layerIndex]];    
        }

        public TextInfo GetTextInfo(String text, Object userData = null)
        {
            return new TextInfo(text, this, userData);
        }
    }
}
