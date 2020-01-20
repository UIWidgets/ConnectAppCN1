using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Components.Swiper {
    public class FractionPaginationBuilder : SwiperPlugin {
        public readonly Color color;

        public readonly Color activeColor;

        public readonly float fontSize;

        public readonly float activeFontSize;

        public readonly Key key;

        public FractionPaginationBuilder(
            Color color = null,
            float fontSize = 20.0f,
            Color activeColor = null,
            float activeFontSize = 35.0f,
            Key key = null
        ) {
            this.color = color;
            this.fontSize = fontSize;
            this.key = key;
            this.activeColor = activeColor;
            this.activeFontSize = activeFontSize;
        }

        public override Widget build(BuildContext context, SwiperPluginConfig config) {
            Color activeColor = this.activeColor ?? CColors.PrimaryBlue;
            Color color = this.color ?? CColors.White;

            if (Axis.vertical == config.scrollDirection) {
                return new Column(
                    key: this.key,
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        new Text(
                            $"{config.activeIndex + 1}",
                            style: new TextStyle(color: activeColor, fontSize: this.activeFontSize)
                        ),
                        new Text(
                            "/",
                            style: new TextStyle(color: color, fontSize: this.fontSize)
                        ),
                        new Text(
                            $"{config.itemCount}",
                            style: new TextStyle(color: color, fontSize: this.fontSize)
                        )
                    }
                );
            }
            return new Row(
                key: this.key,
                mainAxisSize: MainAxisSize.min,
                children: new List<Widget> {
                    new Text(
                        $"{config.activeIndex + 1}",
                        style: new TextStyle(color: activeColor, fontSize: this.activeFontSize)
                    ),
                    new Text(
                        $" / {config.itemCount}",
                        style: new TextStyle(color: color, fontSize: this.fontSize)
                    )
                }
            );
        }
    }

    public class RectSwiperPaginationBuilder : SwiperPlugin {
        public readonly Color activeColor;

        public readonly Color color;

        public readonly Size activeSize;

        public readonly Size size;

        public readonly float space;

        public readonly Key key;

        public RectSwiperPaginationBuilder(
            Color activeColor = null,
            Color color = null,
            Size size = null,
            Size activeSize = null,
            float space = 3.0f,
            Key key = null
        ) {
            this.activeColor = activeColor;
            this.color = color;
            this.size = size ?? new Size(10.0f, 2.0f);
            this.activeSize = activeSize ?? new Size(10.0f, 2.0f);
            this.space = space;
            this.key = key;
        }

        public override Widget build(BuildContext context, SwiperPluginConfig config) {
            Color activeColor = this.activeColor ?? CColors.PrimaryBlue;
            Color color = this.color ?? CColors.White;
            List<Widget> list = new List<Widget> ();
            D.assert(() => {
                if (config.itemCount > 20) {
                    Debug.LogWarning(
                        "The itemCount is too big, we suggest use FractionPaginationBuilder instead of DotSwiperPaginationBuilder in this sitituation");
                }

                return true;
            });

            int itemCount = config.itemCount;

            int activeIndex = config.activeIndex.Value;
            for (int i = 0; i < itemCount; ++i) {
                bool active = i == activeIndex;
                Size size = active ? this.activeSize : this.size;
                list.Add(new SizedBox(
                    width: size.width,
                    height: size.height,
                    child: new Container(
                        color: active ? activeColor : color,
                        key: Key.key($"pagination_{i}"),
                        margin: EdgeInsets.all(value: this.space)
                    )
                ));
            }

            if (config.scrollDirection == Axis.vertical) {
                return new Column(
                    key: this.key,
                    mainAxisSize: MainAxisSize.min,
                    children: list
                );
            }
            return new Row(
                key: this.key,
                mainAxisSize: MainAxisSize.min,
                children: list
            );
        }
    }

    public class DotSwiperPaginationBuilder : SwiperPlugin {
        public readonly Color activeColor;

        public readonly Color color;

        public readonly float activeSize;

        public readonly float size;

        public readonly float space;

        public readonly Key key;

        public DotSwiperPaginationBuilder(
            Color activeColor = null,
            Color color = null,
            Key key = null,
            float size = 10.0f,
            float activeSize = 10.0f,
            float space = 3.0f
        ) {
            this.activeColor = activeColor;
            this.color = color;
            this.key = key;
            this.size = size;
            this.activeSize = activeSize;
            this.space = space;
        }

        public override Widget build(BuildContext context, SwiperPluginConfig config) {
            D.assert(() => {
                if (config.itemCount > 20) {
                    Debug.LogWarning(
                        "The itemCount is too big, we suggest use FractionPaginationBuilder instead of DotSwiperPaginationBuilder in this sitituation");
                }

                return true;
            });

            Color activeColor = this.activeColor ?? CColors.PrimaryBlue;
            Color color = this.color ?? CColors.White;

            if (config.indicatorLayout != PageIndicatorLayout.none &&
                config.layout == SwiperLayout.normal) {
                return new PageIndicator(
                    count: config.itemCount,
                    controller: config.pageController,
                    layout: config.indicatorLayout ?? PageIndicatorLayout.slide,
                    size: this.size,
                    activeColor: activeColor,
                    color: color,
                    space: this.space
                );
            }

            List<Widget> list = new List<Widget> ();
            int itemCount = config.itemCount;

            int activeIndex = config.activeIndex.Value;
            for (int i = 0; i < itemCount; ++i) {
                bool active = i == activeIndex;
                float size = active ? this.activeSize : this.size;
                list.Add(new Container(
                    Key.key($"pagination_{i}"),
                    margin: EdgeInsets.all(value: this.space),
                    child: new Container(
                        width: size,
                        height: size,
                        decoration: new BoxDecoration(
                            active ? activeColor : color,
                            borderRadius: BorderRadius.all(size / 2.0f)
                        )
                    )
                ));
            }

            if (config.scrollDirection == Axis.vertical) {
                return new Column(
                    key: this.key,
                    mainAxisSize: MainAxisSize.min,
                    children: list
                );
            }
            return new Row(
                key: this.key,
                mainAxisSize: MainAxisSize.min,
                children: list
            );
        }
    }

    public delegate Widget SwiperPaginationBuilder(BuildContext context, SwiperPluginConfig config);

    public class SwiperCustomPagination : SwiperPlugin {
        public readonly SwiperPaginationBuilder builder;

        public SwiperCustomPagination(
            SwiperPaginationBuilder builder
        ) {
            D.assert(builder != null);
            this.builder = builder;
        }

        public override Widget build(BuildContext context, SwiperPluginConfig config) {
            return this.builder(context: context, config: config);
        }
    }

    public class SwiperPagination : SwiperPlugin {
        public static readonly SwiperPlugin dots = new DotSwiperPaginationBuilder(size: 8, activeSize: 8, color: CColors.DarkGray, activeColor: CColors.White);
        public static readonly SwiperPlugin fraction = new FractionPaginationBuilder();
        public static readonly SwiperPlugin rect = new RectSwiperPaginationBuilder();
        public readonly Alignment alignment;
        public readonly EdgeInsets margin;
        public readonly SwiperPlugin builder;
        public readonly Key key;

        public SwiperPagination(
            Alignment alignment = null,
            Key key = null,
            EdgeInsets margin = null,
            SwiperPlugin builder = null
        ) {
            this.alignment = alignment;
            this.key = key;
            this.margin = margin ?? EdgeInsets.all(10.0f);
            this.builder = builder ?? dots;
        }

        public override Widget build(BuildContext context, SwiperPluginConfig config) {
            Alignment alignment = this.alignment ??
                                  (config.scrollDirection == Axis.horizontal
                                      ? Alignment.bottomCenter
                                      : Alignment.centerRight);

            Widget child = new Container(
                margin: this.margin,
                child: this.builder.build(context: context, config: config)
            );
            if (config.outer != true) {
                child = new Align(
                    key: this.key,
                    alignment: alignment,
                    child: child
                );
            }

            return child;
        }
    }
}