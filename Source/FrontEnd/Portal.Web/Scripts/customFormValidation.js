$(function() {
    $('form').validate();
    $('form').bind('input change keyup', function() {
        if ($(this).valid()) {
            $(".input-validation-error2").hide();
            $(".bt_green_aver").removeClass("bt_dissabled").attr('disabled', false);
        } else {
            $(".input-validation-error2").show();
            $(".bt_green_aver").addClass("bt_dissabled").attr('disabled', true);
        }
    });
});