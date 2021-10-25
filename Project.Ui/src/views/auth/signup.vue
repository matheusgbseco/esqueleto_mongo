<template>

    <div>
        <div class="main-content">

            <div class="header bg-gradient-warning py-5 py-lg-6">
                <div class="container">
                    <div class="header-body text-center mb-7">
                        <div class="row justify-content-center">

                        </div>
                    </div>
                </div>
            </div>
            <!-- Page content -->
            <div class="container mt--8 pb-5">
                <div class="row justify-content-center">
                    <div class="col-lg-5 col-md-7">
                        <div class="card bg-secondary shadow border-0">
                            <div class="card-body px-lg-5 py-lg-5">
                                <div class="text-center text-muted mb-4">
                                    <small>Create your account</small>
                                </div>

                                <form ref="user-form-create" v-on:submit.prevent="createUser()" novalidate>

                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label for="name">Name</label>
                                            <input type="text" class="form-control" name="nome" placeholder="Full Name" v-model="model.nome" required />
                                        </div>
                                        <div class="form-group col-md-12">
                                            <label for="email">login</label>
                                            <input type="text" class="form-control" name="login" placeholder="Login" v-model="model.login" required />
                                        </div>
                                        <div class="form-group col-md-12">
                                            <label for="password">Password</label>
                                            <input type="password" class="form-control" name="senha" placeholder="Senha" v-model="model.senha" required />
                                        </div>
                                        <!--<div class="form-group col-md-6">
                                            <label for="password">Confirm</label>
                                            <input type="password" class="form-control" name="password" placeholder="Confirm your password" v-model="model.passwordConfirm" required />
                                        </div>-->

                                        <div class="col-md-12 text-center">
                                            <button type="button" class="btn btn-primary my-4" @click="createUser()">Confirm</button>
                                        </div>

                                    </div>
                                </form>

                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-6">
                                <a href="/auth/signin" class="text-light"><small>Back</small></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

</template>
<script>

    import Api from "../../common/api";
    import base from "../../common/mixins/base";


    export default {
        name: 'login',
        components: {},
        mixins: [base],
        data() {
            return {
                model: {},
            }
        },
        methods: {
            createUser() {

                //let isValid = await this.formValidate();
                //if (isValid == false)
                //    return;

                new Api("Usuario/CriarUsuario")
                    .post(this.model)
                    .then(() => {
                        this.defaultSuccessResult("Please login");
                        this.$router.push('/auth/signin')
                    }, err => {
                        this.defaultErrorResult(err, true);
                    });
            },
            async formValidate() {
                var $form = this.$refs["user-form-create"];
                $form.classList.add("was-validated");
                return $form.checkValidity();
            },
        },
        mounted() {

        }
    };
</script>
