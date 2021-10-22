<template>
    <main role="main">

        <div class="content-960 mobile-row" style="margin-top:40px;max-width: 470px;text-align: center;">
            <div class="wrapper-960 clearfix">
                <div class="titles">
                    <h2 class="uppercase">
                        <b>Acesso negado</b>
                    </h2>
                </div>
                <br />
                <div class="titles">
                    <i class="fa fa-6x fa-exclamation-triangle" style="font-size: 90px;color: #cc2213;"></i>
                    <br />
                    <em>Você não tem permissão para acessar esta tela! Favor entrar em contato com o Administrador do Sistema.</em>
                </div>
                <div class="row">
                    <div class="col-12 col-sm-12 col-lg-12 col-sm-12" style="text-align:center">
                        <div class="form-group cna-login__group">
                            <a href="/home" class="btn btn-sm btn-default btn-default-app p-2">Ir para home</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </main>

</template>
<script>

    export default {
        name: 'acessonegado',
    }
</script>
