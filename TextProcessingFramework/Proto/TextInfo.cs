using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessingFramework.Proto
{
    public class TextInfo
    {
        List<Object> _lockObjects;

        /// <summary>
        /// Constrictor.
        /// </summary>
        /// <param name="text">String, which represent text.</param>
        /// <param name="manager">Reference to the object manager.</param>
        /// <param name="userData">Some user's data. By default is null.</param>
        public TextInfo(String text, TextProcessingManager manager, Object userData = null)
        {
            Text = text;
            Manager = manager;
            UserData = userData;
            _textLayers = new List<TextLayer>(manager.LayersCount+1);

            _lockObjects = new List<Object>(manager._processorList.Count+1);

            UpdateListsOfLayersAndLocks();

        }

        private void UpdateListsOfLayersAndLocks()
        {
            for (int i = _textLayers.Count; i < Manager.LayersCount; ++i)
            {
                _textLayers.Add(null);
            }
            for (int i = _lockObjects.Count; i < Manager._processorList.Count; ++i)
            {
                _lockObjects.Add(new Object());
            }
        }
        /// <summary>
        /// Reference to the object manager.
        /// </summary>
        public TextProcessingManager Manager
        {
            get;
            internal set;
        }
        /// <summary>
        /// Some user's data.
        /// </summary>
        public Object UserData
        {
            get;
            set;
        }

        List<TextLayer> _textLayers;

        /// <summary>
        /// An array of layers.
        /// </summary>
        public TextLayer[] TextLayers
        {
            get
            {
                //Parallel.For(0, _TextLayers.Length, i => GetLayer(i));
                UpdateListsOfLayersAndLocks();
                return _textLayers.ToArray();
            }
        }

        /// <summary>
        /// Get layer by name. 
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        /// <returns>Base representation of the layer.</returns>
        public TextLayer GetLayer(String layerName)
        {
            int index = 0;
            Manager.TryGetLayerIndex(layerName, out index);
            return GetLayer(index);
        }

        /// <summary>
        /// Get layer by index. 
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        /// <returns>Base representation of the layer.</returns>
        public TextLayer GetLayer(int layerIndex)
        {

            UpdateListsOfLayersAndLocks();

            if (_textLayers[layerIndex] == null)
            {
                int lockIndex = 0;
                BaseTextProcessor processor = Manager.GetProcessorByOutputLayerIndex(layerIndex, out lockIndex);

                lock (_lockObjects[lockIndex])
                {
                    if (_textLayers[layerIndex] == null)
                    {
                        //Some code for optimisation
                        //Parallel.ForEach(processor.InputLayerNames, inputName =>
                        //{
                        //    GetLayer(inputName);
                        //});

                        foreach (TextLayer layer in processor.Process(this))
                        {
                            layer.Owner = this;
                            SetLayer(layer);  
                        }
                    }   
                }
                
            }
            return _textLayers[layerIndex];

        }

        private void SetLayer(TextLayer layer)
        {
            int index = 0;
            Manager.TryGetLayerIndex(layer.Name, out index);
            _textLayers[index] = layer;
        }

        // todo BaseText GetLayer <int> implementation with using BaseTextProcesson.Process

        /// <summary>
        /// Get text string 
        /// </summary>
        public String Text
        {
            get;
            internal set;
        }

        /// <summary>
        /// Check layer by name.
        /// </summary>
        /// <param name="LayerName">Name of the layer.</param>
        /// <returns></returns>
        public Boolean ContainsLayer(String LayerName)
        {
            int index = 0;
            return Manager.TryGetLayerIndex(LayerName, out index);
        }

    }
}
