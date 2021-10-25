<template>

    <div class="container-fluid mt--8">

        <div class="row">
            <div class="col">
                <div class="card shadow">
                    <div class="card-header border-0">
                        <div class="row">
                            <div class="col-xl-6">
                                <h2>
                                    <i class="fas fa-plus"></i>
                                    Novo Cadastro > Pessoas
                                </h2>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
						<form-create v-if="dialogCreate" @on-saved="onSaved" @on-back="onBack()" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>
<script>

    import formCreate from './produto-form-create'

    export default {
        name: 'produto-create',
        components: { formCreate },
        data: () => ({
            dialogCreate: false,
        }),
        methods: {
            onSaved(data) {
                this.$router.push({ path: '/produto/edit/' + data.data.produtoId });
            },
            onBack() {
                this.$router.go(-1);
            }
        },
        mounted() {
            this.dialogCreate = true;
        }
    };
</script>

