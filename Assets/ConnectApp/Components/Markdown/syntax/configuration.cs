using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Unity.UIWidgets.painting;
using FontStyle = Unity.UIWidgets.ui.FontStyle;
using Color = Unity.UIWidgets.ui.Color;

namespace SyntaxHighlight {
    public interface IConfiguration {
        IDictionary<string, Definition> Definitions { get; }
    }


    public class XmlConfiguration : IConfiguration {
        private IDictionary<string, Definition> definitions;
        public IDictionary<string, Definition> Definitions {
            get { return this.GetDefinitions(); }
        }

        public XDocument XmlDocument { get; protected set; }

        public XmlConfiguration(XDocument xmlDocument) {
            if(xmlDocument == null) {
                throw new ArgumentNullException("xmlDocument");
            }

            this.XmlDocument = xmlDocument;
        }

        protected XmlConfiguration() {
        }

        private IDictionary<string, Definition> GetDefinitions() {
            if(this.definitions == null) {
                this.definitions = this.XmlDocument
                    .Descendants("definition")
                    .Select(selector: this.GetDefinition)
                    .ToDictionary(x => x.Name);
            }

            return this.definitions;
        }

        private Definition GetDefinition(XElement definitionElement) {
            var name = definitionElement.GetAttributeValue("name");
            var patterns = this.GetPatterns(definitionElement);
            var caseSensitive = Boolean.Parse(definitionElement.GetAttributeValue("caseSensitive"));
            var style = this.GetDefinitionStyle(definitionElement);

            return new Definition(name, caseSensitive, style, patterns);
        }

        private IDictionary<string, Pattern> GetPatterns(XContainer definitionElement) {
            var patterns = definitionElement
                .Descendants("pattern")
                .Select(this.GetPattern)
                .ToDictionary(x => x.Name);

            return patterns;
        }

        private Pattern GetPattern(XElement patternElement) {
            const StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
            var patternType = patternElement.GetAttributeValue("type");
            if(patternType.Equals("block", stringComparison)) {
                return this.GetBlockPattern(patternElement);
            }
            if(patternType.Equals("markup", stringComparison)) {
                return this.GetMarkupPattern(patternElement);
            }
            if(patternType.Equals("word", stringComparison)) {
                return this.GetWordPattern(patternElement);
            }

            throw new InvalidOperationException(String.Format("Unknown pattern type: {0}", patternType));
        }

        private BlockPattern GetBlockPattern(XElement patternElement) {
            var name = patternElement.GetAttributeValue("name");
            var style = this.GetPatternStyle(patternElement);
            var beginsWith = patternElement.GetAttributeValue("beginsWith");
            var endsWith = patternElement.GetAttributeValue("endsWith");
            var escapesWith = patternElement.GetAttributeValue("escapesWith");

            return new BlockPattern(name, style, beginsWith, endsWith, escapesWith);
        }

        private MarkupPattern GetMarkupPattern(XElement patternElement) {
            var name = patternElement.GetAttributeValue("name");
            var style = this.GetPatternStyle(patternElement);
            var highlightAttributes = Boolean.Parse(patternElement.GetAttributeValue("highlightAttributes"));
            var bracketColors = this.GetMarkupPatternBracketColors(patternElement);
            var attributeNameColors = this.GetMarkupPatternAttributeNameColors(patternElement);
            var attributeValueColors = this.GetMarkupPatternAttributeValueColors(patternElement);

            return new MarkupPattern(name, style, highlightAttributes, bracketColors, attributeNameColors, attributeValueColors);
        }

        private WordPattern GetWordPattern(XElement patternElement) {
            var name = patternElement.GetAttributeValue("name");
            var style = this.GetPatternStyle(patternElement);
            var words = this.GetPatternWords(patternElement);

            return new WordPattern(name, style, words);
        }

        private IEnumerable<string> GetPatternWords(XContainer patternElement) {
            var words = new List<string>();
            var wordElements = patternElement.Descendants("word");
            if(wordElements != null) {
                words.AddRange(from wordElement in wordElements select Regex.Escape(wordElement.Value));
            }

            return words;
        }

