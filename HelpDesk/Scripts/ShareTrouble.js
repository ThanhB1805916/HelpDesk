var arrLang = {
    'en': {
        newTask: 'New Task',
        techID: 'Technician ID',
        desManager: 'Description of Manager',
        deadline: 'Dealine',
        delete: 'Delete'

    },
    'vi': {
        newTask: 'Task Mới',
        techID: 'ID Kỹ Thuật Viên',
        desManager: 'Mô Tả Của Quản Lý',
        deadline: 'Hạn Chót',
        delete: 'Xóa'
    }
}

function AddTech(lang) {
    console.log(n);
    var i = n;
    
    $('#txtNoTech').hide();
    $('#divTech').append(
        '<div id="newTech-'+i+'">'+
        '<hr />' +
        '<fieldset style="text-decoration: none; background-color: #48dbfb; margin: 10px">' +
        '<legend style="font-size: 18px; background-color: #54a0ff; padding-left:10px; font-weight: bold" key="newTask" class="lang">' + arrLang[lang]["newTask"] + '</legend>' +
        '<div class="form-group">' +
        '<label class= "control-label col-md-2 lang" for= "id_Tech" key="techID">' + arrLang[lang]["techID"] + '</label >' +
           ' <div class="col-md-10">'+
                   '<select id="id_Tech_' + i + '_" name="id_Tech[' + i + ']">'+
                        
                    '</select >'+
            '</div>'+
         '</div >'+
        
        '<div class="form-group">'+
        '<label class="control-label col-md-2 lang" for="describeTech" key="desManager">' + arrLang[lang]["desManager"] + '</label>'+
            '<div class="col-md-10">'+
        '<textarea cols="50" htmlAttributes="{ class = form-control }" id="describeTech_' + i + '_" name="describeTech[' + i +']" rows="7"></textarea>'+
                '<span class="field-validation-valid text-danger" data-valmsg-for="describeTech" data-valmsg-replace="true"></span>'+
            '</div>'+
        '</div>' +

        '<div class="form-group">'+
        '<label class= "control-label col-md-2 lang" for= "deadline" key="deadline" >' + arrLang[lang]["deadline"] + '</label>'+
            '<div class="col-md-10">'+
                '<input data-val="true" data-val-date="The field Nullable`1 must be a date." htmlAttributes="{ class = form-control }" id="deadline_'+i+'_" name="deadline['+i+']" class="dealine" type="date" value="" />'+
                '<span class="field-validation-valid text-danger" data-valmsg-for="deadline" data-valmsg-replace="true"></span>'+
            '</div>'+
        '</div>'+

        '<div class="form-group">'+
        '<label class= "control-label col-md-2" for= "id_FAQ">ID FAQ</label >'+
            '<div class="col-md-10">'+
                //'<input data-val="true" data-val-number="The field Int32 must be a number." data-val-required="The Int32 field is required." htmlAttributes="{ class = form-control }" id="id_FAQ_'+i+'_" name="id_FAQ['+i+']" type="number"/>'+
                //'<span class="field-validation-valid text-danger" data-valmsg-for="id_FAQ" data-valmsg-replace="true"></span>'+
                '<select id="id_FAQ_' + i + '_" name="id_FAQ[' + i + ']">' +
                    
                '</select >' +
            '</div>' +
        '</div >'+
        
        '<div class="form-group">'+
            '<div class="col-md-offset-2 col-md-10">'+
        '<input type="button" value="' + arrLang[lang]["delete"] + '" id="'+i+'" onclick="RemoveTech(this)" class="btn btn-danger lang" key="delete" />'+
            '</div>'+
        '</div>' +
        '</fieldset >'+
        '</div>'
    );
    n++;
    getListTech(i);
    getListFAQ(i);
    //count++;
    ////countTech++;
    //console.log(count);
    $('.lang').each((i, element) => {
        $(this).text(arrLang[lang][$(this).attr('key')])
    })
}

function RemoveTech(i) {
    $('#newTech-' + i.id).remove();
    //countTech--;
    //count--;
    n--;
}

function validationForm() {
    var dateManage = $('#dateManage')
    var errorDate = $('#errorDate')
    var dealine = $('.dealine')
    errorDate.text('');
    if (dateManage.val() == '') {
        errorDate.text('Require')
        window.scrollTo(0, 400)
        console.log(errorDate)
        return false;
    }
    //else {
    //    for (i = 0; i < dealine.length; i++) {
    //        console.log(dealine[i].val())
    //    }
        
    //}
    //return false;
}

function DeleteTech(i) {
    console.log('delelte' + i);
    $('#div_' + i).remove();
    n--;
}

function getListTech(index) {
    var selected = $('#id_Tech_' + index + '_')
    $.ajax({
        url: location.origin + '/Manager/Trouble/GetListTech',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            $.each(data, (i, user) => {
                selected.append(`<option value="${user.id_user}">${user.username}</option>`)
            })
        },
        error: (err) => {
            console.log(err)
        }
    })
}

function getListFAQ(index) {
    var selected = $('#id_FAQ_' + index + '_')
    $.ajax({
        url: location.origin + '/Manager/Trouble/GetListFAQ',
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            selected.append('<option value="0">none</option>')
            $.each(data, (i, faq) => {
                selected.append(`<option value="${faq.id_FAQ}">${faq.question}</option>`)
            })
        },
        error: (err) => {
            console.log(err)
        }
    })
}