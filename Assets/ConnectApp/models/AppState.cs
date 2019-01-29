using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectApp.models {
  public class AppState {
    private readonly Dictionary<string, object> _state;

    private AppState() {
      _state = new Dictionary<string, object>();
    }

    AppState FromJson(Dictionary<string, object> json) => _dictToState(json);

    Dictionary<string, object> ToJson() => _stateToDict(this);

    public void Set(string key, object value) {
      if (!_checkType(value)) {
        throw new ApplicationException($"Cannot set state {key} using value cannot be serialized.");
      }

      object fVal;
      switch (value) {
        case List<object> objects:
          fVal = _listToState(objects);
          break;
        case Dictionary<string, object> dictionary:
          fVal = _dictToState(dictionary);
          break;
        default:
          fVal = value;
          break;
      }

      this._set(key.Split('.'), fVal);
    }

    private void _set(IReadOnlyList<string> keys, object value) {
      if (keys.Count > 1) {
        if (_state[keys[0]] != null && _state[keys[0]] is AppState appState) {
          appState._set(keys.Skip(1).ToArray(), value);
        }
        else {
          var newState = new AppState();
          newState._set(keys.Skip(1).ToArray(), value);
          _state[keys[0]] = newState;
        }
      }
      else {
        _state[keys[0]] = value;
      }
    }

    public object Get(string key, object defaultValue) {
      return _get(key.Split('.')) ?? defaultValue;
    }

    private object _get(IReadOnlyList<string> keys) {
      if (!_state.ContainsKey(keys[0])) return null;
      var val = _state[keys[0]];
      switch (val) {
        case AppState appState:
          return keys.Count == 1 ? _stateToDict(appState) : appState._get(keys.Skip(1).ToArray());
        case List<object> objects:
          return _stateToList(objects);
        default:
          return val;
      }
    }

    AppState clone() {
      return _cloneState(this);
    }

    private static List<object> _cloneList(IEnumerable<object> list) {
      var cloned = new List<object>();
      foreach (var item in list) {
        switch (item) {
          case List<object> objects:
            cloned.Add(_cloneList(objects));
            break;
          case AppState appState:
            cloned.Add(_cloneState(appState));
            break;
          default:
            cloned.Add(item);
            break;
        }
      }

      return cloned;
    }

    private static AppState _cloneState(AppState state) {
      var cloned = new AppState();
      foreach (var key in state._state.Keys) {
        switch (state._state[key]) {
          case List<object> objects:
            cloned._set(new[] {key}, _cloneList(objects));
            break;
          case AppState appState:
            cloned._set(new[] {key}, _cloneState(appState));
            break;
          default:
            cloned._set(new[] {key}, state._state[key]);
            break;
        }
      }

      return cloned;
    }

    private static Dictionary<string, object> _stateToDict(AppState state) {
      var dict = new Dictionary<string, object>();
      foreach (var key in state._state.Keys) {
        switch (state._state[key]) {
          case AppState appState:
            dict[key] = _stateToDict(appState);
            break;
          case List<object> objects:
            dict[key] = _stateToList(objects);
            break;
          default:
            dict[key] = state._state[key];
            break;
        }
      }

      return dict;
    }

    private static AppState _dictToState(Dictionary<string, object> dict) {
      var state = new AppState();
      foreach (var key in dict.Keys) {
        switch (dict[key]) {
          case Dictionary<string, object> _:
            state._state[key] = _dictToState((Dictionary<string, object>) dict[key]);
            break;
          case List<object> _:
            state._state[key] = _listToState((List<object>) dict[key]);
            break;
          default:
            state._state[key] = dict[key];
            break;
        }
      }

      return state;
    }

    private static List<object> _stateToList(IEnumerable<object> state) {
      var list = new List<object>();
      foreach (var item in state) {
        switch (item) {
          case List<object> objects:
            list.Add(_stateToList(objects));
            break;
          case AppState appState:
            list.Add(_stateToDict(appState));
            break;
          default:
            list.Add(item);
            break;
        }
      }

      return list;
    }

    private static List<object> _listToState(IEnumerable<object> list) {
      var state = new List<object>();
      foreach (var item in list) {
        switch (item) {
          case List<object> objects:
            state.Add(_listToState(objects));
            break;
          case Dictionary<string, object> dictionary:
            state.Add(_dictToState(dictionary));
            break;
          default:
            state.Add(item);
            break;
        }
      }

      return state;
    }

    private static bool _checkType(object value) {
      var type = value;
      if (!(type is int) &&
          !(type is double) &&
          !(type is bool) &&
          !(type is string) &&
          !(type is null) &&
          !(value is List<object>) &&
          !(value is Dictionary<string, object>)) {
        return false;
      }

      if (value is Dictionary<string, object> dictionary) {
        foreach (var key in dictionary.Keys) {
          if (key == null) {
            return false;
          }

          var valid = _checkType(dictionary[key]);
          if (!valid) {
            return false;
          }
        }
      }
      else {
        var objects = (List<object>) value;
        if (objects != null) {
          foreach (var item in objects) {
            var valid = _checkType(item);
            if (!valid) {
              return false;
            }
          }
        }
      }
      return true;
    }

    AppState emptyState() => new AppState();

      AppState initialState() {
        var state = _dictToState(
          new Dictionary<string, object> {
            {
              "login", new Dictionary<string, object> {
                {"email", ""},
                {"loading", false},
                {"isLoggedIn", false}
              }
            }, {
              "settings", new Dictionary<string, object> {
                {"language", "zh_CN"}
              }
            },
            {"users", new Dictionary<string, object>()},
            {"cookies", new List<string>()}
          });
        return state;
      }
    }
}