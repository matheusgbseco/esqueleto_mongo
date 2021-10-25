<template>

    <form ref="produto-form-edit" v-on:keyup.enter="executeEdit()" novalidate>

        <div class="row">
            <div class="form-group col-md-12">
                <label for="nome">Nome</label>
                <input type="text" class="form-control form-control-alternative" name="nome" placeholder="Nome" v-model="model.nome" required />
            </div>
            <div class="form-group col-md-12">
                <label for="nome">Categoria</label>
                <select v-select="{ dataitem: 'Categoria', default: 'Selecione' }" v-model="model.categoriaId" class="form-control form-control-alternative" name="categoriaId" required></select>
            </div>
        </div>

        <button type="button" class="btn btn-outline-default" @click="onBack()">
            <span><i class="fas fa-arrow-left"></i> Voltar</span>
        </button>
        <button type="button" class="btn btn-success float-right" @click="executeEdit()">
            <span><i class="fas fa-save"></i> Salvar</span>
        </button>

    </form>


</template>
<script>

    import base from '@/common/mixins/base.js'
    import Api from '@/common/api'

    export default {
        name: "produto-form-edit",
        mixins: [base],
        props: { id: String },
        data: () => ({

            model: {},

            form: "produto-form-edit",

        }),

        methods: {

            executeEdit() {

                if (this.formValidate() == false) return;

                this.showLoading();

                new Api('produto').put(this.model).then(data => {
                    this.defaultSuccessResult();
                    this.$emit('on-saved', data)
                    this.hideLoading();
                }, err => {
                    this.defaultErrorResult(err);
                    this.hideLoading();
                })
            },

            onBack() {
                this.$emit('on-back')
            }
        },

        mounted() {

            this.showLoading();

            new Api('produto/GetById').get({ produtoId: this.id }).then(_result => {
                this.model = Array.isArray(_result.data || {}) ? _result.data[0] : _result.data;
                this.hideLoading();
            }, err => {
                this.defaultErrorResult(err);
                this.hideLoading();
            })
        }
    };
</script>