$('.btn-cong, .btn-tru').on('click', function(){
    var type = $(this).attr('data-type');
    var parent = $(this).parent();
    var input_slg = parent.find('.input_slg');
    var slg = input_slg.val();
    if(type == "cong"){
        slg = parseInt(slg) + 1;
    }else{
        if(slg > 1){
            slg = parseInt(slg) - 1;
        }
    }
    input_slg.val(slg);

    var price = input_slg.attr('data-price');
    var total_1_sp = slg * price;

    var parents = $(this).parents('.action');
    parents.find('.total_1_sp').text(formatPrice(total_1_sp)+'đ');
    var total = totalPrice();
    $('#total_price').text(formatPrice(total)+'đ');
});

$('.btn-xoa').on('click', function(){
    $(this).parents('.row').remove();
    var total = totalPrice();
    $('#total_price').text(formatPrice(total)+'đ');
});

function formatPrice(total){
    return total.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
}

function totalPrice(){
    var total = 0;
    $('.slg_sp').each(function(){
        var slg = $(this).find('.input_slg').val();
        var price = $(this).find('.input_slg').attr('data-price');
        var total_1_sp = slg * price;
        console.log(total_1_sp);
        total = total + parseInt(total_1_sp);
    });
    return total;
}