
$(document).ready(() => {
    getCount()
})

var myHub = $.connection.TroubleHub;

$.connection.hub.start()
    .done(() => {
        console.log('connected');
        myHub.server.hello();

    })

myHub.client.show = () => {
    consoe.log('hell');
}

myHub.client.displayTrouble = () => {
    //getData();
    getNewTrobule();
}

myHub.client.sendAll = () => {
    console.log('all');
}

function getData() {
    console.log('Get data');

    var status = ['Sent', 'Received', 'Prosessing', 'Finished'];

    var $tbl = $('#tableTrouble');
    var $notify = $('#notify');
    $.ajax({
        url: $("#Get").val(),
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            if (data.length == 0) {
                $notify.empty();
                $notify.append('No New Trouble')
                $('#headTable').hide()
            }
            else {
                $notify.empty();
                $tbl.empty();
                $('#headTable').show()
                $('#newTrouble').text(data.length)
                $.each(data, function (i, model) {
                    $tbl.append
                        (
                            '<tr class="bg-info">' +
                            '<td>' + model.id_Trouble + '</td>' +
                            '<td>' + model.id_Report + '</td>' +
                            '<td>' + model.id_Fill + '</td>' +
                            '<td>' + setID(model.id_Manage) + '</td>' +
                            '<td>' + model.id_Device + '</td>' +
                            //'<td>< i class= "fas fa-envelope" ></i ><span style="padding-left: 5px">New report </span></td >'+
                            '<td>' + status[model.status] + '</td>' +
                            '<td>' + setLevel(model.level) + '</td>' +
                            '<td>' + formatDate(model.dateStaff) + '</td>' +
                            '<td>' + formatDate(model.dateManage) + '</td>' +
                            '<td>' + formatDate(model.dateTech) + '</td>' +
                            '<td>' + '<a href = "/Manager/Trouble/Share/' + model.id_Trouble + '">Details</a ></td >' +
                            '</tr>'

                        );
                });
            }

        },
        error: (err) => {
            console.log(err)
        }
    });
}

function formatDate(strDate) {
    if (strDate != null) {
        var msec = strDate.slice(strDate.indexOf('(') + 1, strDate.lastIndexOf(')'));
        var date = new Date(parseInt(msec));
        //var d = date.toLocaleString();
        var dd = date.getDate();
        var mm = date.getMonth() + 1;
        var yy = date.getFullYear();
        var d = dd + '/' + mm + '/' + yy
    }
    else {
        d = ' ';
    }
    return d;
}

function setID(id) {
    if (id == -1) return 'none';
    return id;

}

function setLevel(id) {
    var level = ['Less Serious', 'Serious', 'Very Serious'];
    if (id == -1) return 'No  Define';
    return level[id];
}

function getNewTrobule() {
    var status = ['Sent', 'Received', 'Prosessing', 'Finished'];
    const table = $('#tableTrouble tr:first')
    $.ajax({
        url: location.origin + '/Manager/Trouble/GetNewTrouble',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            console.log(data)
            $('#newTrouble').text(data.count)
            table.after
                (
                    `<tr onclick="window.location.href = '/Manager/Trouble/Share/${data.trouble.id_Trouble}'" class='bg-info'>`+
                    '<td>' + data.trouble.id_Trouble + '</td>' +
                    '<td>' + data.trouble.id_Report + '</td>' +
                    '<td>' + data.trouble.id_Fill + '</td>' +
                    '<td></td>' +
                    '<td>' + data.trouble.id_Device + '</td>' +
                    '<td><i class= "fas fa-envelope"></i><span style="padding-left: 5px">New report</span></td>' +
                    //'<td>' + status[data.trouble.status] + '</td>' +
                    '<td>' + setLevel(data.trouble.level) + '</td>' +
                    '<td>' + formatDate(data.trouble.dateStaff) + '</td>' +
                    '<td>' + formatDate(data.trouble.dateManage) + '</td>' +
                    '<td>' + formatDate(data.trouble.dateTech) + '</td>' +
                    '</tr>'

                );
        },
        error: (err) => {
            console.log(err)
        }
    })   

}

function getCount() {
    
    $.ajax({
        url: location.origin + '/Manager/Trouble/GetNewTrouble',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            console.log(data)
            $('#newTrouble').text(data.count)
           
        },
        error: (err) => {
            console.log(err)
        }
    })

}

