
function fixMethodClick(rad) {
    var idBill = $('#id_Bill');
    var contentBill = $('#contentBill')
    $('#vlFix').text('');
    if (rad.value == 0) {
        idBill.val('')
        contentBill.val('')

        $('#id_Bill').prop('readonly', true);
        $('#contentBill').prop('readonly', true);
        
    }
    if (rad.value == 1) {
        $('#id_Bill').prop('readonly', false);
        $('#contentBill').prop('readonly', false);
    }
}

function validateForm() {
    var fix = $('#vlFix')
    var rad0 = $('#rad0')
    var rad1 = $('#rad1')
    $('#ErrIdBill').text('')
    $('#ErrContent').text('')
    if (rad0.prop('checked') == false && rad1.prop('checked') == false) {
        fix.text('Require');
        return false;
    }

    if (rad1.prop('checked') == true) {
        if ($('#id_Bill').prop('value') == '') {
            $('#ErrIdBill').text('Require')
            return false
        }
        if ($('#contentBill').prop('value') == '') {
            $('#ErrContent').text('Require')
            return false
        }
    }
   
}

$(document).ready(() => {
    var btnFinish = $('#btnFinish')
    if (btnFinish.prop('disabled') == true) {
        $('#rad0').prop('disabled', true)
        $('#rad1').prop('disabled', true)
        $('#describeFAQ').prop('disabled', true)
        $('#finishTime').prop('disabled', true)
    }
})