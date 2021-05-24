Chart.defaults.global.legend.display = false;
Chart.defaults.global.tooltips.enabled = false;

function renderChart() {
    var moisture = document.getElementById("moisture-heading").innerHTML.slice(0, -1);

    var ctx = document.getElementById("myChart");
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