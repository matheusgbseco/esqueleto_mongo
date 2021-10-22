import Vue from 'vue'
import Auth from '../auth'

Vue.directive('canaccess', {
    twoWay: true,
    bind: function (el, binding) {
        var can = Auth.canAccess(binding.value);
        if (!can)
            el.style.display = 'none'
    }
})