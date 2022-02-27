Chart.defaults.global.legend.display = false;
Chart.defaults.global.tooltips.enabled = false;

function renderLoader() {

}

function renderChart() {
    var moisture = document.getElementById("moisture-heading").innerHTML.slice(0, -1);

    var ctx = document.getElementById("moisture-chart");
    var myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Moisture", "Empty"],
            datasets: [{
                label: '# of Votes',
                data: [moisture, 100-moisture],
                backgroundColor: [
                    'rgba(0, 112, 191)',
                    'rgba(211, 211, 211)'
                ]
            }]
        },
        options: {
            rotation: 1 * Math.PI,
            circumference: 1 * Math.PI
        }
    });
}

function loadHistoricalData(element, id) {
    $.ajax({
        url: "../Home/GetHistoricalData",
        type: "POST",
        beforeSend: function () { // Before we send the request, remove the .hidden class from the spinner and default to inline-block.
            element.html("<div style=\"margin:2.5rem;\"><div class=\"lds-ring\"><div></div><div></div><div></div><div></div></div></div>");
        },
        data: {
            deviceId: id
        },
        success: function (data) {
            element.html(data);
        },
        error: function () {

        },
        complete: function () { // Set our complete callback, adding the .hidden class and hiding the spinner.
            //$('#loader').addClass('hidden')
        }
    });
}