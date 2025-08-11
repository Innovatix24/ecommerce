
window.bootstrapInterop = {
    showModal: (selector) => {
        const myModal = new bootstrap.Modal(document.querySelector(selector));
        myModal.show();
    },
    hideModal: (selector) => {
        const myModal = bootstrap.Modal.getInstance(document.querySelector(selector));
        if (myModal) myModal.hide();
    }
};

window.initSidebarToggle = () => {
    const toggleButton = document.getElementById("sidebarToggle");
    const wrapper = document.getElementById("wrapper");

    if (toggleButton) {
        toggleButton.addEventListener("click", () => {
            wrapper.classList.toggle("toggled");
        });
    }
};