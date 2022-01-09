function bind_device() {
    var id = $('#id_Report').val()
    var id_device = $('#id_Device')
    $('#idReport_error').empty()
    if (id > 0) {
        $.ajax({
            url: location.origin + '/Device/GetDevice/' + id,
            type: 'GET',
            datatype: 'json',
            success: function (data) {
                if (data) {
                    if (data.length > 0) {
                        id_device.empty()
                        $.each(data, function (i, item) {
                            id_device.append($('<option>', {
                                value: item.id_Device,
                                text: item.nameDevice
                            }));
                        });
                    }
                    else {
                        id_device.empty()
                        id_device.append($('<option>', {
                            value: 0,
                            text: 'Not Device Found'
                        }));
                    }
                }
                else {
                    $('#idReport_error').text('ID Not Found')
                }
            },
            error: (err) => {
                console.log(err)
            }
        })
    }
    else {
        $('#idReport_error').text('ID Not Found')
    }
}

$(document).ready(() => {
    addEvent()
})

function addEvent() {
    $('#id_Report').keyup(() => {
        delay(() => {
            bind_device()
        }, 1000)
    })
}

var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();
//$('input').keyup(function () {
//    delay(function () {
//        alert('Hi, func called');
//    }, 1000);
//});