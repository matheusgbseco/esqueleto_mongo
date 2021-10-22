<template>

    <div class="loader" v-show="isloading">
        <img src="../../assets/loading.svg" alt="Carregando..." />
    </div>

</template>
<script>

    export default {
        name: "loading",
        data() {
            return {
                isloading: false,
                totalRequesting: 0
            };
        },
        watch: {
            totalRequesting: function () {
                if (this.totalRequesting <= 0)
                    this.isloading = false
                else
                    this.isloading = true
            }
        },
        mounted() {
            this.$eventHub.$on('show-loading', (show) => {
                if (show) {
                    this.totalRequesting++;
                }
                else {
                    this.totalRequesting--;
                }
            })
        },
        beforeDestroy() {
            this.$eventHub.$off('show-loading');
        },
    };
</script>
<style>
    .loader {
        position: fixed;
        z-index: 9999;
        background-color: #FFFFFF;
        opacity: 0.7;
        filter: alpha(opacity=70);
        width: 100%;
        height: 100%;
    }

        .loader img {
            display: block;
            margin: 200px auto;
        }

    @-webkit-keyframes sk-rotateplane {
        0% {
            -webkit-transform: perspective(110px) rotateY(0deg);
        }

        25% {
            -webkit-transform: perspective(110px) rotateY(180deg);
        }

        50% {
            -webkit-transform: perspective(110px) rotateY(360deg) rotateX(180deg);
        }

        75% {
            -webkit-transform: perspective(110px) rotateY(180deg);
        }

        100% {
            -webkit-transform: perspective(110px) rotateY(0deg);
        }
    }

    @keyframes sk-rotateplane {

        0% {
            transform: perspective(110px) rotateY(0deg);
            -webkit-transform: perspective(110px) rotateY(0deg);
        }

        25% {
            transform: perspective(110px) rotateY(180deg);
            -webkit-transform: perspective(110px) rotateY(180deg);
        }

        50% {
            transform: perspective(110px) rotateY(360deg);
            -webkit-transform: perspective(110px) rotateY(360deg) rotateX(180deg);
        }

        75% {
            transform: perspective(110px) rotateY(180deg);
            -webkit-transform: perspective(110px) rotateY(180deg);
        }

        100% {
            transform: perspective(110px) rotateY(0deg);
            -webkit-transform: perspective(110px) rotateY(0deg);
        }
    }
</style>