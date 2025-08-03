
window.jsfunction = {
    focusElement: function (id)
    {
        const element = document.getElementById(id); element.focus();
    },

    getComponentLocation: function (componentId) {
        const component = document.getElementById(componentId);
        const rect = component.getBoundingClientRect();
        return {
            top: rect.top,
            left: rect.left,
            width: rect.width,
            height: rect.height
        };
    },

    clearTextBoxValue: function (componentId) {
        const component = document.getElementById(componentId);
        component.value = "";
    }
}
