


window.testFunctions = {

    getComponentLocation: function (componentId) {
        const component = document.getElementById(componentId);
        const rect = component.getBoundingClientRect();
        return {
            top: rect.top,
            left: rect.left,
            width: rect.width,
            height: rect.height
        };
        return rect;
    },

    setComponentLocation: function (componentId, top, left) {
        const component = document.getElementById(componentId);
        component.style.background = "red";
        component.style.position = "absolute";
        component.style.top = top + "px";
        component.style.left = left + "px";
        component.style.height = "100px";
        component.style.width = "100px";
    },

    focusToSearchTextBox: function (componentId, txtValue) {
        const component = document.getElementById(componentId);
        component.value = txtValue;
        component.focus();
    },
    clearTextBoxValue: function (componentId) {
        const component = document.getElementById(componentId);
        component.value = "";
    },
}
