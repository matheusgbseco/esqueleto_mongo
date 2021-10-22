<template>

    <footer class="footer fixed-bottom">
        <div class="row align-items-center justify-content-around">
            <div class="col-sm-4 text-center">
                <span class="alpha-7">Feito com <i class="fa fa-heart"></i>.</span>
            </div>
            <div class="col-sm-4 mt-4 mt-sm-0 text-center">
                <span class="alpha-7">Todos os direitos reservados © 2021.</span>
            </div>
        </div>
    </footer>

</template>
<script>

    import Cache from '../common/cache'

    export default {
        name: "footer-app",
        methods: {
            clearSessionStorage() {
                Cache.reset(true)
            },
        },
    };
</script>