function dangnhaptaikhoan(){
    var objs = document.getElementsByClassName('form_input');
    for(var i = 0; i < objs.length; i++){
        var obj = objs[i];
        var obj_value = obj.value;
        var attr = obj.getAttribute('name');
        var obj_error = document.getElementById('error_'+attr);
        var field_name = obj.getAttribute('field-name');
        if(obj_value == ''){
            obj_error.innerHTML = 'Vui lòng nhập '+field_name;
            obj_error.style.display = 'block';
        }else{
            obj_error.innerHTML = '';
            obj_error.style.display = 'none';
        }
    }
}