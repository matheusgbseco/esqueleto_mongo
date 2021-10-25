<template>
    <div class="container-fluid">

        <div class="row">
            <div class="col">
                <b-card class="shadow mb-3">
                    <div class="row">
                        <div class="col">
                            <h2>Bem-vindo a minha estrutura em DDD + SPA.</h2>
                            <p>na parte de Front-End usei VUE + JS, para a criação e interação na interface do sistema</p>
                            <p> Já parte de login e autenticação/api implementei uma nova forma usando os JwtSecurityTokenHandler, ClaimsIdentity, ótima e segura para requisições autenticadas.</p>
                            <p>Para o back-end utilizando estrutura DDD junto com IoC + DI e AutoMapper,</p>
                            <p> a fins de ter um código limpo e estruturado facilitando a manutenção e busca por códigos e seguir o máximo as boas práticas </p>
                            <p>camada 1 - Presentation: fica meu Front-End com Vue e Js utilizando mecanismo de routes</p>
                            <p>camada 2 - API: onde fica todas as controller, start-up e configurações em geral</p>
                            <p>camada 3 - Services: onde ficam os meus DTOs que são os meus objetos do front para minhas minhas controller e também os Services, onde ficam as regras de negócios do meu sistema</p>
                            <p>camada 4 - Domain: a camada onde fica minhas models que é usada para criação de regras dentro dos Services e os meus Repositórios, camada basicamente responsável por fazer consultas</p>
                            <p>camada 5 - Commom: camada onde fica algumas interfaces génericas que são utilizadas em vários serviços ou repositórios</p>
                            <p> Obrigado - Matheus Seco</p>
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
