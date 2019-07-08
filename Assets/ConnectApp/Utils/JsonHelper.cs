using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class JsonHelper {
        public static T[] FromJson<T>(string json) {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) {
            Wrapper<T> wrapper = new Wrapper<T> {
                Items = array
            };
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(List<T> list) {
            Wrapper<T> wrapper = new Wrapper<T> {
                Items = list.ToArray()
            };
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) {
            Wrapper<T> wrapper = new Wrapper<T> {
                Items = array
            };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static Dictionary<string, object> ToDictionary(object json) {
            if (json is Dictionary<string, object> dictionary) {
                return dictionary;
            }
            if (json is string jsonString) {
                var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
                return jsonDictionary;
            }
            return new Dictionary<string, object>();
        }

        [Serializable]
        class Wrapper<T> {
            public T[] Items;
        }
    }
}