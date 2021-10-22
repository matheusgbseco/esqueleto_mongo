<template>

    <fragment>
        <b-form-file v-model="modelFiles" class="mt-3"
                     placeholder="Escolhe um arquivo ou arraste aqui..."
                     drop-placeholder="Arraste seus arquivos aqui..."
                     @input="selecionaArquivos($event)"
                     accept="image/x-png,image/gif,image/jpeg,video/mp4,video/x-m4v,video/*,image/*"
                     plain
                     multiple>
        </b-form-file>

        <div class="row mt-3">
            <div class="col-2 mb-3" v-for="(file, index) in files" v-bind:key="file.id">

                <div v-if="file.src" class="div-thumbnail" v-bind:style="{ backgroundImage: 'url('+ file.src +')' }" @click="lightboxIndex = index"></div>
                <div v-if="file.uploading" class="div-thumbnail">{{ file.progressUpload }}%</div>

                <div class="row mt-2 no-gutters">
                    <div class="col-8">
                        <small class="font-sm">{{ file.title }}</small>
                    </div>
                    <div class="col-4 text-right">
                        <button class="btn btn-sm btn-primary" @click="lightboxIndex = index"><i class="fas fa-search"></i></button>
                        <button class="btn btn-sm btn-danger" @click="deletarArquivo(file, index)"><i class="far fa-trash-alt"></i></button>
                    </div>
                </div>

            </div>
        </div>

        <CoolLightBox v-if="files"
                      :items="files"
                      :index="lightboxIndex"
                      @close="lightboxIndex = null">
        </CoolLightBox>

    </fragment>

</template>
<script>

    import firebase from '@/common/firebase'
    import base from '@/common/mixins/base.js'

    import Api from '@/common/api'

    export default {
        name: 'midia-upload',
        mixins: [base],
        components: {},
        props: {
            folder: {
                type: String,
                required: true
            },
        },
        data: () => ({
            modelFiles: [],
            files: [],
            lightboxItens: [],
            lightboxIndex: null,
            totalArquivosCarregados: 0,

        }),
        watch: {
            totalArquivosCarregados: function () {
                if (this.files.length == this.totalArquivosCarregados) {
                    this.$emit('upload-realizado', this.files)
                }
            }
        },
        methods: {
            selecionaArquivos(fileList) {

                Array.from(Array(fileList.length).keys()).map(i => {

                    let fileSelected = fileList[i];
                    let fileName = fileSelected.name;
                    let id = this.newGuid();

                    this.files.push({
                        progressUpload: 0,
                        uploadTask: null,
                        uploading: true,
                        uploadEnd: false,
                        src: '',

                        id: id,
                        path: this.folder + '/' + id + '.' + this.getFileExtension(fileName),
                        title: fileName,
                        tipo: this.isImage(fileName) ? 1 : this.isVideo(fileName) ? 2 : 0
                    });

                    const x = this.files.length - 1;

                    this.files[x].uploadTask = firebase.storage().ref(this.files[x].path).put(fileSelected)


                    this.files[x].uploadTask.on('state_changed', (snapshot) => {
                        var progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;

                        this.files[x].progressUpload = progress.toFixed(2)

                        switch (snapshot.state) {
                            case firebase.storage.TaskState.PAUSED:
                                this.defaultSuccessResult("Upload do arquivo pausado.")
                                break;
                        }

                        if (progress >= 100) {
                            this.files[x].uploading = false;
                            this.files[x].uploadEnd = true;
                            this.totalArquivosCarregados += 1;
                        }

                    }, error => {
                        this.defaultErrorResult("Erro ao realizar upload - " + error)
                    }, () => {
                        this.showLoading();
                        this.files[x].uploadTask.snapshot.ref.getDownloadURL().then(src => {
                            new Api("midia").post({
                                midiaId: id,
                                descricao: fileName,
                                url: src,
                                tipo: this.files[x].tipo,
                                caminho: this.files[x].path,
                            }).then(() => {
                                this.hideLoading();
                                this.files[x].src = src;
                            }, error => {
                                this.hideLoading();
                                this.defaultErrorResult(error)
                            })
                        }, error => {
                            this.hideLoading();
                            this.defaultErrorResult("Erro ao obter URL - " + error)
                        });
                    });
                })

            },
            deletarArquivo(file) {

                this.showLoading();

                new Api("midia").delete({
                    midiaId: file.id,
                }).then(() => {

                    this.files = this.files.filter(item => item.id !== file.id);

                    this.defaultSuccessResult("Arquivo excluído com sucesso.")
                    this.$emit('upload-realizado', this.files)

                    this.hideLoading();

                    firebase.storage()
                        .ref(file.path)
                        .delete()
                        .then(() => { })
                        .catch((error) => {
                            this.defaultErrorResult("Erro ao excluir arquivo do storage - " + error)
                        })
                }, error => {
                    this.defaultErrorResult(error)
                })



            },

            isImage(filename) {
                var ext = this.getFileExtension(filename);
                switch (ext.toLowerCase()) {
                    case 'jpg':
                    case 'jpeg':
                    case 'gif':
                    case 'bmp':
                    case 'png':
                        return true;
                }
                return false;
            },

            isVideo(filename) {
                var ext = this.getFileExtension(filename);
                switch (ext.toLowerCase()) {
                    case 'm4v':
                    case 'avi':
                    case 'mpg':
                    case 'mp4':
                        return true;
                }
                return false;
            },

            getFileExtension(filename) {
                return (/[.]/.exec(filename)) ? /[^.]+$/.exec(filename)[0] : "txt";
            }
        },
        mounted() {

        }
    };
</script>

