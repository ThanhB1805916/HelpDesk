var myHub = $.connection.TroubleHub;

$.connection.hub.start()
    .done(() => {
        console.log('connected');
        myHub.server.hello();
        //myHub.server.send();
        //myHub.server.notifyFinish(12)
    })

myHub.client.show = () => {
    console.log('hell');
}


myHub.client.displayFinish = (idReport, idTrouble) => {
    
    SentNoifyFinish(idReport, idTrouble)
}

function SentNoifyFinish(idReport, idTrouble) {
    var idUser = $('#idUser')
    //var notify = $('#notifi')
    if (idUser.val() == idReport) {
        //notify.append(
        //    '<li class="alert-info"><a href="DetailHis/' + idTrouble + '">Trouble with id ' + idTrouble + ' was finish. Click here to see more>></a></li>'
        //)
        getNewTrobule(idTrouble)
    }
}

function getNewTrobule(idTrouble) {
    
    const table = $('#tableTrouble tr:first')
    $.ajax({
        url: location.origin + '/Trouble/GetTrouble/' + idTrouble,
        type: 'GET',
        datatype: 'json',
        success: function(data) {
            //console.log(data)
            //    //$('#newTrouble').text(data.count)
            //$('#notifi').append(
            //    '<li class="alert-info"><a href="DetailHis/' + idTrouble + '">Trouble with id ' + idTrouble + ' was finish. Click here to see more>></a></li>'
            //)
            table.after(
                `<tr onclick="detailTrouble(${idTrouble},${data.status})" class="bg-info">` +
                //'<tr onclick="detailTrouble(' + idTrouble + ')" class="bg-info">' +
                '<td>' + data.id_Trouble + '</td>' +
                '<td>' + data.describe + '</td>' +
                '<td>' + formatDate(data.dateStaff) + '</td>' +
                '<td><i class="fas fa-check"></i><span style= "padding-left: 5px">Finished</span></td>' +
                '</tr>'

            );
            //$('#row_' + idTrouble).remove();

        },
        error: (err) => {
            console.log(err)
        }
    })

}

function deleteTrouble(idTrouble) {
    var row = $('#row_' + idTrouble)
    var notify = $('#fail')
    if (confirmDelete(idTrouble)) {
        $.ajax({
            url: '/Trouble/DeleteP/' + idTrouble,
            type: 'POST',
            dataType: 'json',
            contentType: "application/json; charset=UTF-8",
            success: function(data) {
                if (data) {
                    row.remove()
                    notify.text('')
                    notify.text('Delete successfully')
                } else {
                    notify.text('')
                    notify.text('Delete fail')
                }
            },
            error: (err) => {
                console.log(err)
            }

        })
    }

}

function confirmDelete(idTrouble) {
    return confirm('Do you want delete trouble report, ID: ' + idTrouble)
}

function detailTrouble(idTrouble, status) {
    if (status != 3) {
        window.location.href = 'Details/' + idTrouble
    }
    else {
        finishViewed(idTrouble)
        
    }
    
}

function setLevel(id) {
    var level = ['Less Serious', 'Serious', 'Very Serious'];
    if (id == -1) return 'No  Define';
    return level[id];
}

function formatDate(strDate) {
    if (strDate != null) {
        var msec = strDate.slice(strDate.indexOf('(') + 1, strDate.lastIndexOf(')'));
        var date = new Date(parseInt(msec));
        //var d = date.toLocaleString();
        var dd = date.getDate();
        var mm = date.getMonth() + 1;
        var yy = date.getFullYear();
        var d = dd + '-' + mm + '-' + yy
    } else {
        d = ' ';
    }
    return d;
}

function historyDevice(idDevice) {
    window.location.href = 'History/' + idDevice
}

function finishViewed(idTrouble) {
    $.ajax({
        url: '/Trouble/FinishViewed/' + idTrouble,
        type: 'POST',
        dataType: 'json',
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            if (data) {
                window.location.href = 'Details/' + idTrouble
            } else {
                $('#fail').text('Fail');
            }
        },
        error: (err) => {
            console.log(err)
        }

    })
}