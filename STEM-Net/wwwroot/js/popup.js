Chart.defaults.global.legend.display = false;
Chart.defaults.global.tooltips.enabled = false;

console.log("please run");

function renderChart() {
    var ctx = document.getElementById("myChart");
    var myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Red", "Blue"],
            datasets: [{
                label: '# of Votes',
                data: [30, 20],
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