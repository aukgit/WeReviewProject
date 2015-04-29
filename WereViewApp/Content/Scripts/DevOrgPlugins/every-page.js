
/*
 * Written by Alim Ul Karim
 * Developers Organism
 * https://www.facebook.com/DevelopersOrganism
 * mailto:info@developers-organism.com
*/


function transactionStatusHide() {
    var $transactionStatus = $.queryAll(".transaction-status");
    if ($transactionStatus.length > 0) {
        $transactionStatus.delay(3500).fadeOut(2500);
    }
}


$(function () {
    //var devBackBtns = $("a.dev-btn-back");
    //if (devBackBtns.length > 0) {
    //    devBackBtns.click(function (e) {
    //        e.preventDefault();
    //        history.back();
    //    });
    //} 
    $.queryAll('.tooltip-show').tooltip();
    


    transactionStatusHide();
});
