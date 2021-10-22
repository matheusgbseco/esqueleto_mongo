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
						<form-edit v-if="dialogEdit" :id="pessoaId" @on-saved="onSaved" @on-back="onBack()" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>
<script>

    import formEdit from './pessoa-form-edit'

    export default {
        name: 'pessoa-edit',
        components: { formEdit },
        data: () => ({
            dialogEdit: false,
			pessoaId: 0
        }),
        methods: {
            onSaved(data) {
				if (data) this.$router.push({ path: '/pessoa' });
            },
            onBack() {
                this.$router.go(-1);
            }
        },
        mounted() {
            this.pessoaId = +this.$route.params.id;
            this.dialogEdit = true;
        }
    };
</script>

