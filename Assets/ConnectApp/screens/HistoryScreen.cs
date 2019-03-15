using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.widgets;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.painting;

namespace ConnectApp.screens {
    public class HistoryScreen : StatefulWidget {
        
        public HistoryScreen(
            Key key = null
        ) : base(key) {
        }

        public override State createState() => new _HistoryScreenState();
    }
    
    internal class _HistoryScreenState : State<HistoryScreen> {
        
        private int _selectedIndex;
        
        public override void initState() {
            base.initState();
            _selectedIndex = 0;
        }
        
        public override Widget build(BuildContext context) {
            return new SafeArea(
                child: new Container(
                    color: CColors.White,
                    child: new Column(
                        children: new List<Widget> {
                            _buildNavigationBar(context),
                            _buildSelectView()
                        }
                    )
                )
            );
        }
        
        private static Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context).size.width,
                height: 140,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.only(16, 10, 16),
                            onPressed: () => Navigator.pop(context),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 28,
                                color: CColors.icon2
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(16, bottom: 12),
                            child: new Text(
                                "浏览历史",
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
            );
        }
        
        private Widget _buildSelectView() {
            return new CustomSegmentedControl(
                new List<string>{"文章", "活动"},
                index => {
                    if (_selectedIndex != index) {
                        setState(() => _selectedIndex = index);
                    }
                },
                _selectedIndex
            );
        }
    }
}