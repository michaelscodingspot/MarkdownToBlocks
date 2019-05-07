using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace MarkdownToBlocks
{
    public class Transformer
    {
        public string Transform(string inputText, bool changeCodeBlocks, bool changeImageSource, bool changeImageAltLikeName, bool replaceSpecialCharacters, 
            DateTime dateTime)
        {

            var output = inputText;
            if (changeCodeBlocks)
            {
                output = TransformCodeSection(output);
            }
            if (changeImageSource)
            {
                output = TransformImageSource(output, dateTime);
            }

            if (changeImageAltLikeName)
            {
                output = TransformImageAlt(output);
            }

            if (replaceSpecialCharacters)
            {
                output = TransformReplaceSpecialCharacters(output);
            }
            return output;
        }

        private string TransformCodeSection(string text)
        {
            text = text.Replace("<!-- wp:syntaxhighlighter/code -->", "<!-- wp:code -->");
            text = text.Replace("<!-- /wp:syntaxhighlighter/code -->", "<!-- /wp:code -->");

            var startSection = "<pre class=\"wp-block-syntaxhighlighter-code\">";
            var endSection = "</pre>";


            var sectionIndex = text.IndexOf(startSection);
            while (sectionIndex != -1)
            {
                text = ReplaceImmediateText(text, sectionIndex, startSection.Length, "<pre class=\"wp-block-code\"><code>");
                var endIndex = text.IndexOf(endSection, sectionIndex + 1);
                if (endIndex != -1)
                {
                    text = ReplaceImmediateText(text, endIndex, endSection.Length, "</code></pre>");
                }

                sectionIndex = text.IndexOf(startSection);
            }

            return text;
        }

        private string TransformImageSource(string text, DateTime dateTime)
        {
            var month = dateTime.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            var year = dateTime.Year.ToString();
            var prefix = $"https://michaelscodingspot.com/wp-content/uploads/{year}/{month}/";
            string srcEq = "src=\"";
            int indexOfSrc = text.IndexOf(srcEq);
            
            while (indexOfSrc != -1)
            {
                var srcEnd = text.IndexOf("\"", indexOfSrc + 5);
                var srcStart = text.Substring(indexOfSrc, srcEnd-indexOfSrc);
                if (!srcStart.Contains("wp-content"))
                {

                    text = ReplaceImmediateText(text, indexOfSrc, srcEq.Length, srcEq + prefix);
                }
                indexOfSrc = text.IndexOf("src=\"", indexOfSrc + 1);
            }

            return text;
        }

        private string TransformImageAlt(string text)
        {
            string imgTag = "<img";
            int indexOfImgTag = text.IndexOf(imgTag);
            text = text.Replace("alt=\"\"", "");

            while (indexOfImgTag != -1)
            {
                var endImg = text.IndexOf("/>", indexOfImgTag);
                if (!text.Substring(indexOfImgTag, (endImg - indexOfImgTag)).Contains("alt"))
                {
                    var srcStart = text.IndexOf("src", indexOfImgTag);
                    if (srcStart == -1)
                    {
                        indexOfImgTag = text.IndexOf(imgTag, indexOfImgTag + 1);
                        continue;
                    }
                    var endOfSrcTag = text.IndexOf("\"", srcStart + 5);
                    var endOfSrcIndex = text.LastIndexOf(".", endOfSrcTag);
                    var imageName = text.Substring(srcStart + 5, (endOfSrcIndex - srcStart - 5));
                    var alt = imageName.Replace("-", " ").Replace("_", " ");
                    

                    var indexOfSlash = alt.LastIndexOf("/");
                    if (indexOfSlash != -1)
                    {
                        alt = alt.Substring(indexOfSlash + 1);
                    }
                    var firstUpper = char.ToUpper(alt[0]);
                    alt = firstUpper + alt.Substring(1);

                    text = ReplaceImmediateText(text, endImg, 0, $" alt=\"{alt}\" ");
                }
                indexOfImgTag = text.IndexOf(imgTag, indexOfImgTag + 1);
            }

            return text;
        }

        private string TransformReplaceSpecialCharacters(string text)
        {
            return text.Replace("&lt;", "<").Replace("&gt;", ">");
        }

        private string ReplaceImmediateText(string text, int sectionIndex, int sectionLength, string replacement)
        {
            var start = text.Substring(0, sectionIndex);
            var end = text.Substring(sectionIndex + sectionLength);
            return start + replacement + end;
        }
    }
}
