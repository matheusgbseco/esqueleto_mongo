import Cache from './cache'
import Global from './global'
import Api from "./api"


export default {

    loadfinished_key: 'PFB___FXX_JB_04',
    lastlocation_key: 'PFB___AEF_03_5O',



    getToken: function () {
        return Cache.get(Global.ID_TOKEN);
    },

    getAccessToken: function () {
        return Cache.get(Global.ACCESS_TOKEN);
    },

    getState: function () {
        return Cache.get(Global.ID_STATE);
    },

    logged: function () {
        return Cache.get(Global.ID_TOKEN) && Cache.get(this.loadfinished_key) == "finished";
    },

    getUser: function (model, onSuccess, onError) {
        new Api("cliente/LoginClienteEcommerce").post(model).then(result => {
            if (onSuccess) onSuccess(result.data);

            if (result.data != null) {

                Cache.add(Global.USER_INFO, JSON.stringify(result.data));
                Cache.add(Global.ID_TOKEN, result.data.token);
                Cache.add(Global.ACCESS_TOKEN, result.data.token);

                Cache.add(this.loadfinished_key, "finished")

                setTimeout(() => {
                    let _lastlocation = Cache.get(this.lastlocation_key);
                    if (_lastlocation == "/auth/forgottenpassword") window.location = "/";
                    if (_lastlocation) window.location = _lastlocation;
                    //else window.location = "/";
                }, 50);
            }
        }, (err) => {
            if (onError) onError(err);
        })
    },

    forgottenPassword: function (model, onSuccess, onError) {
        new Api("cliente/forgottenpassword").post(model).then(result => {
            if (onSuccess) onSuccess(result.data);
        }, (err) => {
            if (onError) onError(err);
        })

    },

    login: function (saveActualPage) {

        if (saveActualPage)
            Cache.add(this.lastlocation_key, window.location.pathname);

        //window.location = "/auth/signin";

        this.reset();
    },


    async currentUser() {
        var _user = JSON.parse(Cache.get(Global.USER_INFO));

        return _user;
    },

    //async getToken() {
    //    var _user = await this.currentUser();
    //    if (_user) {
    //        let _token = await _user.getIdToken();
    //        return _token;
    //    }
    //    return '';
    //},

    async logout() {
        this.reset();
         window.location = "/auth/signin";
    },

    processTokenCallback: function (callback) {
        if (window.location.href.indexOf("access_token") < 0) {
            callback();
            return;
        }

        this.reset();

        var itens = window.location.hash.split('#');
        var result = itens[1].split('&').reduce(function (result, item) {
            var parts = item.split('=');
            result[parts[0]] = parts[1];
            return result;
        }, {});

        Cache.add(Global.ACCESS_TOKEN, result.access_token, result.expires_in);
        Cache.add(Global.ID_TOKEN, result.id_token, result.expires_in);

        setTimeout(() => {
            callback();
        }, 500);

    },


    reset() {
        //Cache.remove(Global.ACCESS_TOKEN);
        //Cache.remove(Global.ID_TOKEN);
        //Cache.remove(Global.USER_INFO);
        Cache.reset();
    },

    getMenu() {
        var _fromcache = Cache.get(this.menu_key);
        return JSON.parse(_fromcache);
    },

    canAccess(url) {

        if (url == '' ||
            url == '/' ||
            url == '/home' ||
            1 == 1)
            return true;

        var permissions = this.getPermissoes();
        var can = false;

        if (url.endsWith("/"))
            url = url.slice(0, -1)

        var rotaSplited = url.split('/');

        for (var key in permissions) {
            var splited = permissions[key].split('/');
            var todosparametrosbatem = false;
            for (var itemrota in rotaSplited) {
                if (splited[itemrota] == "[parametro]" || rotaSplited[itemrota] == splited[itemrota]) {
                    todosparametrosbatem = true;
                }
                else {
                    todosparametrosbatem = false;
                    break;
                }
            }

            if (todosparametrosbatem) {
                can = true;
                break;
            }
        }

        if (!can)
            return false;

        return true;
    }
}
