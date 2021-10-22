<template>

    <div>
        <div class="main-content">

            <div class="header bg-gradient-default py-5 py-lg-6">
                <div class="container">
                    <div class="header-body text-center mb-7">
                        <div class="row justify-content-center">
                            <div class="col-lg-5 col-md-6">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Page content -->
            <div class="container mt--8 pb-5">
                <div class="row justify-content-center">
                    <div class="col-lg-7 col-md-10">
                        <div class="card bg-secondary shadow border-0">

                            <div class="card-header bg-transparent pb-5">
                                <div class="text-muted text-center mt-2 mb-3">
                                    <small>Carregando suas informações, aguarde...</small>
                                </div>
                            </div>

                            <div class="card-body text-center pb-0" v-if="errorMessage">
                                <div class="alert alert-danger">
                                    {{ errorMessage }}
                                </div>  
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

</template>
<script>

    import Auth from "../../common/auth";
    import Api from "../../common/api";

    export default {
        name: 'verify',
        components: {},
        data() {
            return {
                errorMessage: '',
                user: null
            }
        },
        methods: {
            async verifyUser() {
                this.user = await Auth.currentUser() || {};

                console.log("this.user", this.user)

                var _email = this.user.email;
                var _name = this.user.displayName;

                if (!_email && this.user.providerData) {
                    _email = this.user.providerData[0].email;
                    _name = this.user.providerData[0].displayName;
                }

                new Api("currentuser/verify")
                    .post({ email: _email, nome: _name || _email })
                    .then(result => {

                        let _data = result.data;

                        if (_data && _data.pessoaId) {
                            Auth.setUserId(_data.pessoaId);
                            Auth.setAdditionalData(_data);
                            this.$router.push('/home')
                        }

                        else {
                            this.step = '';
                            this.errorMessage = 'Erro...' + result;
                        }

                    });
            },

        },
        async mounted() {
            Auth.logged(async () => {
                await this.verifyUser();
            })
        }
    };
</script>
