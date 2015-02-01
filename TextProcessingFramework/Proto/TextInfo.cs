using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework.Proto
{
    public class TextInfo
    {
        Object[] _lockObjects;

        public TextInfo(String text, TextProcessingManager manager, Object userData = null)
        {
            Text = text;
            Manager = manager;
            UserData = userData;
            _TextLayers = new TextLayer[manager.LayersCount];
            _lockObjects = new Object[manager._processorList.Count];
            for (int i = 0; i < manager._processorList.Count; ++i)
                _lockObjects[i] = new Object();

        }

        public TextProcessingManager Manager
        {
            get;
            internal set;
        }

        public Object UserData
        {
            get;
            set;
        }

        TextLayer[] _TextLayers;

        public TextLayer[] TextLayers
        {
            get
            {
                return _TextLayers.ToArray();
            }
        }

        public TextLayer GetLayer(String layerName)
        {
            int index = 0;
            Manager.TryGetLayerIndex(layerName, out index);
            return GetLayer(index);
        }

        public TextLayer GetLayer(int layerIndex)
        {
            if (_TextLayers[layerIndex] == null)
            {
                int lockIndex = 0;
                BaseTextProcessor processor = Manager.GetProcessorByOutputLayerIndex(layerIndex, out lockIndex);

                lock (_lockObjects[lockIndex])
                {
                    if (_TextLayers[layerIndex] == null)
                    {

                        Parallel.ForEach(processor.InputLayerNames, inputName =>
                        {
                            GetLayer(inputName);
                        });

                        foreach (TextLayer layer in processor.Process(this))
                        {
                            layer.Owner = this;
                            SetLayer(layer);  
                        }
                    }   
                }
                
            }
            return _TextLayers[layerIndex];

        }

        private void SetLayer(TextLayer layer)
        {
            int index = 0;
            Manager.TryGetLayerIndex(layer.Name, out index);
            _TextLayers[index] = layer;
        }

        // todo BaseText GetLayer <int> implementation with using BaseTextProcesson.Process


        public String Text
        {
            get;
            internal set;
        }

        public Boolean ContainsLayer(String LayerName)
        {
            int index = 0;
            return Manager.TryGetLayerIndex(LayerName, out index);
        }

    }
}
