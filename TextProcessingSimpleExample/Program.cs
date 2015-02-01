using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessingFramework;
using TextProcessingFramework.Proto;

namespace TextProcessingSimpleExample
{
    class Program
    {


        static void Main(string[] args)
        {

            TextProcessingManager exampleManager = new TextProcessingManager();
   
            MyExampleTextProcessor firstProcessor = new MyExampleTextProcessor();
            MyExampleTextProcessor2 secondProcessor = new MyExampleTextProcessor2();

            if (exampleManager.RegisterProcessor(firstProcessor) !=
                TextProcessingManager.RegisterState.SuccessfullyRegistered ||
                exampleManager.RegisterProcessor(secondProcessor) !=
                TextProcessingManager.RegisterState.SuccessfullyRegistered)
            {
                
                Console.WriteLine("Error!");

            }

            TextInfo textInfo = exampleManager.GetTextInfo("It is my test text!");

            textInfo.GetLayer("SecondLayer");

            Console.ReadLine();

        }
    }
}
