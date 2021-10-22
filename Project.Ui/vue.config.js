module.exports = {
    devServer: {
        overlay: {
            warnings: true,
            errors: true
        }
    },

    pages: {
        index: {
            title: 'Challenge',
            entry: 'src/main.js',
            template: 'public/index.html',
            filename: 'index.html',
        },
    },


}