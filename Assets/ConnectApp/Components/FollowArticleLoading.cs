using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class FollowArticleLoading : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.Separator2,
                child: ListView.builder(
                    physics: new NeverScrollableScrollPhysics(),
                    itemCount: 6,
                    itemBuilder: (cxt, index) => {
                        if (index == 0) {
                            return this._buildHead(context);
                        }

                        return this._buildContent();
                    }
                )
            );
        }

        Widget _buildHead(BuildContext context) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 48, left: 16, right: 16, top: 24),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            height: 14,
                            width: 64,
                            color: new Color(0xFFF8F8F8)
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 16),
                            child: new Row(
                                children: new List<Widget> {
                                    _Avatar(context),
                                    _Avatar(context),
                                    _Avatar(context),
                                    _Avatar(context),
                                    _Avatar(context)
                                }
                            )
                        )
                    }
                )
            );
        }

        static Widget _Avatar(BuildContext context) {
            var width = (MediaQuery.of(context: context).size.width - 18 * 2 - 40) /
                        5; // 18 是边缘的 padding, 10 是 卡片间的间距
            var size = width - 4 * 2; // 4 是头像的水平 padding
            return new Container(
                width: width,
                alignment: Alignment.center,
                margin: EdgeInsets.symmetric(horizontal: 4),
                child: new Container(
                    width: size,
                    height: size,
                    decoration: new BoxDecoration(
                        new Color(0xFFF8F8F8),
                        borderRadius: BorderRadius.circular(size / 2)
                    )
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                margin: EdgeInsets.only(top: 8),
                padding: EdgeInsets.only(16, 16, 16, 56),
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(bottom: 24),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget> {
                                    new Container(
                                        child: new Row(
                                            children: new List<Widget> {
                                                new Container(
                                                    margin: EdgeInsets.only(right: 8),
                                                    color: new Color(0xFFF8F8F8),
                                                    width: 38, height: 38),
                                                new Column(
                                                    children: new List<Widget> {
                                                        new Container(color: new Color(0xFFF8F8F8), width: 40,
                                                            height: 14,
                                                            margin: EdgeInsets.only(bottom: 12)),
                                                        new Container(color: new Color(0xFFF8F8F8), width: 40,
                                                            height: 6)
                                                    })
                                            })),
                                    new Container(color: new Color(0xFFF8F8F8), width: 40, height: 12)
                                })),
                        new Row(children: new List<Widget> {
                            new Expanded(child: new Container(height: 12, color: new Color(0xFFF8F8F8)))
                        }),
                        new Container(
                            padding: EdgeInsets.only(top: 16),
                            child: new Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Expanded(
                                        child: new Column(
                                            children: new List<Widget> {
                                                new Container(color: new Color(0xFFF8F8F8), height: 6,
                                                    margin: EdgeInsets.only(bottom: 16)),
                                                new Container(color: new Color(0xFFF8F8F8), height: 6)
                                            })),
                                    new Container(color: new Color(0xFFF8F8F8), width: 100, height: 66,
                                        margin: EdgeInsets.only(16))
                                }))
                    }));
        }
    }
}