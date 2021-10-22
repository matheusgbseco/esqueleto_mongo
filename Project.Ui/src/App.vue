<template>
    <div>
        <loading />
        <fade-transition origin="center" mode="out-in" :duration="250">
            <router-view />
        </fade-transition>
    </div>
</template>

<script>

    import loading from '@/common/components/loading'

    export default {
        components: {
            loading
        }
    };

</script>


<style lang="scss">
</style>
