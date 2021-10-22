import Vue from 'vue'

Vue.prototype._canInstallApp = false;
Vue.prototype._installAppEvent = null;

window.addEventListener('beforeinstallprompt', e => {
    e.preventDefault();
    Vue.prototype._canInstallApp = true;
    Vue.prototype._installAppEvent = e;
});