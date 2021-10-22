<template>

    <form ref="pessoa-form-edit" v-on:keyup.enter="executeEdit(model)" novalidate>

        <div class="row">

            
					<div class="form-group col-md-12">
                        <label for="nome">Nome</label>
                        <input type="text" class="form-control form-control-alternative" name="nome" placeholder="Nome" v-model="model.nome" required />
                    </div>
					<div class="form-group col-md-12">
                        <label for="dataCadastro">DataCadastro</label>
                        <datepicker name="dataCadastro" v-model="model.dataCadastro" input-class="form-control form-control-alternative" placeholder="dataCadastro" :language="datepicker_lang" :format="datepicker_format" required />
                    </div>
					<div class="form-group col-md-12">
                        <label for="email">Email</label>
                        <input type="text" class="form-control form-control-alternative" name="email" placeholder="Email" v-model="model.email"  />
                    </div>


        </div>

        <button type="button" class="btn btn-outline-default" @click="onBack()">
            <span><i class="fas fa-arrow-left"></i> Voltar</span>
        </button>
        <button type="button" class="btn btn-success float-right" @click="executeEdit(model)">
            <span><i class="fas fa-save"></i> Salvar</span>
        </button>

    </form>


</template>
<script>

    import base from '@/common/mixins/base.js'
    import Api from '@/common/api'

    export default {
        name: "pessoa-form-edit",
        mixins: [base],
        props: { id: String },
        data: () => ({

            model: {},

            form: "pessoa-form-edit",

        }),
		
        methods: {

            executeEdit(model) {

                if (this.formValidate() == false) return;

                this.showLoading();
                
                new Api('pessoa').put(model).then(data => {
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
                
            new Api('pessoa').get({ pessoaId: this.id }).then(_result => {
                this.model = Array.isArray(_result.data || {}) ? _result.data[0] : _result.data;
                this.hideLoading();
            }, err => {
                this.defaultErrorResult(err);
                this.hideLoading();
            })
        }
    };
</script>