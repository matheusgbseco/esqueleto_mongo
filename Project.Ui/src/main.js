import Vue from 'vue'
import App from './App.vue'
import router from './router'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import Transitions from 'vue2-transitions'

import './registerServiceWorker'
import './plugins/pwa-install.js'
import './plugins/verify-mobile.js'

import './common/directives/select'
import './common/directives/datepicker'

import 'bootstrap-vue/dist/bootstrap-vue.css'
import './assets/scss/nucleo/css/nucleo.css'
import 'vue-cool-lightbox/dist/vue-cool-lightbox.min.css'
import './assets/scss/main.scss'

import 'sweetalert2/dist/sweetalert2.min.css';

const moment = require('moment')
require('moment/locale/pt-br')
Vue.use(require('vue-moment'), { moment })

import Fragment from 'vue-fragment'
Vue.use(Fragment.Plugin);

import VueSweetalert2 from 'vue-sweetalert2';
Vue.use(VueSweetalert2);

import VueTheMask from 'vue-the-mask'
Vue.use(VueTheMask)

import money from 'v-money'
Vue.use(money)

import CoolLightBox from 'vue-cool-lightbox'
Vue.use(CoolLightBox)

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(Transitions)

Vue.config.productionTip = false

Vue.prototype.$eventHub = new Vue();

new Vue({
    router,
    render: h => h(App),
    mounted() {
        this.$eventHub.$on('show-notification', (data) => {

            var isMobile = IsMobile();

            let _config = {
                position: 'top',
                toast: isMobile,
                icon: data.type,
                title: data.title,
                html: data.text,
                timer: 5000,
                showConfirmButton: false,
            }

            if (data.type == "error") {
                _config.timer = null;
                _config.showCancelButton = true;
                _config.cancelButtonText = "Fechar";
            }

            if (data.modal) {
                _config.position = 'center';
                _config.toast = false;
                _config.grow = false;
                _config.showConfirmButton = true;
                _config.showCancelButton = false;
                _config.timer = null;
                _config.allowOutsideClick = false;
                _config.allowEscapeKey = false;
                _config.allowEnterKey = false;
            }

            function IsMobile() {
                if (navigator.userAgent.match(/Android/i)
                    || navigator.userAgent.match(/webOS/i)
                    || navigator.userAgent.match(/iPhone/i)
                    || navigator.userAgent.match(/iPad/i)
                    || navigator.userAgent.match(/iPod/i)
                    || navigator.userAgent.match(/BlackBerry/i)
                    || navigator.userAgent.match(/Windows Phone/i)
                ) {
                    return false;
                }
                else {
                    return true;
                }
            }

            this.$swal(_config);
        })
    },
    beforeDestroy() {
        this.$eventHub.$off('show-notification');
    },
}).$mount('#app')
