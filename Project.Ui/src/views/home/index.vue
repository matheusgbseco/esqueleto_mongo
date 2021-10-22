<template>
    <div class="container-fluid">

        <div class="row">
            <div class="col">
                <b-card class="shadow mb-3">
                    <div class="row">
                        <div class="col">
                            <h2>Bem-vindo ao sistema administrativo.</h2>
                        </div>

                    </div>
                </b-card>
            </div>
        </div>
    </div>
</template>
<script>
    export default {
        data() {
            return {
                showInstall: false,
                deferredPrompt: null
            }
        },
        methods: {
            irComecar() {
                this.$router.push('/monte-seu-casamento')
            },

            dismiss() {
                this._installAppEvent = null;
                this._canInstallApp = false;
                this.showInstall = false;
            },
            install() {
                this._installAppEvent.prompt();
            }
        },
        created() {

            setTimeout(() => {
                this.showInstall = this._canInstallApp;
                this.deferredPrompt = this._installAppEvent;
            }, 2000)

            window.addEventListener("appinstalled", () => {
                this._installAppEvent = null;
                this._canInstallApp = false;
                this.showInstall = false;
            });

        }
    }
</script>
<style>
</style>
