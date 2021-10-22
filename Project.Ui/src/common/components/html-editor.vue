<template>
    <div v-bind:id="editorId" class="unlayer-editor" v-bind:style="{ height: height }"></div>
</template>

<script>
    import { loadScript } from './html-editor-scripts';

    let lastEditorId = 0;

    export default {
        name: 'html-editor',
        props: {
            options: Object,
            projectId: Number,
            tools: Object,
            appearance: Object,
            locale: String,
            height: {
                type: String,
                default: '500px',
            },
        },
        computed: {
            editorId() {
                return `editor-${++lastEditorId}`;
            }
        },
        created() {
        },
        mounted() {
            loadScript(this.loadEditor.bind(this));
        },
        methods: {
            loadEditor() {
                const options = this.options || {};

                if (this.projectId) {
                    options.projectId = this.projectId
                }

                if (this.tools) {
                    options.tools = this.tools
                }

                if (this.appearance) {
                    options.appearance = this.appearance
                }

                if (this.locale) {
                    options.locale = this.locale
                }

                /* global unlayer */
                this.editor = unlayer.createEditor({
                    ...options,
                    id: this.editorId,
                    fonts: {
                        showDefaultFonts: true,
                        customFonts: [
                            {
                                label: "Sulphur Point",
                                value: "'Sulphur Point', sans-serif"
                            },
                            {
                                label: "Sacramento",
                                value: "'Sacramento', cursive"
                            },
                            {
                                label: "Arial",
                                value: "Arial, sans-serif"
                            },
                            {
                                label: "Anton",
                                value: "'Anton', sans-serif",
                                url: "https://fonts.googleapis.com/css?family=Anton"
                            },
                            {
                                label: "Georgia",
                                value: "Georgia, Times, 'Times New Roman', serif"
                            },
                            {
                                label: "Helvetica",
                                value: "'Helvetica Neue', Helvetica, Arial, sans-serif",
                            },
                            {
                                label: "Lucida",
                                value: "'Lucida Grande', 'Lucida Sans', Geneva, Verdana, sans-serif"
                            },
                            {
                                label: "Lato",
                                value: "'Lato', Tahoma, Verdana, sans-serif",
                                url: "https://fonts.googleapis.com/css?family=Lato"
                            }
                        ]
                    }
                });


                this.$emit('load');
            },
            loadDesign(design) {
                this.editor.loadDesign(design);
            },
            saveDesign(callback) {
                this.editor.saveDesign(callback);
            },
            exportHtml(callback) {
                this.editor.exportHtml(callback);
            }
        },
    }
</script>

<style scoped>
    .unlayer-editor {
        flex: 1;
        display: flex;
    }
</style>