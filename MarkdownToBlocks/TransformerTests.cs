using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MarkdownToBlocks
{
    public class TransformerTests
    {
        private Transformer _transformer;

        public TransformerTests()
        {
            _transformer = new Transformer();

        }

        [Test]
        public void SimpleText_Transform_NothingHappens()
        {
            var input = "asdf";
            var output = _transformer.Transform(input, false, false, false, false, DateTime.Now);
            Assert.AreEqual(output, input);
        }

        [Test]
        public void OneCodeSection_TransformCodeSection_CodeSectionTransformed()
        {
            var input = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestResources\\SingleCodeSectionInput.txt");
            var outputExpected = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestResources\\SingleCodeSectionOutput.txt");
            
            var output = _transformer.Transform(input, true, false, false, false, DateTime.Now);
            Assert.AreEqual(outputExpected, output);
        }

        [Test]
        public void OneImgSection_TransformImg_ImgSectionTransformed()
        {
            var input = "<img src=\"producer-consumer-performance-visual-comparison.png\" alt=\"\"/>";
            var dateTime = new DateTime(2019, 4, 22);
            var outputExpected = "<img src=\"https://michaelscodingspot.com/wp-content/uploads/2019/04/producer-consumer-performance-visual-comparison.png\" alt=\"\"/>";

            var output = _transformer.Transform(input, false, true, false, false
                , dateTime);
            Assert.AreEqual(outputExpected, output);
        }

        [Test]
        public void OneImgSection_AddingAlt_AltAdded()
        {
            var input = "<img src=\"producer-consumer-performance-visual-comparison.png\"/>";
            var outputExpected = "<img src=\"producer-consumer-performance-visual-comparison.png\" alt=\"Producer consumer performance visual comparison\" />";

            var output = _transformer.Transform(input, false, false, true, false
                , DateTime.Now);
            Assert.AreEqual(outputExpected, output);
        }

        [Test]
        public void OneImgSectionWithHttp_AddingAlt_AltAdded()
        {
            var input = "<img src=\"http://FF.jj/GG/producer-consumer-performance-visual-comparison.png\"/>";
            var outputExpected = "<img src=\"http://FF.jj/GG/producer-consumer-performance-visual-comparison.png\" alt=\"Producer consumer performance visual comparison\" />";

            var output = _transformer.Transform(input, false, false, true, false
                , DateTime.Now);
            Assert.AreEqual(outputExpected, output);
        }

        [Test]
        public void OneImgSectionWithAlt_AddingAlt_NotChanged()
        {
            var input = "<img src=\"producer-consumer-performance-visual-comparison.png\" alt=\"aaa\" />";
            var outputExpected = "<img src=\"producer-consumer-performance-visual-comparison.png\" alt=\"aaa\" />";

            var output = _transformer.Transform(input, false, false, true, false
                , DateTime.Now);
            Assert.AreEqual(outputExpected, output);
        }


    }
}
