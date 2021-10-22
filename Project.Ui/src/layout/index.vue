<template>

    <div>

        <!--<loading />-->

        <menu-app />

        <div class="main-content">
            <header-app />
            <div class="mt--9" v-bind:class="{ 'mt--9': !mobile, 'mt--6': mobile }">
                <fade-transition origin="center" mode="out-in" :duration="250">
                    <router-view :key="$route.fullPath" />
                </fade-transition>
            </div>
            <!--<footer-app />-->
        </div>

    </div>

</template>
<script>

    import menuApp from './menu'
    import headerApp from './header'
    //import footerApp from './footer'
    //import loading from '@/common/components/loading'
    import base from '@/common/mixins/base.js'

    export default {
        name: "Template",
        components: {
            menuApp,
            headerApp,
            //footerApp,
            //loading
        },
        mixins: [base],
        data() {
            return {
                mobile: false

            };
        },
        methods: {

        },
        mounted() {
            this.mobile = window.innerWidth <= 768;

        },
        beforeDestroy() {

        },
    };
</script>
