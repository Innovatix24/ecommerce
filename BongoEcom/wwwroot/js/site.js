
window.bootstrapInterop = {
    showModal: (selector) => {
        const myModal = new bootstrap.Modal(document.querySelector(selector));
        myModal.show();
    },
    hideModal: (selector) => {
        //const myModal = bootstrap.Modal.getInstance(document.querySelector(selector));
        //var element = document.querySelector(selector);
        //element.classList.remove("show");
        //element.style.display = "none";
        //console.log(element);
        //if (myModal) myModal.hide();

        var element = document.querySelector(selector);
        element.style.display = "none";
        element.classList.remove("show");
        document.body.classList.remove("modal-open");
        var backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) {
            backdrop.remove();
        }
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

window.bannerCarousel = {
    init: function (element, dotNetHelper) {
        let startX, movedX;
        const threshold = 50; // Minimum swipe distance

        element.addEventListener('touchstart', handleTouchStart, { passive: true });
        element.addEventListener('touchmove', handleTouchMove, { passive: true });
        element.addEventListener('touchend', handleTouchEnd);

        function handleTouchStart(e) {
            startX = e.touches[0].clientX;
            movedX = startX;
        }

        function handleTouchMove(e) {
            movedX = e.touches[0].clientX;
        }

        function handleTouchEnd() {
            const diffX = movedX - startX;

            if (Math.abs(diffX) > threshold) {
                if (diffX > 0) {
                    dotNetHelper.invokeMethodAsync('HandleSwipe', 'right');
                } else {
                    dotNetHelper.invokeMethodAsync('HandleSwipe', 'left');
                }
            }
        }

        // Store reference for cleanup
        element._swipeHandlers = {
            touchstart: handleTouchStart,
            touchmove: handleTouchMove,
            touchend: handleTouchEnd
        };
    },

    dispose: function (element) {
        if (element._swipeHandlers) {
            element.removeEventListener('touchstart', element._swipeHandlers.touchstart);
            element.removeEventListener('touchmove', element._swipeHandlers.touchmove);
            element.removeEventListener('touchend', element._swipeHandlers.touchend);
            delete element._swipeHandlers;
        }
    }
};


window.getBrowserId = () => {
    let browserId = localStorage.getItem('browserId');
    if (!browserId) {
        browserId = 'browser_' + Math.random().toString(36).substring(2, 15);
        localStorage.setItem('browserId', browserId);
    }
    return browserId;
}