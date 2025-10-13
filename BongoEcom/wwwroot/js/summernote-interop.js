
window.summernoteInterop = {
    init: function (elementId, dotNetHelper) {
        $('#' + elementId).summernote({
            height: 250,
            callbacks: {
                onChange: function (contents) {
                    dotNetHelper.invokeMethodAsync('OnContentChanged', contents);
                }
            }
        });
    },
    getContent: function (elementId) {
        return $('#' + elementId).summernote('code');
    },
    setContent: function (elementId, content) {
        $('#' + elementId).summernote('code', content);
    },
    destroy: function (elementId) {
        $('#' + elementId).summernote('destroy');
    }
};
