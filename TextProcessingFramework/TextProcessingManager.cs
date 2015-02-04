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
        /// <summary>
        /// This enum represent state of processor registration.
        /// </summary>
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

        /// <summary>
        /// Method, which perform registration processor and its layers.
        /// Stable, this method guarantees the indexes processors previously recorded.
        /// </summary>
        /// <param name="processor">a processor for registration.</param>
        /// <returns>Registration result state</returns>
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
        /// <summary>
        /// Get a copy of registered processors list.
        /// </summary>
        public List<BaseTextProcessor> RegisteredProcessors
        {
            get
            {
                return _processorList.ToList();
            }
        }
        /// <summary>
        /// Get a copy of registered layers list. 
        /// </summary>
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
        /// <summary>
        /// Try to get layer index by name.
        /// </summary>
        /// <param name="layerName">Layer's name/</param>
        /// <param name="index">Output value of index. If layer not found index equals to zero.</param>
        /// <returns>Flag, which represent success of serch.</returns>
        public bool TryGetLayerIndex(String layerName, out int index)
        {
            return _layerNameTable.TryGetValue(layerName, out index);
        }

        //This method need to refactor! And BaseTextProcessor need to change some of it's ideology. 
        /// <summary>
        /// Try to get processor index by index of layer.
        /// </summary>
        /// <param name="layerIndex">Index of layer.</param>
        /// <param name="processorIndex">Output value of index. If layer not found index equals to zero.</param>
        /// <returns>Processor object.</returns>
        public BaseTextProcessor GetProcessorByOutputLayerIndex(int layerIndex, out int processorIndex) //with sync object
        {
            processorIndex = _processorByOutputLayer[layerIndex];
            return _processorList[_processorByOutputLayer[layerIndex]];    
        }

        /// <summary>
        /// Create instance of TextInfo associated with this Manager.
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="userData">user defined data</param>
        /// <returns>Instance of TextInfo class</returns>
        public TextInfo GetTextInfo(String text, Object userData = null)
        {
            return new TextInfo(text, this, userData);
        }
    }
}
