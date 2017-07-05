var legendOpened = false;

function AnimateLegend() {

    $("#legend").toggleClass("lightback");

    $("#legend .header").toggleClass("lightback");
    $("#legend .header").toggleClass("darkback");

    $("#legend .body").toggle();
    
    legendOpened = !legendOpened;
}