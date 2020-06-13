
"use strict";


window.onload = function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/sensor").withAutomaticReconnect().build();
    var dps = []; // dataPoints
    var dataLength = 20;
    var x = 1;
    var chart = new CanvasJS.Chart("chartContainer", {
        title: {
            text: "Dynamic Data"
        },
        axisY: {
            includeZero: false
        },
        data: [{
            type: "spline",
            dataPoints: dps
        }]
    });

    /*******CPU Graf *******/
    var dps2 = []; // dataPoints
    var dataLength2 = 60;
    var x2 = 1;
    var chart2 = new CanvasJS.Chart("chartContainerCPU", {
        title: {
            text: "CPU"
        },
        axisY: {
            includeZero: false
        },
        data: [{
            type: "line",
            dataPoints: dps2
        }]
    });

    document.getElementById("conBtn").addEventListener("click", function (event) {
        connection.invoke("StartCouReading").catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });

    connection.on("ClientConnected", function () {
        document.getElementById("conBtn").disabled = false;
    });
    connection.on("SenzorChange", function (data) {
        dps.push({
            label: data.x,
            x: x++,
            y: parseFloat(data.y)
        });
        if (dps.length > dataLength) {
            dps.shift();
        }
        chart.render();
        console.log(data);
    });
    connection.on("Alert", function (data) {
        dps.push({
            label: data.x,
            x: x++,
            color: "Red",
            labelFontColor: "Red",
            lineColor:"Red",
            y: parseFloat(data.y)
        });
        if (dps.length > dataLength) {
            dps.shift();
        }
        chart.render();
        console.log(data);
        
    });
    connection.on("Cpu", function (data) {
        dps2.push({
            label: data.x,
            x: x2++,
            color: "Red",
            lineColor: "Red",
            labelFontColor: "Red",
            y: parseFloat(data.y)
        });
        if (dps2.length > dataLength2) {
            dps2.shift();
        }
        chart2.render();

    });
    connection.onreconnected(connectionId => {
        this.Console.log("Reconected");
    });

    connection.onreconnecting(error => {

        this.Console.log(error);

    });
    connection.start().then(function () {
        console.log("Connected");
    }).catch(function (err) {
        return console.error(err.toString());
    });
};


