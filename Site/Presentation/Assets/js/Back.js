function ChooseBackground() {
    var max = 7;

    var rnd = Math.random();
    var bgNum = Math.floor(rnd * (max + 1));

    if (bgNum > max) bgNum = max;
    if (bgNum < 1) bgNum = 1;

    $("body").addClass("bg" + bgNum);
}