function AjustEditTextSize(reference) {
    if ($(".auxi")[0]) {
        var main = $(".main").width();
        var auxis = 2 * ScaleWidth(".auxi");
        var adders = 3 * ScaleWidth("adder");
        var scroll = 25;

        var text = main - auxis - adders - scroll;

        if (!reference) reference = ".paragraph .text";

        $(reference).width(text);
    }
}