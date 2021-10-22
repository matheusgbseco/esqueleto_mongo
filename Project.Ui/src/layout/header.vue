<template>

    <fragment>

        <nav class="navbar navbar-top" id="navbar-main" v-bind:class="{ 'bg-white': mobile, 'navbar-dark': !mobile }">
            <div class="container-fluid">

                <a class="h4 mb-0 text-uppercase d-inline-block" @click="toggleMenu()" v-bind:class="{ 'text-white': !mobile }">
                    <i class="fas fa-bars fa-2x"></i>
                </a>

                <ul class="navbar-nav align-items-center d-flex">
                    <li class="nav-item dropdown">
                        <div class="nav-link pr-0">
                            <div class="media align-items-center">
                                <div class="media-body ml-2 d-block">
                                    <span class="mb-0 text-sm font-weight-bold" v-if="user && user.pessoa">{{ user.pessoa.nome }}</span>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>

        <div id="header-background" class="header pb-8 pt-5 pt-md-8"
             v-bind:style="{ 'background': 'linear-gradient(87deg, #42d1f5 0, #42d1f5 160%) !important' }"></div>

    </fragment>

</template>
<script>

    import Cache from '@/common/cache'
    import Auth from '@/common/auth'

    export default {
        name: "header-app",
        data() {
            return {
                user: null,
                mobile: false
            }
        },
        methods: {
            async toggleMenu() {
                var menuOpen = await this.getMenuOpen();
                Cache.add("MENU_OPEN", !menuOpen);

                if (this.mobile)
                    this.$eventHub.$emit('show-menu', true);
                else
                    document.body.classList.toggle('navbar-hidden');
            },
            async getMenuOpen() {
                return JSON.parse(Cache.get("MENU_OPEN") || true);
            },

        },
        async mounted() {

            this.mobile = window.innerWidth <= 768;

            if (this.mobile)
                document.body.classList.remove('navbar-hidden');
            else {
                var menuOpen = await this.getMenuOpen();
                if (menuOpen)
                    document.body.classList.remove('navbar-hidden');
            }

            this.user = Auth.getAdditionalData();
        },

    };
</script>