        private Style GetPatternStyle(XContainer patternElement) {
            var fontElement = patternElement.Descendants("font").Single();
            var colors = this.GetPatternColors(fontElement);
            var font = this.GetPatternFont(fontElement);

            return new Style(colors, font);
        }

        private ColorPair GetPatternColors(XElement fontElement) {
            var foreColor = this.ColorFromName(fontElement.GetAttributeValue("foreColor"));
            var backColor = this.ColorFromName(fontElement.GetAttributeValue("backColor"));

            return new ColorPair(foreColor, backColor);
        }

        // A helper function to get ui.Color directly from color name
        // with the help of System.Drawing.Color
        private Color ColorFromName(string name) {
            return new Color(ColorsFromName.ColorFromName(name));
        }

        private TextStyle GetPatternFont(XElement fontElement, TextStyle defaultFont = null) {
            var fontFamily = fontElement.GetAttributeValue("name");
            if(fontFamily != null) {
                var emSize = fontElement.GetAttributeValue("size").ToSingle(11f);
                var height = fontElement.GetAttributeValue("height").ToSingle(1.46f);
                var style = Enum<FontStyle>.Parse(fontElement.GetAttributeValue("style"), FontStyle.normal, true);

                return new TextStyle(
                    fontFamily: fontFamily,
                    fontSize: emSize,
                    fontStyle: style,
                    height: height
                );
            }

            return defaultFont;
        }

        private ColorPair GetMarkupPatternBracketColors(XContainer patternElement) {
            const string descendantName = "bracketStyle";
            return this.GetMarkupPatternColors(patternElement, descendantName);
        }

        private ColorPair GetMarkupPatternAttributeNameColors(XContainer patternElement) {
            const string descendantName = "attributeNameStyle";
            return this.GetMarkupPatternColors(patternElement, descendantName);
        }

        private ColorPair GetMarkupPatternAttributeValueColors(XContainer patternElement) {
            const string descendantName = "attributeValueStyle";
            return this.GetMarkupPatternColors(patternElement, descendantName);
        }

        private ColorPair GetMarkupPatternColors(XContainer patternElement, XName descendantName) {
            var fontElement = patternElement.Descendants("font").Single();
            var element = fontElement.Descendants(descendantName).SingleOrDefault();
            if(element != null) {
                var colors = this.GetPatternColors(element);

                return colors;
            }

            return null;
        }

        private Style GetDefinitionStyle(XNode definitionElement) {
            const string xpath = "default/font";
            var fontElement = definitionElement.XPathSelectElement(xpath);
            var colors = this.GetDefinitionColors(fontElement);
            var font = this.GetDefinitionFont(fontElement);
            return new Style(colors, font);
        }

        private ColorPair GetDefinitionColors(XElement fontElement) {
            var foreColor = this.ColorFromName(fontElement.GetAttributeValue("foreColor"));
            var backColor = this.ColorFromName(fontElement.GetAttributeValue("backColor"));
            return new ColorPair(foreColor, backColor);
        }

        private TextStyle GetDefinitionFont(XElement fontElement) {
            var fontName = fontElement.GetAttributeValue("name");
            var fontSize = Convert.ToSingle(fontElement.GetAttributeValue("size"));
            var fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), fontElement.GetAttributeValue("style"), true);

            return new TextStyle(fontFamily: fontName, fontSize: fontSize, fontStyle: fontStyle);
        }
    }

    internal static class XmlExtensions {
        public static string GetAttributeValue(this XElement element, XName name) {
            if(element == null) {
                throw new ArgumentNullException("element");
            }

            var attribute = element.Attribute(name);
            if(attribute == null) {
                return null;
            }

            return attribute.Value;
        }
    }

    internal static class StringExtensions {
        public static float ToSingle(this string input, float defaultValue) {
            var result = default(float);
            if(Single.TryParse(input, out result)) {
                return result;
            }

            return defaultValue;
        }
    }

    internal static class Enum<T> where T : struct {
        public static T Parse(string value, T defaultValue) {
            return Parse(value, defaultValue, false);
        }

        public static T Parse(string value, T defaultValue, bool ignoreCase) {
            var result = default(T);
            if(Enum.TryParse(value, ignoreCase, out result)) {
                return result;
            }

            return defaultValue;
        }
    }
}