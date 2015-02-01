using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessingFramework.Proto;

namespace TextProcessingSimpleExample
{
    class MyExampleTextProcessor : BaseTextProcessor
    {
        private static String _name = "FirstProcessor";

        private static String[] _inputLayers = new String[] {};

        private static String[] _outputLayers = new String[] {"FirstLayer"};

        

        public MyExampleTextProcessor()
            : base(_name, _inputLayers, _outputLayers)
        {
            
        }

        public override TextLayer[] Process(TextInfo text)
        {
            TextLayer[] result = null;
            if (text != null)
            {
                result = new TextLayer[1];
                int charCount = text.Text.Length;
                result[0] = new TextLayer(_outputLayers[0], new BaseTextLayerItem[]{ new MyLayerItem("Created by " + _name, charCount) });
            }

            return result;
        }
    }

    class MyExampleTextProcessor2 : BaseTextProcessor
    {
        private static String _name = "SecondProcessor";

        private static String[] _inputLayers = new String[] { "FirstLayer" };

        private static String[] _outputLayers = new String[] { "SecondLayer" };



        public MyExampleTextProcessor2()
            : base(_name, _inputLayers, _outputLayers)
        {

        }

        public override TextLayer[] Process(TextInfo text)
        {
            TextLayer[] result = null;
            if (text != null)
            {
                result = new TextLayer[1];
                int charCount = text.Text.Length;
                MyLayerItem data = text.GetLayer("FirstLayer").ItemList[0] as MyLayerItem;
                result[0] = new TextLayer(_outputLayers[0], new BaseTextLayerItem[] { new MyLayerItem("Created by " + _name + " from " + data.Owner.Name, charCount) });
            }

            return result;
        }
    }
}
