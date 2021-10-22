import pagination from '@/common/components/pagination'
import Cache from '@/common/cache'
import Datepicker from 'vue-moment-datepicker'

import $ from 'jquery'
import 'jquery-validation'
import '../../assets/vendor/jquery/jquery.validate.pt-br.js'
import * as langPtBr from "vue-moment-datepicker/src/locale/translations/pt-BR";

export default {
    components: { pagination, Datepicker },
    data: () => ({
        mask_data: '##/##/####',
        mask_datahora: '##/##/#### ##:##',
        mask_cpf: '###.###.###-##',
        mask_rg: '##.###.###-#',
        mask_tel: '(##) ####-#####',
        mask_cel: '(##) #####-####',
        mask_cep: '#####-###',
        money: {
            decimal: ',',
            thousands: '.',
            precision: 2,
            masked: true
        },
        datepicker_lang: langPtBr.default,
        datepicker_format: 'dd/MM/yyyy',
    }),
    methods: {
        newGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        },
        getDateNow(days) {
            var data = new Date();

            if (days && days != 0)
                data.setTime(data.getTime() + (days * 24 * 60 * 60 * 1000));

            var dia = data.getDate().toString(),
                diaF = (dia.length == 1) ? '0' + dia : dia,
                mes = (data.getMonth() + 1).toString(), //+1 pois no getMonth Janeiro começa com zero.
                mesF = (mes.length == 1) ? '0' + mes : mes,
                anoF = data.getFullYear();
            return diaF + "/" + mesF + "/" + anoF;
        },
        defaultPageChanged(filter, index) {
            filter.pageIndex = index;
            return filter;
        },
        defaultOrderBy(filter, field) {
            let type = 0;
            if (filter.orderByType == 0) type = 1;
            filter.orderFields = field;
            filter.orderByType = type;
            filter.isOrderByDynamic = true;
            return filter;
        },

        defaultSuccessResult(msg) {
            this.$eventHub.$emit('show-notification', {
                type: 'success',
                title: msg || 'Ação realizada com sucesso.',
            })
        },
        defaultErrorResult(err, modal) {
            if (err && err.data && err.data.errors) {

                var fullMessage = ""

                for (var i = 0; i < err.data.errors.length; i++) {
                    fullMessage += err.data.errors[i] + "<br />";
                }

                this.$eventHub.$emit('show-notification', {
                    type: 'error',
                    text: fullMessage,
                    modal: modal
                })
            }

            else {

                if (typeof (err) == "object") {

                    if (err.status) {
                        this.$eventHub.$emit('show-notification', {
                            type: 'error',
                            title: err.status,
                            text: err.statusText,
                            modal: modal
                        })
                    }
                    else {
                        this.$eventHub.$emit('show-notification', {
                            type: 'error',
                            text: JSON.stringify(err),
                            modal: modal
                        })
                    }

                }
                else if (err == undefined) {

                    this.$eventHub.$emit('show-notification', {
                        type: 'error',
                        text: "Não foi possível se conectar com a API",
                        modal: modal
                    })
                }
                else {
                    this.$eventHub.$emit('show-notification', {
                        type: 'error',
                        text: err,
                        modal: modal
                    })
                }

            }
        },
        configNotification() {
            this.$eventHub.$on('show-notification', (data) => {
                this.$notify({
                    type: data.type,
                    title: data.title,
                    text: data.text,
                    duration: 10000,
                    speed: 1000
                })
            })
        },
        showLoading() {
            this.$eventHub.$emit('show-loading', true)
        },
        hideLoading() {
            this.$eventHub.$emit('show-loading', false)
        },
        failLoading() {
            this.$eventHub.$emit('show-loading', false)
        },

        saveFilters(key, value) {
            Cache.add("FILTER_" + key, JSON.stringify(value))
        },
        loadFilters(key) {
            return JSON.parse(Cache.get("FILTER_" + key) || null)
        },

        formValidate(form) {

            var _form = this.$refs[form || this.form];

            $(_form).validate({
                errorPlacement: (error, element) => {
                    $(element).css("background", "#ff959594")
                },
            })

            return $(_form).valid();
        },


    },
    mounted() {
        window.scrollTo(0, 0);
    },
}
