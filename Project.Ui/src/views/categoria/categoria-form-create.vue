<template>

    <form ref="categoria-form-create" v-on:keyup.enter="executeCreate(model)" novalidate>

        <div class="row">

            <div class="form-group col-md-12">
                <label for="nome">Nome</label>
                <input type="text" class="form-control form-control-alternative" name="nome" placeholder="Nome" v-model="model.nome" required />
            </div>

        </div>

        <button type="button" class="btn btn-outline-default" @click="onBack()">
            <span><i class="fas fa-arrow-left"></i> Voltar</span>
        </button>
        <button type="button" class="btn btn-success float-right" @click="executeCreate(model)">
            <span><i class="fas fa-save"></i> Salvar</span>
        </button>

    </form>


</template>
<script>

    import base from '@/common/mixins/base.js'
    import Api from '@/common/api'

    export default {
        name: "categoria-form-create",
        mixins: [base],
        data: () => ({

            model: {},

            form: "categoria-form-create",

        }),

        methods: {

            executeCreate(model) {

                if (this.formValidate() == false)
                    return;

                this.showLoading();

                new Api('categoria').post(model).then(_result => {
                    this.$emit('on-saved', _result)
                    this.defaultSuccessResult();
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
    };
</script>

