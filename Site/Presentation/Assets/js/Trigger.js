$(document).ready(function() {

    $(".trigger-toggle").click(function () {

        var toggleType = $(this).data("type") == "+";
        var content = $(this).data("content").toUpperCase();
        
        if (toggleType) {
            toggleType = confirm("Aviso de conteúdo: pode disparar lembranças sobre " + content + ".");
        }

        $(".trigger-hidden").toggle(toggleType);

    });

});