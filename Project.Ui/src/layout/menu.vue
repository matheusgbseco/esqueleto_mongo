<template>

    <nav class="navbar navbar-vertical fixed-left navbar-expand-md navbar-light bg-white py-0" id="sidenav-main">
        <div class="container-fluid">

            <div class="collapse navbar-collapse" v-bind:class="{ 'show': menuOpen }">

                <h6 class="navbar-heading text-muted text-right" v-if="mobile" @click="menuOpen = false"><i class="fas fa-times"></i> Fechar</h6>

                <h6 class="navbar-heading text-muted">Bem-vindo</h6>
                <ul class="navbar-nav mb-md-3">
                    <li class="nav-item">
                        <router-link class="nav-link" to="/home">
                            <i class="fas fa-home"></i> Home
                        </router-link>
                    </li>
                </ul>


                <h6 class="navbar-heading text-muted">Opções</h6>
                <ul class="navbar-nav mb-md-3">
                    <li class="nav-item">
                        <router-link class="nav-link" to="/categoria">
                            <i class="fas fa-cogs"></i> Categorias
                        </router-link>
                    </li>
                    <li class="nav-item">
                        <router-link class="nav-link" to="/produto">
                            <i class="fas fa-cogs"></i> Produtos
                        </router-link>
                    </li>
                </ul>

                <h6 class="navbar-heading text-muted">Conta</h6>
                <ul class="navbar-nav mb-md-3">
                    <li class="nav-item">
                        <a class="nav-link" @click="signout()" href="javascript:;">
                            <i class="fas fa-sign-out-alt"></i> Sair
                        </a>
                    </li>
                </ul>


            </div>
        </div>

        <b-modal :no-close-on-backdrop="true" :no-close-on-esc="true" centered v-model="dialogInstallApp" title="Install Application">
            <p v-if="canInstallApp">Click below to install as an application...</p>
            <p v-if="!canInstallApp">Don't you already have the app installed? If not, your browser does not support</p>
            <template slot="modal-footer" v-if="canInstallApp">
                <button class="btn btn-success btn-block" @click="installApp()">
                    Install <i class="fas fa-rocket"></i>
                </button>
            </template>
            <template slot="modal-footer" v-if="!canInstallApp">
                <button class="btn btn-danger btn-block" disabled>
                    Can't install <i class="far fa-frown"></i>
                </button>
            </template>
        </b-modal>

    </nav>

</template>
<script>

    import Cache from '@/common/cache'
    import Auth from '@/common/auth'

    export default {
        name: "menu-app",
        data() {
            return {
                mobile: false,
                menuOpen: false,
                dialogInstallApp: false,
                canInstallApp: false
            }
        },

        methods: {
            async signout() {
                await Auth.logout();
            },
            async onSelectOrganization(organization) {
                Cache.add("ORGANIZATION_SELECTED", organization.organizationId);
                this.$router.go()
            },
            async installApp() {
                this.dialogInstallApp = false;
                this._installAppEvent.prompt();
                this._installAppEvent.userChoice.then(() => {
                    this._installAppEvent = null;
                    this.canInstallApp = false;
                    Cache.add("APPVERIFIED", true)
                });
            }
        },
        async mounted() {

            this.mobile = window.innerWidth <= 768;

            setTimeout(() => {
                this.canInstallApp = this._canInstallApp;

                //let appVerified = JSON.parse(Cache.get("APPVERIFIED") || false);
                //if (appVerified == false && this.canInstallApp)
                //    this.dialogInstallApp = true
            }, 1000)

            this.$eventHub.$on('show-menu', show => {
                this.menuOpen = show;
            })
        },
        beforeDestroy() {
            this.$eventHub.$off('show-menu');
        },
        watch: {
            $route() {
                this.menuOpen = false;
            }
        }
    };
</script>

<style scoped>
    .active {
        font-weight: bold;
    }
</style>