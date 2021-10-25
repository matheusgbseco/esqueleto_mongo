import Vue from 'vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import Global from './global.js'
import Auth from './auth.js'
import qs from 'qs'

Vue.use(VueAxios, axios)

export default class Api {

    resourse;
    endpoint;

    constructor(resource, endpoint) {
        this.resourse = resource;
        this.endpoint = endpoint;
    }

    defaultFilter = {
        pageSize: 10,
        pageIndex: 0,
        isPaginate: true,
    };

    byCache = false;
    hasDefaultFilters = true;
    lastAction = "none";
    url = "";

    async upload(file, folder) {

        this.lastAction = "upload";
        this.url = await this.makeUri();

        let formData = new FormData();
        formData.append('files', file, file.name);
        formData.append('folder', folder);

        return axios
            .post(this.url, formData)
            .then(res => { this.handleSuccess(res.data); return res.data; })
            .catch(err => { this.handleError(err.response); throw err.response; })
    }

    async post(model) {

        this.lastAction = "post";
        this.url = await this.makeUri();
        return axios
            .post(this.url, model)
            .then(res => { this.handleSuccess(res.data); return res.data; })
            .catch(err => { this.handleError(err.response); throw err.response; })
    }

    async put(model) {

        this.lastAction = "put";
        this.url = await this.makeUri();

        return axios
            .put(this.url, model)
            .then(res => { this.handleSuccess(res.data); return res.data; })
            .catch(err => { this.handleError(err.response); throw err.response; })
    }

    async delete(model) {

        this.lastAction = "delete";
        this.url = await this.makeUri()

        return axios
            .delete(this.url, { params: model })
            .then(res => { this.handleSuccess(res.data); return res.data; })
            .catch(err => { this.handleError(err.response); throw err.response; })
    }

    async get(filters) {

        this.lastAction = "get";
        this.url = await this.makeUri()

        var _filters = Object.assign(this.defaultFilter, filters);
        if (_filters.id)
            this.url = String.format("{0}/{1}", this.url, _filters.id);

        return axios
            .get(this.url, {
                params: _filters,
                paramsSerializer: () => {
                    return qs.stringify(filters)
                }
            })
            .then(res => { this.handleSuccess(res.data); return res.data; })
            .catch(err => { this.handleError(err.response); throw err.response; })
    }

    async export(filters) {

        this.lastAction = "export";
        this.url = await this.makeUri()

        var _filters = Object.assign(this.defaultFilter, filters);
        if (_filters.id)
            this.url = String.format("{0}/{1}", this.url, _filters.id);

        return axios
            .get(this.url, {
                params: _filters,
                paramsSerializer: () => {
                    return qs.stringify(filters)
                }, responseType: "blob"
            })
            .then(res => { this.handleSuccess(res.data); return res.data; })
            .catch(err => { this.handleError(err.response); throw err.response; })
    }

    async dataItem(filters) {

        this.lastAction = "get";
        this.url = await this.makeUri()

        var _filters = Object.assign(this.defaultFilter, filters);
        if (_filters.id)
            this.url = String.format("{0}/{1}", this.url, _filters.id);

        return axios
            .get(this.url, {
                params: _filters,
                paramsSerializer: () => {
                    return qs.stringify(filters)
                }
            })
            .then(res => { this.handleSuccess(res.data); return res.data; })
            .catch(err => { this.handleError(err.response); throw err.response; })
    }

    async makeUri() {
        await this.configHeaders();
        if (!this.endpoint) this.endpoint = Global.END_POINT_DEFAULT;
        return String.format("{0}/{1}", this.endpoint, this.resourse)
    }

    async configHeaders() {
        let _accesstoken = await Auth.getToken()
        if (_accesstoken) axios.defaults.headers.common['Authorization'] = "Bearer " + _accesstoken;

        let _userId = await Auth.currentUser()
        if (_userId) axios.defaults.headers.common['User-Id'] = _userId.usuarioId;

        axios.defaults.headers.common['Accept-Language'] = "pt-BR";
    }

    async handleSuccess(response) {
        return response;
    }

    async handleError(err) {
        if (err && err.status == 401)
            await Auth.login(true);
    }

}

String.format = function () {
    var theString = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
        theString = theString.replace(regEx, arguments[i]);
    }
    return theString;
}

Object.$httpParamSerializer = function () {
    var obj = arguments[0];
    return Object.entries(obj).map(([key, val]) => `${key}=${val}`).join('&')
}