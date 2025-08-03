
function clearTextBoxValue(componentId) {
    const component = document.getElementById(componentId);
    component.value = "";
}

function setText(componentId, value) {
    const component = document.getElementById(componentId);
    component.value = value;
}