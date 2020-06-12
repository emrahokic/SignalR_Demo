
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
            y: parseFloat(data.y)
        });
        if (dps.length > dataLength) {
            dps.shift();
        }
        chart.render();
        console.log(data);
        
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


//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});