using System;
using Unity.UIWidgets.painting;

namespace SyntaxHighlight {

    public class Highlighter {
        public IEngine Engine { get; set; }
        public IConfiguration Configuration { get; set; }

        public Highlighter() {
            Engine = new Engine();
            Configuration = new DefaultConfiguration();
        }

        public TextSpan Highlight(string definitionName, string input) {
            if(definitionName == null) {
                throw new ArgumentNullException("definitionName");
            }

            if(Configuration.Definitions.ContainsKey(definitionName)) {
                var definition = Configuration.Definitions[definitionName];
                return Engine.Highlight(definition, input);
            }

            return new TextSpan(text: input);
        }
    }
}