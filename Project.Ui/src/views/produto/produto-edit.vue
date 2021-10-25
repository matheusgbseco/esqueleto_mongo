<template>

    <div class="container-fluid mt--8">

        <div class="row">
            <div class="col">
                <div class="card shadow">
                    <div class="card-header border-0">
                        <div class="row">
                            <div class="col-xl-6">
                                <h2>
                                    <i class="fas fa-pencil"></i>
                                    Editar Cadastro > Pessoas
                                </h2>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
						<form-edit v-if="dialogEdit" :id="produtoId" @on-saved="onSaved" @on-back="onBack()" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>
<script>

    import formEdit from './produto-form-edit'

    export default {
        name: 'produto-edit',
        components: { formEdit },
        data: () => ({
            dialogEdit: false,
            produtoId: 0
        }),
        methods: {
            onSaved(data) {
                if (data) this.$router.push({ path: '/produto' });
            },
            onBack() {
                this.$router.go(-1);
            }
        },
        mounted() {
            this.produtoId = +this.$route.params.id;
            this.dialogEdit = true;
        }
    };
</script>

