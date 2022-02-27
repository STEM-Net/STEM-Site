function loadData(element, id) {
    $.ajax({
        url: "../Home/GetMoisture",
        type: "POST",
        beforeSend: function () { // Before we send the request, remove the .hidden class from the spinner and default to inline-block.
            element.innerHTML = "<div style=\"margin:2.5rem;\"><div class=\"lds-ring\"><div></div><div></div><div></div><div></div></div></div>";
        },
        data: {
            deviceId: id
        },
        success: function (data) {
            element.innerHTML = data;
            renderChart();

            $('#a-historical-data').click(function () {
                $('historical-data-modal').modal('show');
                loadHistoricalData($('#historical-data-modal-body'), 0); // TODO change this
            });

            //$("#historical-data-modal-x").click(function () {
            //    $('#historical-data-modal-content').remove();
            //    $('#historical-data-modal').removeClass("in");
            //    $(".modal-backdrop").remove();
            //    $('#historical-data-modal').modal('hide');
            //});
        },
        error: function () {

        },
        complete: function () { // Set our complete callback, adding the .hidden class and hiding the spinner.
            //$('#loader').addClass('hidden')
        }
    });
}

    //$("#apply-filters").click(function () {
    //    beginIndex = 0;
    //    endIndex = 24;
    //    loadedEverything = false;
    //    $("#influencersGrid").empty();
    //    loadData();
    //});