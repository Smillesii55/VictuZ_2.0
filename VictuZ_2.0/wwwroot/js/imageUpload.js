document.addEventListener('DOMContentLoaded', function () {
    // Selecteer alle dropzones op de pagina
    const dropzones = document.querySelectorAll('.image-dropzone');

    if (dropzones.length === 0) {
        console.warn('Geen elementen met de klasse .image-dropzone gevonden.');
    }

    dropzones.forEach(function (dropzone, index) {
        const fileInput = dropzone.querySelector('input[type="file"]');
        const imagePreview = dropzone.querySelector('.image-preview');

        // Controleer of de elementen bestaan
        if (!fileInput || !imagePreview) {
            console.warn(`File input of image preview niet gevonden voor dropzone index ${index}.`);
            return;
        }

        // Prevent default drag behaviors
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            dropzone.addEventListener(eventName, preventDefaults, false);
            document.body.addEventListener(eventName, preventDefaults, false);
        });

        // Highlight dropzone when item is dragged over it
        ['dragenter', 'dragover'].forEach(eventName => {
            dropzone.addEventListener(eventName, highlight, false);
        });

        ['dragleave', 'drop'].forEach(eventName => {
            dropzone.addEventListener(eventName, unhighlight, false);
        });

        // Handle dropped files
        dropzone.addEventListener('drop', handleDrop, false);

        // Handle file selection
        fileInput.addEventListener('change', handleFiles, false);

        function preventDefaults(e) {
            e.preventDefault();
            e.stopPropagation();
        }

        function highlight(e) {
            dropzone.classList.add('bg-light', 'border-2');
        }

        function unhighlight(e) {
            dropzone.classList.remove('bg-light', 'border-2');
        }

        function handleDrop(e) {
            const dt = e.dataTransfer;
            const files = dt.files;

            if (files.length > 0) {
                // Stel de nieuwe bestanden in
                fileInput.files = files;

                // Reset de preview voordat je de nieuwe afbeelding toont
                resetPreview();

                // Toon de nieuwe preview
                displayPreview(files[0]);
            }
        }

        function handleFiles() {
            if (fileInput.files.length > 0) {
                // Reset de preview voordat je de nieuwe afbeelding toont
                resetPreview();

                displayPreview(fileInput.files[0]);
            } else {
                resetPreview();
            }
        }

        function displayPreview(file) {
            const reader = new FileReader();
            reader.onload = function (event) {
                imagePreview.innerHTML = `
                    <img src="${event.target.result}" alt="Image Preview" class="img-fluid mb-2 preview-image" />
                    <p>Drag & Drop a new image here or use the file input above</p>
                `;
            }
            reader.readAsDataURL(file);
        }

        function resetPreview() {
            const hasImage = dropzone.dataset.hasImage === 'true';
            const imageUrl = dropzone.dataset.imageUrl || '';

            if (hasImage && imageUrl) {
                imagePreview.innerHTML = `
                    <img src="${imageUrl}" alt="Current Image" class="img-fluid mb-2 preview-image" />
                    <p>Drag & Drop a new image here or use the file input above</p>
                `;
            } else {
                imagePreview.innerHTML = `<p>Drag & Drop an image here or use the file input above</p>`;
            }
        }

    });
});
