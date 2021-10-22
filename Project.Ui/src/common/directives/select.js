import Vue from 'vue'
import Api from '@/common/api'

import 'choices.js/public/assets/styles/choices.min.css'
import Choices from 'choices.js'

function _addOption(el, text, value, selected, placeholder) {
    var option = document.createElement("option");
    option.text = text;
    option.value = value;
    if (placeholder) option.setAttribute("placeholder", true)
    if (selected) option.selected = true
    el.add(option);
}

function _addItems(el, items, value) {
    for (var i = 0; i < items.length; i++) {
        _addOption(el, items[i].name, items[i].id, items[i].id == value);
    }
}

Vue.directive('select', {
    twoWay: true,
    inserted: (el, binding, vnode) => {

        let isMultiple = el.multiple;
        if (isMultiple == false && binding.value.default)
            _addOption(el, " " + binding.value.default, "", false, true);

        var resource = binding.value.dataitem + "/dataitems";

        var filters = {};
        if (binding.value.filters)
            filters = binding.value.filters;

        var api = new Api(resource);
        api.dataItem(filters).then(result => {

            var value = getVModelValue(vnode);

            if (result.data)
                _addItems(el, result.data, value);

            setTimeout(() => {

                new Choices(el, {
                    placeholder: true,
                    placeholderValue: "Selecione...",
                    searchPlaceholderValue: "Digite para buscar...",
                    removeItems: true,
                    removeItemButton: true,
                    shouldSort: false,
                    shouldSortItems: false,
                    itemSelectText: 'Selecione',
                    searchResultLimit: 10
                });

                //if (value) choices.setValue(value);

                el.oninvalid = () => {
                    let elParent = el.parentElement;
                    if (elParent) {
                        elParent.style.cssText = `
                            color: #b7291c !important;
                            border-color: #dc3545 !important;
                            padding-right: calc(1.5em + 0.75rem) !important;`
                    }
                };

                el.onchange = () => {

                    let wasValidated = el.closest(".was-validated");
                    if (wasValidated) {
                        let isValid = el.checkValidity();
                        if (isValid) {
                            let elParent = el.parentElement;
                            if (elParent) elParent.style.cssText = '';
                        }
                    }

                };

            }, 500)

        });

        function findVModelName(vnode) {
            return vnode.data.directives.find(function (o) {
                return o.name === 'model';
            }).expression;
        }

        function getVModelValue(vnode) {
            var prop = findVModelName(vnode)
            return eval('vnode.context.' + prop)
        }

    }
})