using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class GlobalLoading : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Center(
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new CustomActivityIndicator(
                                size: 24
                            )
                        }
                    )
                )
            );
        }
    }
}