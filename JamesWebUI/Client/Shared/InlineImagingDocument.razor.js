//This file is used to prevent files from being dropped onto the browser except onto the RadzenUpload control

//var alreadyChecked = new Object();//Cache of results so that the tree isn't repeatedly climbed for the same elements
//function getUniqueKeyForNode(targetNode) {
//    const pieces = ['doc'];
//    let node = targetNode;

//    while (node && node.parentNode) {
//        pieces.push(Array.prototype.indexOf.call(node.parentNode.childNodes, node));
//        node = node.parentNode
//    }

//    return pieces.reverse().join('/');
//}
//function ParentWithClassCached(element, classname) {
//    //NOTE: Not validating classname for performance reasons
//    if (alreadyChecked[classname] == undefined) {
//        alreadyChecked[classname] = new Object();
//        console.log("Cache for class " + classname + " created");
//    }
//    var elementKey = getUniqueKeyForNode;
//    if (alreadyChecked[classname][elementKey] == undefined) {
//        if (ParentWithClass(element, classname))
//            alreadyChecked[classname][elementKey] = true;
//        else
//            //Interpret false or undefined as false
//            alreadyChecked[classname][elementKey] = false;
//    }
//    //TODO:Remove else after testing
//    else console.log("Cache hit!");
//    return alreadyChecked[classname][elementKey];
//}
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
    globalThis.dragDrop = true;
}
