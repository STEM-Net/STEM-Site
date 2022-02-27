var $range = $(".js-range-slider");
var $inputFrom = $("#low-bound");
var $inputTo = $("#up-bound");
var instance;
var min = 0;
var max = 15400;
var from = 0;
var to = 0;

// Ion range slider configuration

$range.ionRangeSlider({
    type: "double",
    grid: true,
    min: 0,
    max: 100,
    from: 0,
    to: 15400,
    skin: "flat",
    onStart: updateInputs,
    onChange: updateInputs,
    onFinish: updateInputs
});

instance = $range.data("ionRangeSlider");

function updateInputs(data) {
    from = data.from;
    to = data.to;

    $inputFrom.prop("value", from);
    $inputTo.prop("value", to);
}

$inputFrom.on("change", function () {
    var val = $(this).prop("value");

    // validate
    if (val < min) {
        val = min;
    } else if (val > to) {
        val = to;
    }

    instance.update({
        from: val
    });

    $(this).prop("value", val);

});

$inputTo.on("change", function () {
    var val = $(this).prop("value");

    // validate
    if (val < from) {
        val = from;
    } else if (val > max) {
        val = max;
    }

    instance.update({
        to: val
    });

    $(this).prop("value", val);
});

// Switch between tabular and map view
$("#table-map-toggle").change(function () {
    console.log("toggled");
    if (this.checked) {
        $("#table-content").addClass("d-none");
        $("#map-content").removeClass("d-none");
    } else {
        $("#table-content").removeClass("d-none");
        $("#map-content").addClass("d-none");
    }
});