using System;
using ConnectApp.Models.Model;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class EditPersonalInfoScreenActionModel : BaseActionModel {
        public Action pushToJobRole;
        public Func<string, string, string, string, IPromise> editPersonalInfo;
        public Action<string> changeFullName;
        public Action<string> changeTitle;
        public Action<JobRole> changeJobRole;
        public Action cleanPersonalInfo;
    }
}