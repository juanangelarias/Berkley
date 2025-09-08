window.blazor_setExitEvent = (dotNetHelper) => {
    window.onbeforeunload = (event) => {
        dotNetHelper.InvokeMethodAsync('OnPageExit');
    }
}
