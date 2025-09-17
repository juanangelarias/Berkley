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

export function setBackgroundClient(el) {
    if (el) { //This can be null if run too early in the page life cycle.
        var pdiv = setBackground(el, "cornsilk");
        if (pdiv)
            pdiv.classList.add("RunningClientSide");
        else
            el.classList.add("RunningClientSide");
    } else { debugger; }
}

export function setBackgroundServer(el){
    var pdiv = setBackground(el, "#E0E0E ");//Light silver
    pdiv.classList.add("RunningServerSide");
}

