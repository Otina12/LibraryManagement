function configureTinyMce() {
    tinymce.init({
        selector: '#tinyMceEditor',
        plugins: [
            'advlist', 'autolink', 'link', 'image', 'lists', 'charmap', 'preview', 'anchor', 'pagebreak',
            'searchreplace', 'wordcount', 'visualblocks', 'code', 'fullscreen', 'insertdatetime', 'media',
            'table', 'emoticons', 'help'
        ],
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | ' +
            'bullist numlist outdent indent | link image | print preview media fullscreen | ' +
            'forecolor backcolor emoticons | help',
        resize: true,
    });
}

function sortTableByStartDate(ascending, isOneParameter, cellIndex) {
    const table = document.querySelector('table tbody');
    const rows = Array.from(table.rows);

    rows.sort((a, b) => {
        const cellA = a.cells[cellIndex];
        const cellB = b.cells[cellIndex];

        if (!cellA || !cellB) {
            return 0;
        }

        const dateA = isOneParameter ? new Date(cellA.innerText) : new Date(cellA.innerText.split(' - ')[0]);
        const dateB = isOneParameter ? new Date(cellB.innerText) : new Date(cellB.innerText.split(' - ')[0]);

        return ascending ? dateA - dateB : dateB - dateA;
    });

    rows.forEach(row => table.appendChild(row));
}