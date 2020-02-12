using System;
using ConnectApp.Constants;
using Unity.UIWidgets.painting;

namespace SyntaxHighlight {

    public class Highlighter {
        public IEngine Engine { get; set; }
        public IConfiguration Configuration { get; set; }

        public Highlighter() {
            this.Engine = new Engine();
            this.Configuration = new DefaultConfiguration();
        }

        public TextSpan Highlight(string definitionName, string input) {
            if(definitionName == null) {
                throw new ArgumentNullException("definitionName");
            }

            if(this.Configuration.Definitions.ContainsKey(definitionName)) {
                var definition = this.Configuration.Definitions[definitionName];
                return this.Engine.Highlight(definition, input);
            }

            return new TextSpan(text: input, style: CTextStyle.PCodeStyle);
        }
    }
}