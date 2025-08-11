
(() => {
    'use strict';

    const NAME = 'modal';
    const Default = {
        backdrop: true,
        keyboard: true,
        focus: true
    };

    class Modal {
        constructor(element, config = {}) {
            this._config = { ...Default, ...config };
            this._element = element;
            this._backdrop = null;
            this._isShown = false;
            this._ignoreBackdropClick = false;
            this._init();
        }

        _init() {
            this._element.addEventListener('click', (e) => {
                if (e.target.hasAttribute('data-bs-dismiss')) {
                    this.hide();
                }
            });
            if (this._config.keyboard) {
                document.addEventListener('keydown', (e) => {
                    if (this._isShown && e.key === 'Escape') {
                        this.hide();
                    }
                });
            }
        }

        show() {
            if (this._isShown) return;
            this._isShown = true;
            this._showBackdrop(() => {
                this._element.style.display = 'block';
                this._element.classList.add('show');
                if (this._config.focus) this._element.focus();
                document.body.classList.add('modal-open');
            });
        }

        hide() {
            if (!this._isShown) return;
            this._isShown = false;
            this._element.classList.remove('show');
            this._element.style.display = 'none';
            document.body.classList.remove('modal-open');
            this._removeBackdrop();
        }

        _showBackdrop(callback) {
            if (!this._config.backdrop) {
                callback();
                return;
            }
            this._backdrop = document.createElement('div');
            this._backdrop.className = 'modal-backdrop fade show';
            document.body.appendChild(this._backdrop);
            this._backdrop.addEventListener('click', () => {
                if (!this._ignoreBackdropClick) this.hide();
            });
            callback();
        }

        _removeBackdrop() {
            if (this._backdrop) {
                document.body.removeChild(this._backdrop);
                this._backdrop = null;
            }
        }
    }

    // Expose globally
    window.bootstrap = window.bootstrap || {};
    window.bootstrap.Modal = Modal;
})();
