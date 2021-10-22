import Api from '@/common/api'
import Global from '@/common/global'
import "intro.js/introjs.css"
import "intro.js/themes/introjs-modern.css"

var introJs = require("intro.js/intro.js")

export default {

    steps: [],

    start(url) {
        setTimeout(() => {

            var _intro = introJs()

            _intro.setOption("showProgress", true);
            _intro.setOption("showBullets", false);
            _intro.setOption("showStepNumbers", false);
            _intro.setOption("nextLabel", "Próximo");
            _intro.setOption("prevLabel", "Anterior");
            _intro.setOption("doneLabel", "Concluir");
            _intro.setOption("skipLabel", "Sair");
            _intro.setOption("exitOnEsc", false);
            _intro.setOption("exitOnOverlayClick", false);
            _intro.setOption("disableInteraction", true);

            _intro.oncomplete(() => {
                for (var item of this.steps) {
                    new Api('ColaboradorTutorialPasso', Global.END_POINT_PORTAL_CORPORATIVO).post({                        
                        tutorialPassoId: item.tutorialPassoId
                    }).then(data => { })
                }
            });

            new Api('tutorialpasso/more', Global.END_POINT_PORTAL_CORPORATIVO).get({
                chaveIdentificacaoSistema: Global.CHAVE_IDENTIFICACAO_SISTEMA,
                url: url,
                filterBehavior: "GetDataListCustom"
            }).then(result => {
                this.steps = result.data;
                for (var item of this.steps) {
                    var _element = item.element;
                    
                    if (_element) {
                        _intro.addStep({
                            element: _element,
                            intro: item.intro
                        })
                    }
                }
                _intro.start();
            });
        }, 500)
    }
}