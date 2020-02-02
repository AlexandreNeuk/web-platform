$(document).ready(function () {



    //// Set new default font family and font color to mimic Bootstrap's default styling
    //Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
    //Chart.defaults.global.defaultFontColor = '#858796';

    //// Area Chart Example
    //var ctx = document.getElementById("pressaochart");
    //m.pressao.grafico = new Chart(ctx, {
    //    type: 'line',
    //    data: {
    //        //labels: ["Janeiro", "Fevereiro", "Marco", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],   // aunal
    //        //labels: ["Sexta", "Sabado", "Domingo", "Segunda-Feira", "Terca-Feira", "Quarta-Feira", "Quinta-Feira"],                                  // semanal
    //        //labels: ["19:00", "21:00", "22:00", "00:00", "02:00", "05:00", "07:00", "09:00", "11:00", "13:00", "15:00", "17:00"],                    // diário
    //        labels: ["24/05", "27/05", "30/05", "02/06", "05/06", "08/06", "11/06", "14/06", "17/06", "20/06", "23/06", "26/06"],               
    //        datasets: []
    //    },
    //    options: {
    //        maintainAspectRatio: false,
    //        layout: {
    //            padding: {
    //                left: 10,
    //                right: 25,
    //                top: 25,
    //                bottom: 0
    //            }
    //        },
    //        scales: {
    //            xAxes: [{
    //                time: {
    //                    unit: 'date'
    //                },
    //                gridLines: {
    //                    display: false,
    //                    drawBorder: false
    //                },
    //                ticks: {
    //                    maxTicksLimit: 7
    //                }
    //            }],
    //            yAxes: [{
    //                ticks: {
    //                    maxTicksLimit: 5,
    //                    padding: 10,
    //                    // Include a dollar sign in the ticks
    //                    callback: function (value, index, values) {
    //                        return 'KW/h ' + m.g.number_format(value);
    //                    }
    //                },
    //                gridLines: {
    //                    color: "rgb(234, 236, 244)",
    //                    zeroLineColor: "rgb(234, 236, 244)",
    //                    drawBorder: false,
    //                    borderDash: [2],
    //                    zeroLineBorderDash: [2]
    //                }
    //            }],
    //        },
    //        legend: {
    //            display: true
    //        },
    //        tooltips: {
    //            backgroundColor: "rgb(255,255,255)",
    //            bodyFontColor: "#858796",
    //            titleMarginBottom: 10,
    //            titleFontColor: '#6e707e',
    //            titleFontSize: 14,
    //            borderColor: '#dddfeb',
    //            borderWidth: 1,
    //            xPadding: 15,
    //            yPadding: 15,
    //            displayColors: false,
    //            intersect: false,
    //            mode: 'index',
    //            caretPadding: 10,
    //            callbacks: {
    //                label: function (tooltipItem, chart) {
    //                    var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
    //                    return datasetLabel + ': ' + m.g.number_format(tooltipItem.yLabel) + ' KW/h';
    //                }
    //            }
    //        }
    //    }
    //});

    //m.pressao.grafico.data.datasets.push({
    //    label: "Extrusora",
    //    lineTension: 0.3,
    //    backgroundColor: "rgba(78, 115, 223, 0.05)",
    //    borderColor: "rgba(78, 115, 223, 1)",
    //    pointRadius: 3,
    //    pointBackgroundColor: "rgba(78, 115, 223, 1)",
    //    pointBorderColor: "rgba(78, 115, 223, 1)",
    //    pointHoverRadius: 3,
    //    pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
    //    pointHoverBorderColor: "rgba(78, 115, 223, 1)",
    //    pointHitRadius: 10,
    //    pointBorderWidth: 2,
    //    //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100, 2600, 1500, 1100, 1200, 2000] // diario
    //    //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100]                               // semanal
    //    //data: [1400, 1450, 2000, 2200, 2800, 2200, 2100, 2600, 1500, 1100, 1200, 2000] // anual
    //    data: [1400, 1450, 2000, 2200, 2800, 2200, 2100, 2600, 1500, 1100, 1200, 2000]   // mensal
    //});
    //m.pressao.grafico.update();
});