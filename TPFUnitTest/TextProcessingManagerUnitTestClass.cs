using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextProcessingFramework;
using TextProcessingFramework.Proto;

namespace TPFUnitTest
{
    [TestClass]
    public class TextProcessingManagerUnitTestClass
    {
        [TestMethod]
        public void RegisterProcessorMethod_With_TwoProcessorsWithValidDependences()
        {
            TextProcessingManager manager = new TextProcessingManager();

            BaseTextProcessor processor1 = new BaseTextProcessor("processor1", new String[]{}, new String[]{"layer1"});
            BaseTextProcessor processor2 = new BaseTextProcessor("processo2", new String[]{"layer1"}, new String[]{"layer2"});

            String details;
            var regResult = manager.RegisterProcessor(processor1, out details);
            if (regResult != TextProcessingManager.RegisterState.SuccessfullyRegistered)
                Assert.Fail("Registration of the processor failed.", new Object[]{regResult, details});

            regResult = manager.RegisterProcessor(processor2, out details);

            Assert.AreEqual(regResult, TextProcessingManager.RegisterState.SuccessfullyRegistered, "Registration of the processor 2 failed. Details is : " + details);
        }

        [TestMethod]
        public void RegisterMethod_With_ValidCollectionOfProcessors()
        {
            TextProcessingManager manager = new TextProcessingManager();

            //create some processort with valid dependences
            BaseTextProcessor processor1 = new BaseTextProcessor("processor1", new String[]{}, new String[]{"layer1"});
            BaseTextProcessor processor2 = new BaseTextProcessor("processor2", new String[]{"layer1", "layer3"}, new String[]{ "layer2" });
            BaseTextProcessor processor3 = new BaseTextProcessor("processor3", new String[]{}, new String[]{"layer3"});

            //Collection of processors
            BaseTextProcessor[] validCollection = new BaseTextProcessor[] { processor2, processor1, processor3 };

            manager.Register(validCollection);
        }

        static void Main(string[] args)
        {
            new TextProcessingManagerUnitTestClass().RegisterMethod_With_ValidCollectionOfProcessors();
        }
    }
}
