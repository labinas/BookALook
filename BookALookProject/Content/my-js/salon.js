$(document).ready(function () {
    $("#categorySelection").on("change", function () {
        var valueSelected = $("#categorySelection option:selected").data("category");
        $(".serviceOptionItem").each(function () {
            console.log( "soc " + $(this).data("category"));
            console.log(valueSelected);
            if ($(this).data("category") !== valueSelected) {
                $(this).hide();
                $(this).prop("disabled", true);
            }
            else {
                $(this).show();
                $(this).prop("disabled", false);
            }
        })
        $("#serviceList option:selected").prop("selected", false);

    })
});