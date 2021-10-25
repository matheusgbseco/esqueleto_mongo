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
						<form-edit v-if="dialogEdit" :id="categoriaId" @on-saved="onSaved" @on-back="onBack()" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>
<script>

    import formEdit from './categoria-form-edit'

    export default {
        name: 'categoria-edit',
        components: { formEdit },
        data: () => ({
            dialogEdit: false,
            categoriaId: 0
        }),
        methods: {
            onSaved(data) {
				if (data) this.$router.push({ path: '/categoria' });
            },
            onBack() {
                this.$router.go(-1);
            }
        },
        mounted() {
            this.categoriaId = +this.$route.params.id;
            this.dialogEdit = true;
        }
    };
</script>

