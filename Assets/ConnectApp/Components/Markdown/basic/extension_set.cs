using System.Collections.Generic;

namespace markdown {
    public class ExtensionSet {
        public static ExtensionSet none = new ExtensionSet(new List<BlockSyntax>(), new List<InlineSyntax>());

        public static ExtensionSet commanMark = new ExtensionSet(
            new List<BlockSyntax>() {new FencedCodeBlockSyntax()},
            new List<InlineSyntax>() {new InlineHtmlSyntax()}
        );

        public static ExtensionSet githubWeb = new ExtensionSet(
            new List<BlockSyntax>() {
                new FencedCodeBlockSyntax(),
                new HeaderWithIdSyntax(),
                new SetextHeaderWithIdSyntax(),
                new TableSyntax()
            },
            new List<InlineSyntax>() {
                new InlineHtmlSyntax(),
                new StrikethroughSyntax(),
                new EmojiSyntax(),
                new AutolinkExtensionSyntax()
            }
        );

        public static ExtensionSet githubFlavored = new ExtensionSet(
            new List<BlockSyntax>() {
                new FencedCodeBlockSyntax(),
                new TableSyntax()
            },
            new List<InlineSyntax>() {
                new InlineHtmlSyntax(),
                new StrikethroughSyntax(),
                new AutolinkExtensionSyntax()
            }
        );


        internal List<BlockSyntax> blockSyntaxes;
        internal List<InlineSyntax> inlineSyntaxes;

        public ExtensionSet(List<BlockSyntax> blockSyntaxes, List<InlineSyntax> inlineSyntaxes) {
            this.blockSyntaxes = blockSyntaxes;
            this.inlineSyntaxes = inlineSyntaxes;
        }
    }
}