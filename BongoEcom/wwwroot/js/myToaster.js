class Toaster {
    constructor(options = {}) {
        this.defaultOptions = {
            position: 'top-right',
            duration: 3000,
            animation: 'fade',
            ...options
        };
        this.toastContainer = null;
        this.initContainer();
    }

    initContainer() {
        // Create container for all toasts
        this.toastContainer = document.createElement('div');
        this.toastContainer.className = `toaster-container ${this.defaultOptions.position}`;
        document.body.appendChild(this.toastContainer);
    }

    show(message, options = {}) {
        const toast = document.createElement('div');
        toast.className = 'toast';

        // Apply custom classes if provided
        if (options.type) {
            toast.classList.add(options.type);
        }

        toast.textContent = message;
        this.toastContainer.appendChild(toast);

        // Auto-remove after duration
        setTimeout(() => {
            toast.remove();
        }, options.duration || this.defaultOptions.duration);

        return toast;
    }

    success(message, options) {
        return this.show(message, { ...options, type: 'success' });
    }

    error(message, options) {
        return this.show(message, { ...options, type: 'error' });
    }
}