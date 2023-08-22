function validateObj(object){
    var name = object.getAttribute('name');
    var obj_error = document.getElementById('error_'+name);
    var val = object.value;
    var field_name = object.getAttribute('field-name');
    if(val == ''){
        obj_error.innerHTML = "Vui lòng nhập " + field_name;
        obj_error.style.display = 'block';
    }else{
        obj_error.innerHTML = "";
        obj_error.style.display = 'none';
    }
}

function dangKytaiKhoan(){
    var obj_fullname = document.getElementById('input_fullname');
    validateObj(obj_fullname);
    var obj_email = document.getElementById('input_email');
    validateObj(obj_email);
    var obj_password = document.getElementById('input_password')
    validateObj(obj_password);
    var obj_fullname_confirm = document.getElementById('input_password_confirm');
    validateObj(obj_fullname_confirm);
}