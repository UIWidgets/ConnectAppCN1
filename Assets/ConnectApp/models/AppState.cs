using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConnectApp.models {
  public class AppState {
    private readonly Dictionary<string, dynamic> _state;

    AppState() {
      _state = new Dictionary<string, dynamic>();
    }

    AppState fromJson(dynamic json) => _dictToState(json);

    public void set(string key, dynamic value) {
      if (!_checkType(value)) {
        throw new ApplicationException($"Cannot set state {key} using value cannot be serialized.");
      }

      dynamic fVal;
      if (value is List<dynamic>) {
        fVal = _listToState(value);
      }
      else if (value is Dictionary<string, dynamic>) {
        fVal = _dictToState(value);
      }
      else {
        fVal = value;
      }

      this._set(key.Split('.'), fVal);
    }

    private void _set(IReadOnlyList<string> keys, dynamic value) {
      if (keys.Count > 1) {
        if (_state[keys[0]] && _state[keys[0]].GetType() is AppState) {
          (_state[keys[0]] as AppState)?._set(keys.Skip(1).ToArray(), value);
        }
        else {
          var appState = new AppState();
          var subState = appState._set(keys.Skip(1).ToArray(), value);
          _state[keys[0]] = subState;
        }
      }
      else {
        _state[keys[0]] = value;
      }
    }
    
    public dynamic get(string key, dynamic defaultValue) {
      return _get(key.Split('.')) ?? defaultValue;
    }

    private dynamic _get(IReadOnlyList<string> keys) {
      if (!_state.ContainsKey(keys[0])) return null;
      var val = _state[keys[0]];
      if (val.GetType() is AppState) {
        if (keys.Count == 1) {
          return _stateToDict(val);
        }
        return (val as AppState)?._get(keys.Skip(1).ToArray());
      } else if (val is List<dynamic>) {
        return _stateToList(val);
      }
      return val;
    }

    AppState clone() {
      return _cloneState(this);
    }

    Dictionary<string, dynamic> toJson() => _stateToDict(this);

    private static List<dynamic> _cloneList(IEnumerable<dynamic> list) {
      var cloned = new List<dynamic>();
      foreach (var item in list) {
        if (item.GetType() is List<dynamic>) {
          cloned.Add(_cloneList(item));
        } else if (item.GetType() is AppState) {
          cloned.Add(_cloneState(item));
        }
        else {
          cloned.Add(item);
        }
      }
      return cloned;
    }

    private static AppState _cloneState(AppState state) {
      var cloned = new AppState();
      foreach (var key in state._state.Keys) {
        if (state._state[key] is List) {
          cloned._set(new []{key}, _cloneList(state._state[key]));
        } else if (state._state[key].GetType() is AppState) {
          cloned._set(new []{key}, _cloneState(state._state[key]));
        }
        else {
          cloned._set(new []{key}, state._state[key]);
        }
      }
      return cloned;
    }
    
    private static  Dictionary<string, dynamic> _stateToDict(AppState state) {
      var dict = new Dictionary<string, dynamic>();
      foreach (var key in state._state.Keys) {
        if (state._state[key].GetType() is AppState) {
          dict[key] = _stateToDict(state._state[key]);
        } else if (state._state[key].GetType() is List<dynamic>) {
          dict[key] = _stateToList(state._state[key]);
        } else {
          dict[key] = state._state[key];
        }
      }
      return dict;
    }
    
    private static AppState _dictToState(Dictionary<string, dynamic> dict) {
      var state = new AppState();
      foreach (var key in dict.Keys) {
        switch (dict[key]) {
          case Dictionary<string, dynamic> _:
            state._state[key] = _dictToState(dict[key]);
            break;
          case List<dynamic> _:
            state._state[key] = _listToState(dict[key]);
            break;
          default:
            state._state[key] = dict[key];
            break;
        }
      }

      return state;
    }

    private static List<dynamic> _stateToList(List<dynamic> state) {
      var list = new List<dynamic>();
      foreach (var item in state) {
        if (item is List<dynamic>) {
          list.Add(_stateToList(item));
        }
        else if (item.GetType() is AppState) {
          list.Add(_stateToDict(item));
        }
        else {
          list.Add(item);
        }
      }
      return list;
    }
    
    private static List<dynamic> _listToState(IEnumerable<dynamic> list) {
      var state = new List<dynamic>();
      foreach (var item in list) {
        switch (item) {
          case List<dynamic> _:
            state.Add(_listToState(item));
            break;
          case Dictionary<string, dynamic> _:
            state.Add(_dictToState(item));
            break;
          default:
            state.Add(item);
            break;
        }
      }

      return state;
    }

    private static bool _checkType(dynamic value) {
       var type = value.GetType();
      if (!(type is int) && 
          !(type is double) && 
          !(type is bool) && 
          !(type is string) && 
          !(type is null) && 
          !(value is List) && 
          !(value is Dictionary<string, dynamic>)) {
        return false;
      }

      if (value is Dictionary<string, dynamic>) {
        foreach (var key in value.keys) {
          if (!(key.GetType() is string)) {
            return false;
          }
          var valid = _checkType(value[key]);
          if (!valid) {
            return false;
          }
        }
      } else if (value is List) {
        foreach (var item in value) {
          var valid = _checkType(item);
          if (!valid) {
            return false;
          }
        }
      }

      return true;
    }

    AppState emptyState() => new AppState();

    AppState initialState() {
      var state = _dictToState(
        new Dictionary<string, dynamic> {
          {"login", new Dictionary<string, dynamic> {
            {"email", ""},
            {"loading", false},
            {"isLoggedIn", false}
          }},
          {"settings", new Dictionary<string, dynamic> {
            {"language", "zh_CN"}
          }},
          {"users", new Dictionary<string, dynamic>()},
          {"cookies", new List<string>()}
      });
      return state;
    }
  }
}