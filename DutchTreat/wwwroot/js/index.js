var theForm = $("#theForm");
var $loginToggle = $("#loginToggle");
var $popupForm = $(".popup-form")

$loginToggle.on("click", function () {
    $popupForm.toggle();
});