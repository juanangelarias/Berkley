//This file is used to prevent files from being dropped onto the browser except onto the RadzenUpload control

function ParentWithClass(element, classname) {
    if (element.classList && element.classList.contains(classname)) return true;
    return element.parentNode && ParentWithClass(element.parentNode, classname);
}

let dropzoneClass = 'rz-fileupload'

const dragEventHandler = e => {
    var isInsideDropzone = ParentWithClass(e.target, dropzoneClass)
    if (isInsideDropzone == undefined || isInsideDropzone === false) {
        e.preventDefault();
        e.dataTransfer.effectAllowed = 'none';
        e.dataTransfer.dropEffect = 'none';
    }
}

if (!(globalThis.dragDrop)) {
    ['dragenter', 'dragover', 'drop'].forEach(ev => window.addEventListener(ev, dragEventHandler, false));
    //Prevent handlers from being added twice
    globalThis.dragDrop = true;
}
