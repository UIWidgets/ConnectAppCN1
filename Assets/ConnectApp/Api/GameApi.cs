using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;

namespace ConnectApp.Api {
    public static class GameApi {
        public static Promise<FetchGameResponse> FetchGames(int page) {
            var promise = new Promise<FetchGameResponse>();
            var para = new Dictionary<string, object> {
                {"page", page}
            };
            var request = HttpManager.GET($"{Config.apiAddress}{Config.apiPath}/rankList/gameList", parameter: para);
            HttpManager.resume(request: request).Then(responseText => {
                var gamesResponse = JsonConvert.DeserializeObject<FetchGameResponse>(value: responseText);
                promise.Resolve(value: gamesResponse);
            }).Catch(exception => promise.Reject(ex: exception));
            return promise;
        }
    }
}