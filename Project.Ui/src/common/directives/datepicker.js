import Vue from 'vue'
import Pikaday from 'pikaday'
import 'pikaday/scss/pikaday.scss'


/* desabilitado, estamos utilizando https://www.npmjs.com/package/vue-moment-datepicker */

Vue.directive('datepicker', {
    twoWay: true,
    bind: (el, binding, vnode) => {
        var picker = new Pikaday({
            field: el,
            format: 'DD/MM/YYYY',
            onSelect: () => {
                setVModelValue(picker.toString(), vnode)
            },
            i18n: {
                previousMonth: 'Mês Anterior',
                nextMonth: 'Mês Seguinte',
                months: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                weekdays: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                weekdaysShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab']
            }
        });
        
        function findVModelName(vnode) {
            return vnode.data.directives.find(function (o) {
                return o.name === 'model';
            }).expression;
        }

        function setVModelValue(value, vnode) {
            var prop = findVModelName(vnode)
            eval('vnode.context.' + prop + ' = ' + '"' + value + '"')
        }
    },

})