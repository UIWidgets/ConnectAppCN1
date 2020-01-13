using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class DeletedMessage : StatelessWidget {
        public DeletedMessage(
            Key key = null
        ) : base(key: key) {
        }

        const string _content = "此消息已被删除";
        static readonly EdgeInsets _contentPadding = EdgeInsets.symmetric(8, 12);
        static readonly TextStyle _contentStyle = CTextStyle.PLargeBody4;

        public static float CalculateTextHeight(float width) {
            var contentHeight = CTextUtils.CalculateTextHeight(text: _content, textStyle: _contentStyle,
                width - _contentPadding.horizontal);
            return _contentPadding.vertical + contentHeight;
        }

        public override Widget build(BuildContext context) {
            return new Container(
                padding: _contentPadding,
                child: new Text(
                    data: _content,
                    style: _contentStyle
                )
            );
        }
    }

    public class TextMessage : StatelessWidget {
        public TextMessage(
            ChannelMessageView message,
            MentionTapCallback onTap,
            Key key = null
        ) : base(key: key) {
            this.message = message;
            this.onTap = onTap;
        }

        readonly ChannelMessageView message;
        readonly MentionTapCallback onTap;

        static readonly EdgeInsets _contentPadding = EdgeInsets.symmetric(8, 12);
        static readonly TextStyle _contentStyle = CTextStyle.PLargeBlack;

        public static float CalculateTextHeight(string content, float width) {
            var contentHeight = CTextUtils.CalculateTextHeight(
                text: MessageUtils.truncateMessage(content),
                textStyle: _contentStyle,
                textWidth: width - _contentPadding.horizontal);
            return _contentPadding.vertical + contentHeight;
        }

        public override Widget build(BuildContext context) {
            if (this.message == null) {
                return new Container();
            }

            if (this.message.content.isEmpty()) {
                return new Container();
            }

            return new Container(
                padding: _contentPadding,
                child: new RichText(
                    text: new TextSpan(
                        children: MessageUtils.messageWithMarkdownToTextSpans(
                            content: this.message.status == "normal"
                                ? MessageUtils.truncateMessage(this.message.content)
                                : MessageUtils.truncateMessage(this.message.plainText),
                            mentions: this.message.mentions,
                            mentionEveryone: this.message.mentionEveryone,
                            onTap: this.onTap,
                            bodyStyle: _contentStyle
                        ).ToList()
                    )
                )
            );
        }
    }

    public class FileMessage : StatelessWidget {
        public FileMessage(
            ChannelMessageView message,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key: key) {
            this.message = message;
            this.onTap = onTap;
        }

        readonly ChannelMessageView message;
        readonly GestureTapCallback onTap;

        static readonly EdgeInsets _filePadding = EdgeInsets.symmetric(12, 16);
        static readonly TextStyle _fileTitleStyle = CTextStyle.PLargeBlack.copyWith(height: 1);
        static readonly TextStyle _fileSizeStyle = CTextStyle.PSmallBody4.copyWith(height: 1);

        public override Widget build(BuildContext context) {
            if (this.message == null) {
                return new Container();
            }

            if (this.message.attachments.isNullOrEmpty()) {
                return new Container();
            }

            var attachment = this.message.attachments.first();
            var content = new Container(
                padding: _filePadding,
                color: CColors.Transparent,
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Text(
                                        data: attachment.filename,
                                        maxLines: 2,
                                        overflow: TextOverflow.ellipsis,
                                        style: _fileTitleStyle
                                    ),
                                    new SizedBox(height: 4),
                                    new Text(
                                        CStringUtils.FileSize(bytes: attachment.size),
                                        maxLines: 1,
                                        overflow: TextOverflow.ellipsis,
                                        style: _fileSizeStyle
                                    )
                                }
                            )
                        ),
                        new SizedBox(width: 16),
                        this._buildFileIcon()
                    }
                )
            );

            if (this.onTap != null) {
                return new GestureDetector(
                    onTap: this.onTap,
                    child: content
                );
            }

            return content;
        }

        Widget _buildFileIcon() {
            var attachment = this.message.attachments.first();
            string imageName;
            if (attachment.contentType == "application/pdf") {
                imageName = "image/pdf-file-icon";
            }
            else if (attachment.contentType == "video/mp4") {
                imageName = "image/video-file-icon";
            }
            else {
                imageName = "image/file-general-icon";
            }

            return Image.asset(
                name: imageName,
                width: 42,
                height: 48,
                fit: BoxFit.fill
            );
        }

        public static float CalculateTextHeight(ChannelMessageView message, float width) {
            var attachment = message.attachments.first();
            var fileTitleHeight = CTextUtils.CalculateTextHeight(text: attachment.filename, textStyle: _fileTitleStyle,
                width - _filePadding.horizontal - 42 - 16, 2);
            var fileSizeHeight = CTextUtils.CalculateTextHeight(CStringUtils.FileSize(bytes: attachment.size),
                textStyle: _fileSizeStyle, width - _filePadding.horizontal - 42 - 16, 1);
            return _filePadding.vertical + fileTitleHeight + fileSizeHeight + 4;
        }
    }

    public class EmbedMessage : StatelessWidget {
        public EmbedMessage(
            ChannelMessageView message,
            MentionTapCallback onClickUser,
            Action<string> onClickUrl,
            Action<string> onClickImage,
            Dictionary<string, string> headers = null,
            Key key = null
        ) : base(key: key) {
            this.message = message;
            this.onClickUser = onClickUser;
            this.onClickUrl = onClickUrl;
            this.onClickImage = onClickImage;
            this.headers = headers;
        }

        readonly ChannelMessageView message;
        readonly MentionTapCallback onClickUser;
        readonly Action<string> onClickUrl;
        readonly Action<string> onClickImage;
        readonly Dictionary<string, string> headers;

        static readonly EdgeInsets _contentPadding = EdgeInsets.all(12);
        static readonly TextStyle _contentStyle = CTextStyle.PLargeBlack;
        static readonly TextStyle _embedTitleStyle = CTextStyle.PLargeMediumBlue;
        static readonly TextStyle _embedDescriptionStyle = CTextStyle.PRegularBody3;

        public override Widget build(BuildContext context) {
            if (this.message == null) {
                return new Container();
            }

            if (this.message.embeds.isNullOrEmpty()) {
                return new Container();
            }

            return new Container(
                padding: _contentPadding,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        this._buildEmbedContent(),
                        new Container(height: 12),
                        this._buildEmbedDescription()
                    }
                )
            );
        }

        Widget _buildEmbedContent() {
            var embedData = this.message.embeds[0].embedData;

            string embedDataUrl;
            if (this.message.type == ChannelMessageType.embedImage) {
                embedDataUrl = embedData.imageUrl;
            }
            else if (this.message.type == ChannelMessageType.embedExternal) {
                embedDataUrl = embedData.url;
            }
            else {
                embedDataUrl = "";
            }

            if (embedDataUrl.isEmpty()) {
                return new Container();
            }

            return new RichText(
                text: new TextSpan(
                    children: MessageUtils.messageWithMarkdownToTextSpans(
                        content: MessageUtils.truncateMessage(this.message.content),
                        mentions: this.message.mentions,
                        mentionEveryone: this.message.mentionEveryone,
                        onTap: this.onClickUser,
                        bodyStyle: _contentStyle,
                        url: this.message.content.Contains(value: embedDataUrl) ? embedDataUrl : null,
                        onClickUrl: this.onClickUrl
                    ).ToList()
                )
            );
        }

        Widget _buildEmbedDescription() {
            if (this.message.type == ChannelMessageType.embedImage) {
                return this._buildEmbedImage();
            }

            if (this.message.type == ChannelMessageType.embedExternal) {
                return this._buildEmbedExternal();
            }

            return new Container();
        }

        Widget _buildEmbedImage() {
            return new GestureDetector(
                // onTap: () => this.onClickImage(obj: this.message.embeds[0].embedData.imageUrl),
                child: new ImageMessage(
                    id: this.message.id,
                    url: this.message.embeds[0].embedData.imageUrl,
                    data: this.message.imageData,
                    size: 140,
                    ratio: 16.0f / 9.0f,
                    srcWidth: this.message.width,
                    srcHeight: this.message.height,
                    headers: this.headers,
                    isOriginalImage: true
                )
            );
        }

        Widget _buildEmbedExternal() {
            var embedData = this.message.embeds[0].embedData;

            return new GestureDetector(
                onTap: () => this.onClickUrl(obj: embedData.url),
                child: new Container(
                    padding: EdgeInsets.all(12),
                    decoration: new BoxDecoration(
                        color: CColors.White,
                        borderRadius: BorderRadius.all(4)
                    ),
                    child: new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            embedData.title.isEmpty()
                                ? new Container()
                                : new Container(
                                    padding: EdgeInsets.only(bottom: 4),
                                    child: new Text(
                                        data: embedData.title,
                                        style: _embedTitleStyle
                                    )
                                ),
                            embedData.description.isEmpty()
                                ? new Container()
                                : new Container(
                                    padding: EdgeInsets.only(bottom: 4),
                                    child: new Text(
                                        data: embedData.description,
                                        style: _embedDescriptionStyle,
                                        maxLines: 4,
                                        overflow: TextOverflow.ellipsis
                                    )
                                ),
                            _buildEmbeddedName(image: embedData.image, name: embedData.name)
                        }
                    )
                )
            );
        }

        static Widget _buildEmbeddedName(string image, string name) {
            if (image.isEmpty() && name.isEmpty()) {
                return new Container();
            }

            return new Row(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: new List<Widget> {
                    image == null
                        ? (Widget) new Container(width: 14, height: 14)
                        : new CachedNetworkImage(
                            src: image,
                            width: 14, height: 14, fit: BoxFit.cover
                        ),
                    new Container(width: 4),
                    new Expanded(
                        child: new Text(
                            name ?? "",
                            style: CTextStyle.PMediumBody.copyWith(height: 1),
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis
                        )
                    )
                }
            );
        }

        public static float CalculateTextHeight(ChannelMessageView message, float width) {
            var contentHeight = CTextUtils.CalculateTextHeight(
                text: MessageUtils.truncateMessage(message.content),
                textStyle: _contentStyle,
                textWidth: width - _contentPadding.horizontal);
            float descriptionHeight;
            if (message.type == ChannelMessageType.embedImage) {
                descriptionHeight = ImageMessage.CalculateTextHeight(message: message);
            }
            else if (message.type == ChannelMessageType.embedExternal) {
                var embedData = message.embeds[0].embedData;
                var embedDataTitleHeight = CTextUtils.CalculateTextHeight(text: embedData.title,
                    textStyle: _embedTitleStyle, width - 48);
                var embedDescriptionHeight = CTextUtils.CalculateTextHeight(text: embedData.description,
                    textStyle: _embedDescriptionStyle, width - 48, 4);
                float embedNameHeight;
                if (embedData.image.isEmpty() && embedData.name.isEmpty()) {
                    embedNameHeight = 0;
                }
                else {
                    embedNameHeight = 22;
                }

                descriptionHeight = embedDataTitleHeight + 4 + embedDescriptionHeight + 4 + embedNameHeight;
            }
            else {
                descriptionHeight = 0;
            }

            return contentHeight + descriptionHeight + _contentPadding.vertical + _contentPadding.vertical;
        }
    }

    public class ImageMessage : StatefulWidget {
        public ImageMessage(
            string id,
            string url,
            byte[] data,
            float size,
            float ratio,
            float srcWidth = 0,
            float srcHeight = 0,
            Dictionary<string, string> headers = null,
            float radius = 10,
            bool isOriginalImage = false,
            Key key = null
        ) : base(key: key) {
            this.id = id;
            this.url = url;
            this.data = data;
            this.size = size;
            this.ratio = ratio;
            this.radius = radius;
            this.headers = headers;
            this.srcWidth = srcWidth;
            this.srcHeight = srcHeight;
            this.isOriginalImage = isOriginalImage;
        }

        public readonly string id;
        public readonly string url;
        public readonly byte[] data;
        public readonly float size;
        public readonly float ratio;
        public readonly float radius;
        public readonly float srcWidth;
        public readonly float srcHeight;
        public readonly Dictionary<string, string> headers;
        public readonly bool isOriginalImage;

        public static float CalculateTextHeight(ChannelMessageView message) {
            if (message.width > message.height * 16.0f / 9.0f) {
                return 140.0f * 9.0f / 16.0f;
            }

            return message.width > message.height
                ? 140.0f * message.height / message.width
                : 140.0f;
        }

        public Size srcSize {
            get {
                if (this.srcWidth != 0 && this.srcHeight != 0) {
                    return new Size(width: this.srcWidth, height: this.srcHeight);
                }

                return null;
            }
        }

        public override State createState() {
            return new _ImageMessageState();
        }
    }

    class _ImageMessageState : State<ImageMessage> {
        Image image;
        Size size;
        ImageStream stream;
        byte[] imageData;

        Size _finalSize(Size size) {
            if (size.width > size.height * this.widget.ratio) {
                return new Size(
                    width: this.widget.size,
                    height: this.widget.size / this.widget.ratio);
            }

            if (size.width > size.height) {
                return new Size(
                    width: this.widget.size,
                    height: this.widget.size / size.width * size.height);
            }

            if (size.width > size.height / this.widget.ratio) {
                return new Size(
                    width: this.widget.size / size.height * size.width,
                    height: this.widget.size);
            }

            return new Size(
                width: this.widget.size / this.widget.ratio,
                height: this.widget.size);
        }

        void _updateSize(ImageInfo info, bool _) {
            this.size = this._finalSize(new Size(info.image.width, info.image.height));

            this.setState(() => { });
        }

        Image _getImage() {
            return this.widget.data == null
                ? CachedNetworkImage.cachedNetworkImage(
                    this.widget.isOriginalImage ? this.widget.url : CImageUtils.SizeToScreenImageUrl(this.widget.url),
                    fit: BoxFit.cover,
                    headers: this.widget.headers)
                : Image.memory(bytes: this.widget.data, fit: BoxFit.cover);
        }

        public override void initState() {
            base.initState();
            if (this.widget.srcSize == null) {
                this.image = this._getImage();
                this.stream = this.image.image
                    .resolve(new ImageConfiguration());
                this.stream.addListener(this._updateSize);
            }
            else {
                this.size = this._finalSize(this.widget.srcSize);
            }
        }

        public override void dispose() {
            this.stream?.removeListener(this._updateSize);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return this.size == null || (this.widget.url == null && this.widget.data == null)
                ? new Container(
                    width: this.widget.size,
                    height: this.widget.size,
                    decoration: new BoxDecoration(
                        CColorUtils.GetSpecificDarkColorFromId(id: this.widget.id),
                        borderRadius: BorderRadius.all(this.widget.radius)
                    ))
                : (Widget) new ClipRRect(
                    borderRadius: BorderRadius.all(this.widget.radius),
                    child: new Container(
                        width: this.size.width,
                        height: this.size.height,
                        color: CColorUtils.GetSpecificDarkColorFromId(id: this.widget.id),
                        child: this._getImage())
                );
        }
    }
}