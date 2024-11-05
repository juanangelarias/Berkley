function getParentDiv(el){
    while (el && el.parentNode) {
        el = el.parentNode;
        if (el.tagName && el.tagName.toLowerCase() == 'div') {
            return el;
        }
    }
    return null;
}
function setBackground(el, color){
    var pdiv = getParentDiv(el);
    if (pdiv)
        pdiv.style.backgroundColor = color;
    return pdiv;
}

export function setBackgroundClient(el){
    var pdiv = setBackground(el, "cornsilk");
    pdiv.classList.add("RunningClientSide");
}
export function setBackgroundServer(el){
    var pdiv = setBackground(el, "#E0E0E ");//Light silver
    pdiv.classList.add("RunningServerSide");
}

