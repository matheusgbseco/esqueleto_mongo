<template>

    <div>
        <div class="main-content">

            <div class="header bg-gradient-default py-5 py-lg-6">
                <div class="container">
                    <div class="header-body text-center mb-7">
                        <div class="row justify-content-center">
                            <div class="col-lg-5 col-md-6">
                                <h3>Challenger</h3>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Page content -->
            <div class="container mt--8 pb-5">
                <div class="row justify-content-center">
                    <div class="col-lg-5 col-md-7">
                        <div class="card  shadow border-0">

                            <div class="card-body text-center pb-0" v-if="errorMessage">
                                <div class="alert alert-danger">
                                    {{ errorMessage }}
                                </div>
                            </div>

                            <div class="card-body px-lg-5 py-lg-5">
                                <div class="text-center text-muted mb-4">
                                    <small>Utilize seu dados para acesso</small>
                                </div>
                                <form v-on:submit.prevent="loginWithCredencials()" novalidate>
                                    <div class="form-group mb-3">
                                        <div class="input-group input-group-alternative">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><i class="ni ni-email-83"></i></span>
                                            </div>
                                            <input class="form-control" placeholder="E-mail" type="email" v-model="email">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="input-group input-group-alternative">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><i class="ni ni-lock-circle-open"></i></span>
                                            </div>
                                            <input class="form-control" placeholder="Senha" type="password" v-model="password">
                                        </div>
                                    </div>
                                    <div class="text-center">
                                        <button type="button" class="btn btn-primary btn-block my-4" @click="loginWithCredencials()">Acessar</button>
                                        <small>If you don't have an account</small>
                                        <button type="button" class="btn btn-primary btn-block mt-4" @click="goToSignup()">Create Account</button>
                                    </div>
                                </form>
                            </div>

                        </div>
                        <!--<div class="row mt-3">
                            <div class="col-6">
                                <a href="#" class="text-light"><small>Forgot password?</small></a>
                            </div>
                            <div class="col-6 text-right">
                                <a href="/auth/signup" class="text-light"><small>Create new account</small></a>
                            </div>
                        </div>-->
                    </div>
                </div>
            </div>
        </div>

    </div>

</template>
<script>

    //import auth from '@/common/auth'

    //import firebase from "../../common/firebase/index";
    import base from "../../common/mixins/base"

    export default {
        name: 'login',
        components: {},
        mixins: [base],
        data() {
            return {
                email: '',
                password: '',

                errorMessage: ''
            }
        },
        methods: {
            async goToSignup() {
                this.$router.push('/auth/signup')
            },
            async loginWithCredencials() {
                //this.showLoading();
                //this.errorMessage = null;
                //firebase.auth().signInWithEmailAndPassword(this.email, this.password).then(() => {
                //    this.hideLoading();
                //    this.$router.push('/auth/verify')
                //}).catch(error => {
                //    this.hideLoading();
                //    var errorCode = error.code;
                //    var errorMessage = error.message;
                //    if (errorCode === 'auth/wrong-password') {
                //        this.errorMessage = 'Wrong password.'
                //    } else {
                //        this.errorMessage = errorMessage
                //    }
                //});
            }
        },
        mounted() {

        }
    };
</script>
