function FontResizer() {
    $(".sizeLess").click(function () {
        editSize(-1);
    });

    $(".sizeMore").click(function () {
        editSize(+1);
    });
}

function editSize(edit) {
    var size = $(".main").css('font-size');

    size = size
        .replace('px', '')
        * 1
        + edit;

    $(".main").css('font-size', size);
}