
import CryptoJS from "crypto-js"

let _storage = window.localStorage;

function _getSalt(key) {
    return key + "lnOywPDcNeNyh&7c97ixysnXTtR";
}

function _get(key, isSession) {
    _setStorage(isSession);
    var result = _storage.getItem(key);
    if (result) {
        try {
            var _salt = _getSalt(key);
            var bytes = CryptoJS.AES.decrypt(result, _salt);
            var originalText = bytes.toString(CryptoJS.enc.Utf8);
            return originalText;
        } catch (e) {
            return result;
        }
    }
    return result;
}

function _add(key, data, isSession) {
    _setStorage(isSession);
    var _salt = _getSalt(key);
    var _newdata = CryptoJS.AES.encrypt(data, _salt).toString();
    _storage.setItem(key, _newdata);
}

function _update(key, data, isSession) {
    _setStorage(isSession);
    var _salt = _getSalt(key);
    var _newdata = CryptoJS.AES.encrypt(data, _salt).toString();
    _storage.setItem(key, _newdata);
}

function _remove(key, isSession) {
    _setStorage(isSession);
    _storage.removeItem(key);
}

function _reset(isSession) {
    _setStorage(isSession);
    _storage.clear();
}

function _setStorage(isSession) {
    if (isSession) _storage = window.sessionStorage;
    else _storage = window.localStorage;
}

export default {
    get: _get,
    add: _add,
    update: _update,
    remove: _remove,
    reset: _reset,
}
