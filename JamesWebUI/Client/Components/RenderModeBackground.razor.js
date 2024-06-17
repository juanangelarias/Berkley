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
}
export function setBackgroundClient(el){
    setBackground(el, "aliceblue");
}
export function setBackgroundServer(el){
    setBackground(el, "mistyrose");
}

