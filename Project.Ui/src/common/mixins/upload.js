
import firebase from '../firebase/firebase';
import Api from '../api'

export default {
    data: () => ({
        uploadFolder: null
    }),
    computed: {
        uploadSubFolder() {
            return null;
        }
    },
    methods: {
        generateFileName() {
            return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
                (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
            )
        },
        getFileExtension(filename) {
            return (/[.]/.exec(filename)) ? /[^.]+$/.exec(filename)[0] : "txt";
        },

        handleImageAddedTextEdit(file, Editor, cursorLocation, resetUploader) {
            this.executeUpload(file, url => {
                Editor.insertEmbed(cursorLocation, 'image', url);
                resetUploader();
            });
        },

        executeUpload(file, callback) {

            var extension = this.getFileExtension(file.name);
            var name = this.generateFileName() + "." + extension;

            var child = "";
            if (this.uploadFolder) child += this.uploadFolder + "/";
            if (this.uploadSubFolder) child += this.uploadSubFolder + "/";

            var _storage = firebase.storage().ref();
            var _reference = _storage.child(child + name);

            _reference.put(file).then(() => {

                if (this.issueId) {
                    new Api('issuedocument').post({
                        issueId: this.issueId,
                        name: file.name,
                        guid: name,
                        type: file.type
                    }).then(() => {
                        this.$eventHub.$emit('reload-issue-document')
                    });
                }

                _reference.getDownloadURL().then(url => {
                    if (callback) callback(url);
                })

            });
        },

        getDownloadLink(name, callback) {

            var child = "";
            if (this.uploadFolder) child += this.uploadFolder + "/";
            if (this.uploadSubFolder) child += this.uploadSubFolder + "/";

            var _storage = firebase.storage().ref();
            var _reference = _storage.child(child + name);

            _reference.getDownloadURL().then(url => {
                callback(url)
            });
        },

    },
}
