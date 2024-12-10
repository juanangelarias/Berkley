//This file is used to prevent files from being dropped onto the browser except onto the RadzenUpload control

function ParentWithClass(element, classname) {
    if (element.classList && element.classList.contains(classname)) return true;
    return element.parentNode && ParentWithClass(element.parentNode, classname);
}
function ParentWithClasses(element, classnames) {
    for (let classname of classnames) {
        if (element.classList && element.classList.contains(classname)) return true;
    }
    return element.parentNode && ParentWithClasses(element.parentNode, classnames);
}

let dropzoneClass = 'rz-fileupload';
let dropzoneClasses = ['rz-fileupload', 'rz-group-header-drop'];

const dragEventHandler = e => {
        var isInsideDropzone = ParentWithClasses(e.target, dropzoneClasses);
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
