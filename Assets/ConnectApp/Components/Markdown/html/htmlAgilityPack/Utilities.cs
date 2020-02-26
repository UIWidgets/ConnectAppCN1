﻿using System.Collections.Generic;

namespace HtmlAgilityPack {
    static class Utilities {
        public static TValue GetDictionaryValueOrNull<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key)
            where TKey : class {
            return dict.ContainsKey(key) ? dict[key] : default(TValue);
        }
    }
}