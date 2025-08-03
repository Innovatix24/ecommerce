window.setImageSrc = (id, src) => {
    const img = document.getElementById(id);
    if (img) {
        img.src = src;
        if (!img.cropper) {
            new Cropper(img, {
                aspectRatio: 1,
                viewMode: 1,
                autoCropArea: 0.8
            });
        }
    }
};

window.cropImage = async (id) => {
    const img = document.getElementById(id);
    if (img && img.cropper) {
        const canvas = img.cropper.getCroppedCanvas();
        return canvas.toDataURL("image/png");
    }
    return null;
};
